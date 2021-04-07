using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MapElement
{
    public LineRenderer lineRenderer;
    public void UpdateLineRenderer(Vector3[] points)
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

}
