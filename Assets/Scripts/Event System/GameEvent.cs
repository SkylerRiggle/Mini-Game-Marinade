// Title: GameEvent
// Author: Skyler Riggle

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple class used as an in-between for certain game actions and various listener
/// actions.
/// </summary>
[CreateAssetMenu (fileName = "NewEvent", menuName = "Create New Event")]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// A hash set of subscribed listeners. We use a hash set here to avoid duplicate entries.
    /// </summary>
    private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

    /// <summary>
    /// Subscrives a listener to this event.
    /// </summary>
    /// <param name="listener">The listener to be added.</param>
    /// <returns>The outcome of this operation. (True = Added | False = Already Present)</returns>
    public bool Register(GameEventListener listener) => listeners.Add(listener);

    /// <summary>
    /// Unsubscribes a listener from this event.
    /// </summary>
    /// <param name="listener">The listener to be removed.</param>
    /// <returns>The outome of this operation. (True = Removed | False = Not Found)</returns>
    public bool UnRegister(GameEventListener listener) => listeners.Remove(listener);

    /// <summary>
    /// Invokes all subscribed listeners.
    /// </summary>
    public void Invoke()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.Invoke();
        }
    }
}
