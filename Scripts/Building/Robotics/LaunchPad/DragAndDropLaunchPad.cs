
using UnityEngine;
using UnityEngine.EventSystems;
public class DragAndDropLaunchPad : DragAndDropSuper{
    
    private GravityBody2D gravityBody;
    private LaunchPadManager lpmanager;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectBody2D = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        lpmanager = GetComponent<LaunchPadManager>();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(!isPlaced){
            //Move according to mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            Vector2 Origin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.y / 2f);
            Vector2 direction = gravityBody.GravityDirection;
            
            if(IsValidPos(Origin, direction)){
                spriteRenderer.color = Color.green;
            }else{
                spriteRenderer.color = Color.red;
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(!isPlaced)
            spriteRenderer.color = Color.red;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(spriteRenderer.color == Color.green){
            isPlaced = true;
            objectBody2D.isKinematic = true;
            objectBody2D.bodyType = RigidbodyType2D.Static;
            //objectBody2D.simulated = false;
            spriteRenderer.color = Color.white;
            lpmanager.SetPlaced();
            SoundFXManager.Instance.PlaySound(SFX.Player.Work, this.gameObject.transform, 1f);
        }
    }

    protected bool IsValidPos(Vector2 origin, Vector2 dir)
    {
        float distToGround = GetDistanceToGround(origin, dir);
        if (distToGround < .15f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float GetDistanceToGround(Vector2 origin, Vector2 dir){
        float maxGravityDistance = 10f;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, maxGravityDistance, 1 << 7);

        Debug.DrawRay(transform.position, dir * maxGravityDistance, Color.blue);
        if (hit.collider != null){
            return hit.distance;
        }else{
            return maxGravityDistance;
        }
    }

}
