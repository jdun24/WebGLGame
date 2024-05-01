
using UnityEngine;
using UnityEngine.EventSystems;
public class DragAndDropExtractor : DragAndDropSuper{
    
    private GravityBody2D gravityBody;
    public new delegate void placementEvent(GameObject resourceHit);
    public new static event placementEvent OnPlacementEvent;
    private GameObject resource;
    private string discoveredResourceName = "DiscoveredResource";
    private float rayCastReach;
    void Awake(){
        if(gameObject.name == "Extractor(Clone)"){
            rayCastReach = Vector2.Distance(transform.position, AsteroidManager.Instance.GetCurrentAsteroid().transform.position) * .33f;
        }else if(gameObject.name == "CommercialExtractor(Clone)"){
            rayCastReach = Vector2.Distance(transform.position, AsteroidManager.Instance.GetCurrentAsteroid().transform.position) * .66f;
        }else{
            rayCastReach = Vector2.Distance(transform.position, AsteroidManager.Instance.GetCurrentAsteroid().transform.position);
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectBody2D = GetComponent<Rigidbody2D>();
        gravityBody = GetComponent<GravityBody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        layerToUse = SetLayerToUse();
    }
    public void Update(){
        Vector2 direction = gravityBody.GravityDirection;
        Debug.DrawRay(transform.position, direction * rayCastReach, Color.green);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(!isPlaced){
            //Move according to mouse
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = gravityBody.GravityDirection;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayCastReach, layerToUse);
            Debug.DrawRay(transform.position, direction * rayCastReach, Color.green);

            if(IsValidPos(hit, rayOrigin, direction)){
                resource = hit.collider.gameObject;
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
            spriteRenderer.color = Color.white;
            isPlaced = true;
            OnPlacementEvent?.Invoke(resource);
            //Stops all possible movement
            objectBody2D.isKinematic = true;
            objectBody2D.bodyType = RigidbodyType2D.Static;
            SoundFXManager.Instance.PlaySound(SFX.Extractor.Status.Humming, this.gameObject.transform, 1f);
        }
    }

    protected bool IsValidPos(RaycastHit2D hit, Vector2 origin, Vector2 dir)
    {
        float distToGround = GetDistanceToGround(origin, dir);
        if(hit.collider != null){
            //Debug.Log($"collider: {hit.collider.name}\tdistToGround: {distToGround}");
        }
        if (hit.collider != null && distToGround < 1f && distToGround > 0f)
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
        RaycastHit2D hit = Physics2D.Raycast(origin, dir.normalized, maxGravityDistance, 1 << LayerMask.NameToLayer("Asteroid"));

        //Debug.DrawRay(transform.position, dir * maxGravityDistance, Color.blue);
        if (hit.collider != null){
            //Debug.Log($"point: {hit.point}\tdistToGround: {hit.distance}\torigin{origin}\tdist: {Vector2.Distance(hit.point, origin)}");
            return hit.distance;
        }else{
            return maxGravityDistance;
        }
    }
    protected int SetLayerToUse()
    {
        //Resource Layer
        return 1 << LayerMask.NameToLayer(discoveredResourceName);
    }


}
