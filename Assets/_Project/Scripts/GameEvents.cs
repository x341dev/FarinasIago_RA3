using System;
using UnityEngine;

public class GameEvents
{
    public static event Action<int> OnCoinCollected;
    public static event Action OnGravityFlipped;
    public static event Action<int> OnZoneChanged;

	public static void CoinCollected(int amount) => OnCoinCollected?.Invoke(amount);
    public static void GravityFlipped() => OnGravityFlipped?.Invoke();
    public static void ZoneChanged(int zone) => OnZoneChanged?.Invoke(zone);
}
