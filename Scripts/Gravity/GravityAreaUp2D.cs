using UnityEngine;

public class GravityAreaUp2D : GravityArea2D
{
    public override Vector2 GetGravityDirection(GravityBody2D gravityBody)
    {
        return -transform.up;
    }
}
