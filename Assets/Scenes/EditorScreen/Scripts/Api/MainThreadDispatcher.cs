using System;
using System.Collections.Concurrent;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static MainThreadDispatcher instance;
    private static readonly ConcurrentQueue<Action> executeOnMainThread = new ConcurrentQueue<Action>();

    public static void Enqueue(Action action)
    {
        if (instance == null)
        {
            Debug.LogError("MainThreadDispatcher not initialized!");
            return;
        }
        executeOnMainThread.Enqueue(action);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        while (executeOnMainThread.TryDequeue(out var action))
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}