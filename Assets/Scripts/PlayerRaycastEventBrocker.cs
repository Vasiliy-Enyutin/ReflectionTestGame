using System;
using UnityEngine;

public static class PlayerRaycastEventBrocker
{
    public static event Action<Vector3[]> OnEnemiesIsHit;

    public static event Action OnEnemiesIsNotHit;

    public static void InvokeEnemiesIsHit(Vector3[] hitPositions)
    {
        OnEnemiesIsHit?.Invoke(hitPositions);
    }

    public static void InvokeOnEnemiesIsNotHit()
    {
        OnEnemiesIsNotHit?.Invoke();
    }
}
