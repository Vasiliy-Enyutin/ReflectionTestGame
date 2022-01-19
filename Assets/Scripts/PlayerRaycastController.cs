using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycastController : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToHit;
    [SerializeField] private Camera _rayStartPosition;
    [SerializeField] private int _reflectionsNumber;
    [SerializeField] private float _maxLength;

    private Ray _ray;
    private RaycastHit _hit;

    private readonly List<Enemy> _previouslyHitEnemies = new List<Enemy>();
    private readonly List<Enemy> _currentHitEnemies = new List<Enemy>();


    private void Update()
    {
        UpdateRay();
    }

    private void UpdateRay()
    {
        RefreshHitEnemies();
        _ray = new Ray(_rayStartPosition.transform.position, _rayStartPosition.transform.forward);
        float remainingLength = _maxLength;

        for (int i = 0; i < _reflectionsNumber; i++)
        {
            if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLength, _layersToHit))
            {
                if (_hit.collider.TryGetComponent(out Enemy enemy))
                {
                    _currentHitEnemies.Add(enemy);
                    PlayerRaycastEventBroker.InvokeOnEnemyIsHit(enemy, _hit.point);
                }
                remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
            }

            ComparePreviouslyHitEnemies();
        }
    }

    private void RefreshHitEnemies()
    {
        _previouslyHitEnemies.Clear();
        for (int i = 0; i < _currentHitEnemies.Count; i++)
        {
            _previouslyHitEnemies.Add(_currentHitEnemies[i]);
        }
        _currentHitEnemies.Clear();
    }

    private void ComparePreviouslyHitEnemies()
    {
        for (int i = 0; i < _previouslyHitEnemies.Count; i++)
        {
            bool isMatch = false;
            for (int j = 0; j < _currentHitEnemies.Count; j++)
            {
                if (_previouslyHitEnemies[i] == _currentHitEnemies[j])
                {
                    isMatch = true;
                    break;
                }
            }

            if (isMatch == true)
            {
                continue;
            }
            
            PlayerRaycastEventBroker.InvokeOnEnemyIsNotHitAnymore(_previouslyHitEnemies[i]);  
        }
    }
}
