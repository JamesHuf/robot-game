// This struct stores the event names that will be used for the messenger
public struct GameEvent
{
    // UI Changed
    public const string POPUP_OPENED = "POPUP_OPENED";
    public const string POPUP_CLOSED = "POPUP_CLOSED";
    public const string UI_POPUP_OPENED = "UI_POPUP_OPENED";
    public const string UI_POPUP_CLOSED = "UI_POPUP_CLOSED";

    // Pickups
    public const string DOUBLE_POINTS = "DOUBLE_POINTS";
    public const string GRENADE_COUNT_CHANGED = "GRENADE_COUNT_CHANGED";

    // Game State
    public const string WAVE_STARTED = "WAVE_STARTED";
    public const string ENEMY_DEAD = "ENEMY_DEAD";
    public const string HEALTH_CHANGED = "HEALTH_CHANGED";
    public const string PLAYER_DEAD = "PLAYER_DEAD";
    public const string RESTART_GAME = "RESTART_GAME";
    public const string GAME_WON = "GAME_WON";
}
