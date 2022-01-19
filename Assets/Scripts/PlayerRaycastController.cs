using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerRaycastController : MonoBehaviour
{
    [SerializeField] private LayerMask _layersToHit;
    [SerializeField] private Camera _rayStartPosition;
    [SerializeField] private int _reflectionsNumber;
    [SerializeField] private float _maxLength;

    private LineRenderer _lineRenderer;
    private Ray _ray;
    private RaycastHit _hit;
    private float remainingLength;
    
    private readonly List<Vector3> _enemyHitPositions = new List<Vector3>();


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateRay();
        DrawRay();
    }

    private void UpdateRay()
    {
        _enemyHitPositions.Clear();
        
        _ray = new Ray(_rayStartPosition.transform.position, _rayStartPosition.transform.forward);
        remainingLength = _maxLength;

        for (int i = 0; i < _reflectionsNumber; i++)
        {
            if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLength, _layersToHit))
            {
                if (_hit.collider.TryGetComponent(out Enemy enemy))
                {
                    _enemyHitPositions.Add(_hit.point);
                }
                remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
            }
        }
        
        UpdateHitEvents();
    }

    private void UpdateHitEvents()
    {
        if (_enemyHitPositions.Count > 0)
            PlayerRaycastEventBrocker.InvokeEnemiesIsHit(_enemyHitPositions.ToArray());
        else
            PlayerRaycastEventBrocker.InvokeOnEnemiesIsNotHit();
    }
    
    private void DrawRay()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _rayStartPosition.transform.position);
        
        _ray = new Ray(_rayStartPosition.transform.position, _rayStartPosition.transform.forward);
        remainingLength = _maxLength;
        
        for (int i = 0; i < _reflectionsNumber; i++)
        {
            _lineRenderer.positionCount++;
            if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLength, _layersToHit))
            {
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _hit.point);
                remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                _ray = new Ray(_hit.point, Vector3.Reflect(_ray.direction, _hit.normal));
            }
            else
            {
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
            }
        }
    }
}
