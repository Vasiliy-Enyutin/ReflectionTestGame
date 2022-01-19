using System.Collections.Generic;
using UnityEngine;

public class PlayerHintController : MonoBehaviour
{
    [SerializeField] private PlayerHint _hintPrefab;
    
    private readonly Dictionary<Enemy, PlayerHint> _enemies = new Dictionary<Enemy, PlayerHint>();


    private void OnDisable()
    {
        PlayerRaycastEventBroker.OnEnemyIsHit -= UpdateHint;
    }

    private void Start()
    {
        PlayerRaycastEventBroker.OnEnemyIsHit += UpdateHint;
        PlayerRaycastEventBroker.OnEnemyIsNotHitAnymore += UnregisterHint;
    }

    private void UpdateHint(Enemy enemy, Vector3 hitPosition)
    {
        if (_enemies.ContainsKey(enemy))
            UpdateHintPosition(_enemies[enemy], hitPosition);
        else
            RegisterHint(enemy, hitPosition);
    }

    private void UpdateHintPosition(PlayerHint hint, Vector3 newHitPosition)
    {
        hint.transform.position = newHitPosition;
    }
    
    private void RegisterHint(Enemy enemy, Vector3 hitPosition)
    {
        PlayerHint hint = Instantiate(_hintPrefab, hitPosition, Quaternion.identity);
        _enemies.Add(enemy, hint);
    }
    
    private void UnregisterHint(Enemy enemy)
    {
        if (_enemies.ContainsKey(enemy))
        {
            if (_enemies.TryGetValue(enemy, out PlayerHint hint))
            {
                hint.Destroy();
            }
            _enemies.Remove(enemy);
        }
    }
}
