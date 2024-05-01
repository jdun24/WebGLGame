using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;

public class SatelliteController : MonoBehaviour
{
    [Header("Events")]

    [Header("Mutable")]
    [SerializeField] private Rigidbody2D satelliteBody;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private GameObject parentAsteroid;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;

    private Vector2[] edgePoints;
    private int currentTargetIndex = 0;
    private float movingSpeed = 2.5f;
    private float progressBetweenPoints = 0f; // Tracks interpolation progress between points

    private enum State { Autopilot, Manual }
    private State currentState;

    private bool isManualControlEnabled = false;

    // SatelliteScan Component for handling scanning
    SatelliteScan satelliteScan;

    // -------------------------------------------------------------------
    // Handle events

    public void OnSatelliteControlToggle()
    {
        if (SatelliteManager.Instance.IsSatelliteControlUnlocked())
        {
            // Conditional state switching: only if this is the current satellite
            if (SatelliteManager.Instance.GetCurrentSatelliteObject() == this.gameObject)
            {
                State nextState = isManualControlEnabled ? State.Autopilot : State.Manual;
                ChangeState(nextState);
            }
        }
    }

    public void OnSatelliteMove(UnityEngine.Vector2 direction)
    {
        horizontalInput = direction.x;

        if(Mathf.Approximately(satelliteBody.velocity.magnitude, 0))
        {
            satelliteScan.SetIsScanningAllowed(true);
        }
        else
        {
            satelliteScan.SetIsScanningAllowed(false);
        }
    }

    // -------------------------------------------------------------------
    // API

    public void SetParentAsteroid(GameObject asteroid)
    {
        parentAsteroid = asteroid;
    }

    // -------------------------------------------------------------------
    // Base

    void Start()
    {
        satelliteScan = gameObject.GetComponent<SatelliteScan>();
        satelliteScan.SetParentAsteroid(parentAsteroid);
        satelliteScan.SetIsScanningAllowed(true);

        UpdateEdgePoints();
        currentState = State.Autopilot;
    }

    void Update()
    {
        if (edgePoints == null || edgePoints.Length == 0) return;

        // Always perform autonomous movement and rotation updates
        if (currentState == State.Autopilot)
        {
            MoveAlongEdge();
        }

        // Conditional manual movement: only if this is the current satellite
        if (SatelliteManager.Instance.GetCurrentSatelliteObject() == this.gameObject)
        {
            if (currentState == State.Manual)
            {
                ManualMove(-horizontalInput);
            }
        }

        // Update rotation is always called regardless of which satellite is active
        UpdateRotation();
    }

    // -------------------------------------------------------------------
    // Functions

    void UpdateEdgePoints()
    {
        GravityFieldEdgePoints fieldPoints = parentAsteroid.GetComponentInChildren<GravityFieldEdgePoints>();
        if (fieldPoints != null)
        {
            edgePoints = fieldPoints.edgePoints;
            currentTargetIndex = FindClosestEdgePointIndex(edgePoints,PlayerManager.Instance.GetPlayerPosition());
        }
    }

    int FindClosestEdgePointIndex(Vector2[] points, Vector2 position)
    {
        int closestIndex = 0;
        float minDistance = float.MaxValue;
        for (int i = 0; i < points.Length; i++)
        {
            float dist = Vector2.Distance(points[i], position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
        isManualControlEnabled = (newState == State.Manual);

        if (currentState == State.Manual)
        {
            RecalculateProgress();
        }

        // Always reset the velocity when changing states to avoid sudden jumps
        satelliteBody.velocity = Vector2.zero;
    }

    private void RecalculateProgress()
    {
        if (currentTargetIndex >= edgePoints.Length || edgePoints.Length < 2) return;

        Vector2 currentPoint = edgePoints[currentTargetIndex];
        Vector2 nextPoint = edgePoints[(currentTargetIndex + 1) % edgePoints.Length];
        Vector2 position = satelliteBody.position;

        // Calculate the projection of the current position on the line segment from currentPoint to nextPoint
        Vector2 lineDir = nextPoint - currentPoint;
        float lineLength = lineDir.magnitude;
        Vector2 lineDirNormalized = lineDir / lineLength;

        // Project the current position onto the line direction
        float projection = Vector2.Dot(position - currentPoint, lineDirNormalized);
        projection = Mathf.Clamp(projection, 0, lineLength); // Clamping to stay within the segment

        progressBetweenPoints = projection / lineLength;

        // Correct the currentTargetIndex if necessary
        if (projection == 0)
        {
            // Check if it should snap to the previous segment
            currentTargetIndex = (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length;
            nextPoint = currentPoint;
            currentPoint = edgePoints[currentTargetIndex];
            lineDir = nextPoint - currentPoint;
            lineLength = lineDir.magnitude;
            lineDirNormalized = lineDir.normalized;
            projection = Vector2.Dot(position - currentPoint, lineDirNormalized);
            progressBetweenPoints = projection / lineLength;
        }
        else if (projection == lineLength)
        {
            // Move to the next segment
            currentTargetIndex = (currentTargetIndex + 1) % edgePoints.Length;
            progressBetweenPoints = 0;
        }
    }

    private void ManualMove(float input)
    {
        if (currentTargetIndex >= edgePoints.Length) return;

        // Interpolate between the current and next point based on player input
        progressBetweenPoints += input * Time.deltaTime * movingSpeed;
        progressBetweenPoints = Mathf.Clamp01(progressBetweenPoints);

        Vector2 currentPoint = edgePoints[currentTargetIndex];
        Vector2 nextPoint = edgePoints[(currentTargetIndex + 1) % edgePoints.Length];
        satelliteBody.position = Vector2.Lerp(currentPoint, nextPoint, progressBetweenPoints);

        if (progressBetweenPoints >= 1)
        {
            currentTargetIndex = (currentTargetIndex + 1) % edgePoints.Length;
            progressBetweenPoints = 0;
        }
        else if (progressBetweenPoints <= 0)
        {
            currentTargetIndex = (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length;
            progressBetweenPoints = 1;
        }
    }

    void MoveAlongEdge()
    {
        if (edgePoints == null || edgePoints.Length < 2) return;  // Ensure there are at least two points to form a segment

        if (currentTargetIndex < 0 || currentTargetIndex >= edgePoints.Length)
        {
            Debug.LogError("CurrentTargetIndex is out of range: " + currentTargetIndex);
            return;
        }

        // Safely calculate the next index with wrapping
        int nextIndex = (currentTargetIndex - 1 + edgePoints.Length) % edgePoints.Length;

        if (currentTargetIndex < 0 || currentTargetIndex >= edgePoints.Length)
        {
            Debug.LogError("CurrentTargetIndex is out of range: " + currentTargetIndex);
            return;  // Add this check to prevent accessing the array with an invalid index
        }

        if (edgePoints == null || edgePoints.Length < 2)
        {
            Debug.LogError("Edge points array is null or does not contain enough points.");
            return;
        }

        Vector2 currentPoint = edgePoints[currentTargetIndex];
        Vector2 nextPoint = edgePoints[nextIndex];
        float step = movingSpeed * Time.deltaTime;

        satelliteBody.position = Vector2.MoveTowards(satelliteBody.position, nextPoint, step);

        if (Vector2.Distance(satelliteBody.position, nextPoint) < 0.01f)
        {
            currentTargetIndex = nextIndex;
            progressBetweenPoints = 0; // Reset progress when the point is reached
        }
    }

    private void UpdateRotation()
    {
        Vector3 directionToCenter = (transform.position - parentAsteroid.transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToCenter);
    }
}
