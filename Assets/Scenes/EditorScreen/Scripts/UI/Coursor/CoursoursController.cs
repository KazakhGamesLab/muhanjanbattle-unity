using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoursoursController : MonoBehaviour
{
    [SerializeField] private GameObject _coursourPrefab;
    [SerializeField] private Transform _rootContainer;

    [SerializeField]
    private string _localUsername;
    [SerializeField]
    private Dictionary<string, GameObject> _activeCoursours = new();


    public void SetLocalUsername(string username)
    {
        if (!string.IsNullOrEmpty(_localUsername))
        {
            Debug.LogWarning("Local username already set!");
            return;
        }

        _localUsername = username;
        CreateCursorForPlayer(_localUsername, isLocal: true);
    }

    public void CreateCursorForPlayer(string username, bool isLocal = false)
    {
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username is null or empty!");
            return;
        }

        if (_activeCoursours.ContainsKey(username))
        {
            Debug.LogWarning($"Cursor for '{username}' already exists.");
            return;
        }

        GameObject cursorObj = Instantiate(_coursourPrefab, _rootContainer);
        cursorObj.name = $"Cursor_{username}_{_activeCoursours.Count}";

        var cursorComponent = cursorObj.GetComponent<CursorView>();
        if (cursorComponent != null)
        {
            cursorComponent.SetUsername(username);
            cursorComponent.IsLocalPlayer = isLocal;
        }

        _activeCoursours[username] = cursorObj;
    }

    public void RemoveCursorForPlayer(string username)
    {
        if (_activeCoursours.TryGetValue(username, out GameObject cursorObj))
        {
            Destroy(cursorObj);
            _activeCoursours.Remove(username);
        }
    }

    public GameObject GetCursor(string username)
    {
        _activeCoursours.TryGetValue(username, out GameObject cursor);
        return cursor;
    }

    public void ClearAllCursors()
    {
        foreach (var kvp in _activeCoursours)
        {
            Destroy(kvp.Value);
        }
        _activeCoursours.Clear();
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        EventsManager.MoveCoursour(mousePos, _localUsername);
    }
}