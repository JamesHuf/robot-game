using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an event/messenger system intended to allow better abstraction between
/// various components of the game. It uses C# generics to allow listeners to listen
/// to a specific type of event which they want notified about.
/// </summary>
public abstract class EventSystem<T> where T : EventSystem<T>
{
    // Base event attributes - May not be needed
    public string eventDescription;

    private bool hasFired = false;
    public delegate void EventListener(T info);
    private static event EventListener EventListeners;

    /// <summary>
    /// Register a function as a listener for a specific type of event
    /// This should normally be called in OnEnable(), but if it's not a
    /// monobehaviour it may need to be called in the constructor instead
    /// Ex: UnitDiedEvent.Register(HandleUnitDeath);
    /// </summary>
    public static void Register(EventListener listener)
    {
        EventListeners += listener;
    }

    // Unregister a function as a listener for an event
    // This should normally be called in OnDisable(), but if it's not a monobehaviour
    // it may need to become an IDisposable with this called in dispose()
    public static void Unregister(EventListener listener)
    {
        EventListeners -= listener;
    }

    /// <summary>
    /// Triggering an event to alert all listeners is as easy as:
    /// new EventType(eventParameters).Fire();
    /// </summary>
    public void Fire()
    {
        if (hasFired)
        {
            throw new System.Exception("Event has been called twice. In order to prevent infinite loops this is disallowed.");
        }
        hasFired = true;

        EventListeners?.Invoke(this as T);
    }
}