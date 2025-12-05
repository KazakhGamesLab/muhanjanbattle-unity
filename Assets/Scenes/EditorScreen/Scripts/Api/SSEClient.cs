using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"Items\":" + json + "}");
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}

public class SSEClient : MonoBehaviour
{
    private CancellationTokenSource cts;

    private string TilesUrl => $"https://{SettingConnection._baseDomain}{SettingConnection._apiBase}/tiles";
    private string StreamUrl => $"https://{SettingConnection._baseDomain}{SettingConnection._apiBase}/tiles-stream";

    void Start()
    {
        Debug.Log("Initializing tile sync...");
        cts = new CancellationTokenSource();
        _ = InitializeTilesAndStream(cts.Token);
    }

    private async Task InitializeTilesAndStream(CancellationToken ct)
    {
        try
        {
            await LoadInitialTiles(cts.Token).ConfigureAwait(false);

            await ListenToStream(cts.Token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Initialization failed: {ex.Message}");
        }
    }

    private async Task LoadInitialTiles(CancellationToken ct)
    {
        using var httpClient = new HttpClient();
        try
        {
            var response = await httpClient.GetAsync(TilesUrl, ct).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            TileDataSerializable[] tiles = JsonHelper.FromJson<TileDataSerializable>(json);
            foreach (var tile in tiles)
            {
                MainThreadDispatcher.Enqueue(() => EventsManager.GetTilesJson(JsonUtility.ToJson(tile)));
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to load initial tiles: {ex.Message}");
        }
    }



    private async Task ListenToStream(CancellationToken ct)
    {
        using var httpClient = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, StreamUrl);
        request.Headers.Add("Accept", "text/event-stream");
        request.Headers.Add("Cache-Control", "no-cache");

        try
        {
            using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct)
                .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            string eventType = string.Empty;
            var dataLines = new List<string>();

            while (!ct.IsCancellationRequested)
            {
                string line = await reader.ReadLineAsync();

                if (line.StartsWith("event:"))
                {
                    eventType = line["event:".Length..].Trim();
                }
                else if (line.StartsWith("data:"))
                {
                    dataLines.Add(line["data:".Length..]);
                }
                else if (string.IsNullOrWhiteSpace(line))
                {
                    if (dataLines.Count > 0 && eventType == "tile_event")
                    {
                        string jsonData = string.Join("\n", dataLines).Trim();
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            var eventData = new EventData(eventType, jsonData);
                            MainThreadDispatcher.Enqueue(() => EventsManager.SSEEventHandler(eventData));
                        }
                    }
                    eventType = string.Empty;
                    dataLines.Clear();
                }
            }

            if (dataLines.Count > 0)
            {
                string jsonData = string.Join("\n", dataLines).TrimEnd('\n');
                if (!string.IsNullOrEmpty(jsonData))
                {
                    var eData = new EventData(eventType, jsonData);
                    MainThreadDispatcher.Enqueue(() => EventsManager.SSEEventHandler(eData));
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"SSE error: {ex.Message}");
        }
    }

    

    void OnDestroy()
    {
        cts?.Cancel();
    }
}