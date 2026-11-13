using System;
using UnityEngine;

public class GameEvents
{
    public static event Action OnGravityFlipped;
    public static event Action<bool> OnGravityFlippedStateChanged;
	public static event Action<int> OnZoneChanged;
    public static event Action<Vector3, bool> OnCheckPointReached;
    public static event Action OnPlayerDied;
    public static event Action OnPlayerRespawned;

	public static event Action OnPauseToggleRequested;
    public static event Action<bool> OnPauseStateChanged;

    public static event Action OnLevelCompleted;
    public static event Action OnNextLevelRequested;
    public static event Action OnExitToMainMenuRequested;


    public static void GravityFlipped() => OnGravityFlipped?.Invoke();
	public static void GravityFlippedStateChanged(bool isFlipped) => OnGravityFlippedStateChanged?.Invoke(isFlipped);
	public static void ZoneChanged(int zone) => OnZoneChanged?.Invoke(zone);
    public static void CheckPointReached(Vector3 position, bool isFlipped) => OnCheckPointReached?.Invoke(position, isFlipped);
	public static void PlayerDied() => OnPlayerDied?.Invoke();
	public static void PlayerRespawned() => OnPlayerRespawned?.Invoke();

	public static void RequestPauseToggle() => OnPauseToggleRequested?.Invoke();
    public static void NotifyPauseStateChanged(bool isPaused) => OnPauseStateChanged?.Invoke(isPaused);

    public static void EndLevel() => OnLevelCompleted?.Invoke();
    public static void RequestNextLevel() => OnNextLevelRequested?.Invoke();
    public static void RequestExitToMainMenu() => OnExitToMainMenuRequested?.Invoke();
}
