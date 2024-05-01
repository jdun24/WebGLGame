using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragAndDropSuper : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    protected SpriteRenderer spriteRenderer;
    protected int layerToUse;
    protected Rigidbody2D objectBody2D;
    protected BoxCollider2D boxCollider2D;

    protected bool isPlaced = false;
    public delegate void placementEvent();
    public static event placementEvent OnPlacementEvent;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.enabled = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if(!isPlaced){
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

            Vector2 rayDirection = Quaternion.Euler(0, 0, transform.eulerAngles.z) * Vector2.down;
            //The length needs to be half of the size of the asteroid we are currently on
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.size.y / 2f), rayDirection, 20f, layerToUse);

        }
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if(!isPlaced)
            spriteRenderer.color = Color.red;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if(spriteRenderer.color == Color.green){
            isPlaced = true;
            //Stops all possible movement
            objectBody2D.isKinematic = true;
            objectBody2D.bodyType = RigidbodyType2D.Static;
            objectBody2D.simulated = false;
            boxCollider2D.enabled = false;
        }
    }

}