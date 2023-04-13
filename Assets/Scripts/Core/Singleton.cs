// Title: Singleton
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// A singleton represents a class with a single static and
/// persistent instance.
/// </summary>
public class Singleton<T> : MonoBehaviour where T: Component
{
    private static T _instance;
    public static T instance { get { return _instance; } }

    /// <summary>
    /// In the awake method, we enforce the singleton pattern.
    /// </summary>
    private void Awake() 
    {
        // Check to see if an instance already exists.
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, implement this instance as the singleton
        _instance = this as T;
    }
}
