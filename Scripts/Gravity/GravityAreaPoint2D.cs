using UnityEngine;

public class GravityAreaPoint2D : GravityArea2D
{
    [SerializeField] private Vector2 center;

    public override Vector2 GetGravityDirection(GravityBody2D gravityBody)
    {
        return (center - (Vector2)gravityBody.transform.position).normalized;
    }
}
