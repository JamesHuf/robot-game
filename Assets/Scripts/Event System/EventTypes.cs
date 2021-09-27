using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This file defines the various game events which are children of the Event system class.
/// Sample event:
/// public class UnitDiedEvent : EventSystem<UnitDiedEvent>
/// {
///     public readonly Unit deadUnit;
///     public readonly Unit killer;
///     public readonly string diedFrom;
///
///     public UnitDiedEvent(Unit deadUnit, Unit killer, string diedFrom;)
///     {
///         eventDescription = "Creature Died";
///
///         this.deadUnit = deadUnit;
///         this.killer = killer;
///         this.diedFrom = diedFrom;
///     }
/// }
/// </summary>

/********************************  UI EVENTS  ********************************/
public class PopupOpenedEvent : EventSystem<PopupOpenedEvent>
{
    public PopupOpenedEvent()
    {
        eventDescription = "Popup opened";
    }
}

public class PopupClosedEvent : EventSystem<PopupClosedEvent>
{
    public PopupClosedEvent()
    {
        eventDescription = "Popup closed";
    }
}

/********************************  PICKUPS  ********************************/
public class DoublePointsEvent : EventSystem<DoublePointsEvent>
{
    public DoublePointsEvent()
    {
        eventDescription = "Double points activated";
    }
}

public class GrenadeCountChangedEvent : EventSystem<GrenadeCountChangedEvent>
{
    public readonly int newGrenadeCount;

    public GrenadeCountChangedEvent(int newGrenadeCount)
    {
        eventDescription = "Grenade count changed";
        this.newGrenadeCount = newGrenadeCount;
    }
}

/********************************  PLAYER STATE  ********************************/
public class PlayerHealthChangedEvent : EventSystem<PlayerHealthChangedEvent>
{
    public readonly float newHealth;

    public PlayerHealthChangedEvent(float newHealth)
    {
        eventDescription = "Players health changed";
        this.newHealth = newHealth;
    }
}

public class PlayerDeadEvent : EventSystem<PlayerDeadEvent>
{
    public PlayerDeadEvent()
    {
        eventDescription = "Player has died";
    }
}

/********************************  GAME STATE  ********************************/
public class WaveStartedEvent : EventSystem<WaveStartedEvent>
{
    public readonly int currWave;

    public WaveStartedEvent(int currWave)
    {
        eventDescription = "New wave started";
        this.currWave = currWave;
    }
}

public class EnemyDeadEvent : EventSystem<EnemyDeadEvent>
{
    public EnemyDeadEvent()
    {
        eventDescription = "Enemy has died";
    }
}

public class GameRestartEvent : EventSystem<GameRestartEvent>
{
    public GameRestartEvent()
    {
        eventDescription = "Game was restarted";
    }
}

public class GameWonEvent : EventSystem<GameWonEvent>
{
    public GameWonEvent()
    {
        eventDescription = "Game was won by player(s)";
    }
}