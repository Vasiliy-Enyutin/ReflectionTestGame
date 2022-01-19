using UnityEngine;

public class PlayerHintController : MonoBehaviour
{
    [SerializeField] private PlayerHint _hintPrefab;
    [SerializeField] private int _poolDefaultCount;
    [SerializeField] private bool _autoExpand;
    [SerializeField] private Transform _container;

    private PoolMonobehavior<PlayerHint> _hints;


    private void Awake()
    {
        _hints = new PoolMonobehavior<PlayerHint>(_hintPrefab, _poolDefaultCount, _autoExpand, _container);
    }

    private void Start()
    {
        PlayerRaycastEventBrocker.OnEnemiesIsHit += MoveHints;
        PlayerRaycastEventBrocker.OnEnemiesIsNotHit += DeactivateHints;
    }

    private void OnDisable()
    {
        PlayerRaycastEventBrocker.OnEnemiesIsHit -= MoveHints;
        PlayerRaycastEventBrocker.OnEnemiesIsNotHit -= DeactivateHints;
    }

    private void MoveHints(Vector3[] positionsForSpawn)
    {
        DeactivateHints();
        foreach (Vector3 newPosition in positionsForSpawn)
        {
            _hints.GetFreeElement().transform.position = newPosition;
        }
    }

    private void DeactivateHints()
    {
        _hints.DeactivateObjects();
    }
}
