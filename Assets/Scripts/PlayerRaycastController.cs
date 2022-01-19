using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerRayDrawer))]
public class PlayerRaycastController : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToHit;
    [SerializeField] private Camera _rayStartPosition;
    [SerializeField] private int _reflectionsNumber;
    [SerializeField] private float _maxLength;

    private PlayerRayDrawer _playerRayDrawer;
    private Ray _ray;
    private RaycastHit _hit;
    private float remainingLength;
    
    private readonly List<Vector3> _enemyHitPositions = new List<Vector3>();
    readonly List<Vector3> _hitPositions = new List<Vector3>();


    private void Awake()
    {
        _playerRayDrawer = GetComponent<PlayerRayDrawer>();
    }

    private void Update()
    {
        UpdateRay();
    }

    private void UpdateRay()
    {
        _enemyHitPositions.Clear();
        
        _ray = new Ray(_rayStartPosition.transform.position, _rayStartPosition.transform.forward);
        remainingLength = _maxLength;
        _hitPositions.Clear();

        for (int i = 0; i <= _reflectionsNumber; i++)
        {
            if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLength, _layersToHit))
            {
                if (_hit.collider.TryGetComponent(out Enemy enemy))
                {
                    _enemyHitPositions.Add(_hit.point);
                }
                _hitPositions.Add(_hit.point);
                remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
            }
            else
            {
                _hitPositions.Add(_ray.origin + _ray.direction * remainingLength);
            }
        }

        _playerRayDrawer.DrawRay(_hitPositions, _rayStartPosition.transform.position);
        UpdateHitEvents();
    }

    private void UpdateHitEvents()
    {
        if (_enemyHitPositions.Count > 0)
            PlayerRaycastEventBrocker.InvokeEnemiesIsHit(_enemyHitPositions.ToArray());
        else
            PlayerRaycastEventBrocker.InvokeOnEnemiesIsNotHit();
    }
}
