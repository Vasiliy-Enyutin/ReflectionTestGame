using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PlayerRayDrawer : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    
    
    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawRay(IEnumerable<Vector3> positions, Vector3 rayStartPosition)
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, rayStartPosition);

        foreach (Vector3 position in positions)
        {
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position);
        }
    }
}
