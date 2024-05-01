using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class GravityArea2D : MonoBehaviour
{
    [SerializeField] private int priority;
    public int Priority => priority;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public abstract Vector2 GetGravityDirection(GravityBody2D gravityBody);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out GravityBody2D gravityBody))
        {
            gravityBody.AddGravityArea(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out GravityBody2D gravityBody))
        {
            gravityBody.RemoveGravityArea(this);
        }
    }
}