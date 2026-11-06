using System;
using UnityEngine;

public class GameEvents
{
    public static event Action OnGravityFlipped;
    public static event Action<bool> OnGravityFlippedStateChanged;
	public static event Action<int> OnZoneChanged;
    public static event Action<Vector3, bool> OnCheckPointReached;
    public static event Action OnPlayerDied;


    public static void GravityFlipped() => OnGravityFlipped?.Invoke();
	public static void GravityFlippedStateChanged(bool isFlipped) => OnGravityFlippedStateChanged?.Invoke(isFlipped);
	public static void ZoneChanged(int zone) => OnZoneChanged?.Invoke(zone);
    public static void CheckPointReached(Vector3 position, bool isFlipped) => OnCheckPointReached?.Invoke(position, isFlipped);
	public static void PlayerDied() => OnPlayerDied?.Invoke();
}
