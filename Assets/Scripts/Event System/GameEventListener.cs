// Title: GameEventListener
// Author: Skyler Riggle

using UnityEngine;

/// <summary>
/// The GameEventListener class acts as a base for all other listener classes.
/// This base class handles subscription to an event, and gives other instances 
/// an entry point to trigger this listener.
/// </summary>
public abstract class GameEventListener : MonoBehaviour
{
    /// <summary>
    /// The game event that this listener will subscribe to.
    /// </summary>
    [SerializeField] private GameEvent gameEvent = null;

    /// <summary>
    /// Subscribes the associated event
    /// </summary>
    public void Awake() => gameEvent.Register(this);

    /// <summary>
    /// When the attached entity is destroyed, unsubscribe from the associated event.
    /// </summary>
    public void OnDestroy() => gameEvent.UnRegister(this);

    /// <summary>
    /// Handles the listener's core functionality.
    /// </summary>
    public abstract void Invoke();
}
