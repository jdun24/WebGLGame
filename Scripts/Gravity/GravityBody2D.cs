using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody2D : MonoBehaviour
{
    [SerializeField, ReadOnly] private const float GRAVITY_FORCE = 500;
    [SerializeField] private bool gravityApplied;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField, ReadOnly] private float distanceToGround = 0;
    [SerializeField] private LayerMask gravityFieldLayer;
    private float maxGravityDistance = 10f;
    private float minRotationSpeed = 1f;
    private float maxRotationSpeed = 5f;
    [SerializeField, ReadOnly] private float currentRotationSpeed = 0f;

    public Vector2 GravityDirection
    {
        get
        {
            if (gravityAreas.Count == 0) return Vector2.zero;
            gravityAreas.Sort((area1, area2) => area1.Priority.CompareTo(area2.Priority));
            return gravityAreas.Last().GetGravityDirection(this).normalized;
        }
    }

    private Rigidbody2D rigidBody2D;
    private List<GravityArea2D> gravityAreas;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        gravityAreas = new List<GravityArea2D>();
    }

    private float calculateDistanceToGround()
    {
        Vector2 rayDirection = GravityDirection;
        // Use a large initial value to ensure the ray reaches the ground
        float initialRaycastDistance = 100f;  // Arbitrary large value to cover the maximum expected distance

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, initialRaycastDistance, groundLayer);

        if (hit.collider != null)
        {
            // Draw Raycast for the actual hit distance for debugging
            Debug.DrawRay(transform.position, rayDirection * hit.distance, Color.red);
            distanceToGround = hit.distance;
            return hit.distance;
        }
        else
        {
            // Draw Raycast for the expected maximum distance if no hit is detected
            Debug.DrawRay(transform.position, rayDirection * initialRaycastDistance, Color.red);
            return initialRaycastDistance;
        }
    }

    private float GetDistanceFromOutsideGravityFieldToPlayer()
    {
        Vector2 rayDirection = GravityDirection; // Direction towards the player
        float externalStartOffset = 100f; // Distance to start the ray outside the gravity field
        float maxRayDistance = 100f; // Maximum distance that the ray will travel

        // Calculate the start position: slightly outside the expected boundary of the gravity field
        Vector2 startPosition = (Vector2)transform.position - rayDirection * externalStartOffset;

        // Cast a ray from outside towards the player
        RaycastHit2D hit = Physics2D.Raycast(startPosition, rayDirection, maxRayDistance, gravityFieldLayer);

        if (hit.collider != null)
        {
            // Draw the ray for debugging
            Debug.DrawRay(startPosition, rayDirection * (hit.distance), Color.blue);

            // Calculate the distance from the collider hit point to the player's current position
            float distanceToPlayer = (hit.point - (Vector2)transform.position).magnitude;
            return distanceToPlayer;
        }
        else
        {
            // If no collision is detected, draw the full length ray
            Debug.DrawRay(startPosition, rayDirection * maxRayDistance, Color.blue);
            return maxRayDistance; // Return the maximum distance if no boundary is detected
        }
    }


    private void FixedUpdate()
    {
        
        if (gravityApplied)
            rigidBody2D.AddForce(GravityDirection * (GRAVITY_FORCE * Time.fixedDeltaTime), ForceMode2D.Force);

        calculateDistanceToGround();
        currentRotationSpeed = Mathf.Lerp(maxRotationSpeed, minRotationSpeed, distanceToGround / maxGravityDistance);

        float targetAngle = Mathf.Atan2(GravityDirection.y, GravityDirection.x) * Mathf.Rad2Deg + 90;
        float smoothedAngle = Mathf.LerpAngle(rigidBody2D.rotation, targetAngle, currentRotationSpeed * Time.fixedDeltaTime);
        rigidBody2D.rotation = smoothedAngle;

        // GetDistanceFromOutsideGravityFieldToPlayer();
    }

    public delegate void GravityAreaChangeHandler(GravityArea2D gravityArea);
    public event GravityAreaChangeHandler OnEnterGravityArea;
    public event GravityAreaChangeHandler OnExitGravityArea;

    public void AddGravityArea(GravityArea2D gravityArea)
    {
        gravityAreas.Add(gravityArea);
        OnEnterGravityArea?.Invoke(gravityArea);
    }

    public void RemoveGravityArea(GravityArea2D gravityArea)
    {
        // gravityAreas.Remove(gravityArea); // disable to prevent player from free-floating in space

        if (gravityAreas.Count == 0)
        {
            OnExitGravityArea?.Invoke(gravityArea);
        }
    }
}
