using System;
using UnityEngine;

public class GravityFieldEdgePoints : MonoBehaviour
{
    private CircleCollider2D gravityFieldCollider;
    public Vector2[] edgePoints;  // Array to store edge points
    private int numberOfEdgePoints = 64;  // Number of points around the edge (adjustable for granularity)
    private bool edgePointsCalculated = false;  // Flag to ensure edge points are calculated only once

    public void InitializeEdgePoints()
    {
        if (!edgePointsCalculated)
        {
            gravityFieldCollider = GetComponent<CircleCollider2D>(); // Directly get the component on this GameObject
            if (gravityFieldCollider == null)
            {
                Debug.LogError("Gravity Field Collider not found on: " + this.gameObject.name);
                return;
            }

            GenerateEdgePoints();
            edgePointsCalculated = true;
        }
    }

    private void GenerateEdgePoints()
    {
        // Debug.Log("GenerateEdgePoints called.");
        edgePoints = new Vector2[numberOfEdgePoints];
        float angleStep = 360f / numberOfEdgePoints;
        float angle = 0;  // Start angle
        Vector2 fieldCenter = gravityFieldCollider.transform.position;
        float effectiveRadius = gravityFieldCollider.radius * Mathf.Max(gravityFieldCollider.transform.lossyScale.x, gravityFieldCollider.transform.lossyScale.y); // Effective radius including scale

        for (int i = 0; i < numberOfEdgePoints; i++)
        {
            edgePoints[i] = new Vector2(
                fieldCenter.x + effectiveRadius * Mathf.Cos(angle * Mathf.Deg2Rad),
                fieldCenter.y + effectiveRadius * Mathf.Sin(angle * Mathf.Deg2Rad)
            );
            angle += angleStep;
            // Debug.Log($"Edge Point {i}: {edgePoints[i]}");
        }
    }

    void FixedUpdate()
    {
        if (edgePoints != null && edgePoints.Length > 0 && gravityFieldCollider != null)
        {
            Vector2 fieldCenter = gravityFieldCollider.transform.position; // Center of the gravity field
            foreach (Vector2 point in edgePoints)
            {
                Debug.DrawLine(fieldCenter, point, Color.red); // Draw line from center to each edge point
            }
        }
    }
}
