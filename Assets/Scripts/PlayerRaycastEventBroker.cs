using System;
using UnityEngine;

public static class PlayerRaycastEventBroker
{
    public static event Action<Enemy, Vector3> OnEnemyIsHit;

    public static event Action<Enemy> OnEnemyIsNotHitAnymore;

    public static void InvokeOnEnemyIsHit(Enemy enemy, Vector3 hitPosition)
    {
        OnEnemyIsHit?.Invoke(enemy, hitPosition);
    }

    public static void InvokeOnEnemyIsNotHitAnymore(Enemy enemy)
    {
        OnEnemyIsNotHitAnymore?.Invoke(enemy);
    }
}
