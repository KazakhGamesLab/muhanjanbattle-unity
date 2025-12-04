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
    private static string _baseDomain = "http://" + SettingConnection._baseDomain;
    private static string _apiBase = SettingConnection._apiBase;

    /// <summary>
    /// Установить базовый домен, чтобы не указывать каждый раз
    /// </summary>
    public static void SetDomain(string domain)
    {
        _baseDomain = domain.TrimEnd('/');
    }

    private static string BuildUrl(string path)
    {
        return $"{_baseDomain}{_apiBase}/{path.TrimStart('/')}";
    }

    /// <summary>
    /// Отправка JSON на любой эндпоинт POST
    /// </summary>
    public static async Task<string> PostAsync(string endpoint, string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(BuildUrl(endpoint), "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone)
                await Task.Yield();

#if UNITY_2020_1_OR_NEWER
            if (request.result != UnityWebRequest.Result.Success)
#else
            if (request.isNetworkError || request.isHttpError)
#endif
            {
                Debug.LogError($"ApiClient POST error: {request.error}");
                return null;
            }

            return request.downloadHandler.text;
        }
    }

    /// <summary>
    /// Специализированный метод для отправки тайлов на /tiles-bulk
    /// </summary>
    public static async Task<bool> SendTilesAsync(TileBulkData data)
    {
        string json = JsonUtility.ToJson(data);
        string response = await PostAsync("tiles-bulk", json);
        if (response != null)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Удобный метод для текущей кисти и позиций
    /// </summary>
    public static async Task SendCurrentBrushAsync(string tileName, int brushSize, string brushMode, List<Vector2Int> positions)
    {
        List<TilePosition> posList = new List<TilePosition>();
        foreach (var p in positions)
            posList.Add(new TilePosition(p.x, p.y));

        TileBulkData data = new TileBulkData(tileName, brushSize, brushMode, posList);
        await SendTilesAsync(data);
    }
}
