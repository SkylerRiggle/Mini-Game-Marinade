// Title: DirectListener
// Author: Skyler Riggle

using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This listener directly performs a set of actions immediately once triggered.
/// </summary>
public class DirectListener : GameEventListener
{
    /// <summary>
    /// Method references for all tasks to be performed by this listener.
    /// </summary>
    [SerializeField] private UnityEvent[] unityEvents = new UnityEvent[0];

    /// <summary>
    /// Calls all of the attached unity events directly.
    /// </summary>
    public override void Invoke()
    {
        foreach (UnityEvent unityEvent in unityEvents)
        {
            unityEvent.Invoke();
        }
    }
}
