using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class TilePosition
{
    public int x;
    public int y;

    // JsonUtility требует параметрless конструктор
    public TilePosition() { }

    public TilePosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

[Serializable]
public class TileBulkData
{
    public string tileName;
    public int brushSize;
    public string brushMode;
    public List<TilePosition> positions;

    public TileBulkData() { }

    public TileBulkData(string tileName, int brushSize, string brushMode, List<TilePosition> positions)
    {
        this.tileName = tileName;
        this.brushSize = brushSize;
        this.brushMode = brushMode;
        this.positions = positions;
    }
}

/// <summary>
/// Статический клиент для работы с сервером FastAPI
/// </summary>
public static class ApiClient
{
    private static string _baseDomain = "https://" + SettingConnection._baseDomain;
    private static string _apiBase = SettingConnection._apiBase;

    public static void SetDomain(string domain)
    {
        _baseDomain = domain.TrimEnd('/');
    }

    private static string BuildUrl(string path)
    {
        return $"{_baseDomain}{_apiBase}/{path.TrimStart('/')}";
    }

    public static async Task<string> PostAsync(string endpoint, string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);


        using (UnityWebRequest request = new UnityWebRequest(BuildUrl(endpoint), "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Content-Length", bodyRaw.Length.ToString()); // ← КЛЮЧЕВОЕ ИСПРАВЛЕНИЕ

            var op = request.SendWebRequest();
            while (!op.isDone)
                await Task.Yield();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                // Выводим не только ошибку, но и URL + JSON для отладки
                Debug.LogError($"POST failed to {BuildUrl(endpoint)}\nJSON: {json}\nError: {request.error}");
                return null;
            }

            return request.downloadHandler.text;
        }
    }

    public static async Task<bool> SendTilesAsync(TileBulkData data)
    {
        string json = JsonUtility.ToJson(data);
        string response = await PostAsync("tiles-bulk", json);
        return response != null;
    }

    public static async Task SendCurrentBrushAsync(string tileName, int brushSize, string brushMode, List<Vector2Int> positions)
    {
        var posList = new List<TilePosition>();
        foreach (var p in positions)
            posList.Add(new TilePosition(p.x, p.y));

        var data = new TileBulkData(tileName, brushSize, brushMode, posList);
        await SendTilesAsync(data);
    }
}