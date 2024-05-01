using UnityEngine;
using System.Collections;
public class RobotBuddyController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private bool FocusedOn;
    [SerializeField, ReadOnly] private float groundCheckRadius = 0.7f;
    [Header("Objects")]
    [SerializeField] private Rigidbody2D robotBody;
    [SerializeField] private GravityBody2D gravityBody;

    private float walkingSpeed = 5.5f;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    private bool isIdle = false;
    [SerializeField] private float jumpForce = 1f;

    [Header("Movement")]
    [SerializeField, ReadOnly] private bool isGrounded = false;

    private enum RobotState { Idle, Walking, Interacting, Jumping }
    [SerializeField, ReadOnly] private RobotState currentState = RobotState.Idle;
    [SerializeField] public BoolEvent robotBuddyInteract;
    [SerializeField] private RobotUIEvent adjustRobotUI;
    // Animation
    private Vector3 playerCoordinates;
    private bool isInPit;
    private bool playerCanInteract = false;
    [SerializeField, ReadOnly] private bool isActive;
    [SerializeField, ReadOnly] private int TechTier = 0;
    [SerializeField, ReadOnly] private bool controllingRobotAlpha = false;
    [SerializeField, ReadOnly] private bool controllingRobotBeta = false;
    [SerializeField, ReadOnly] private bool beingCarried = false;
    RobotBuddy robotBuddy;
    CapsuleCollider2D chargeCollider;
    CapsuleCollider2D normalCollider;

    private float idleReduceCharge = 4.0f;
    private float jumpReduceCharge = 5.0f;
    private float interactReduceCharge = 5.0f;
    private Vector3 hiddenPosition = new Vector3(999f, 999f);
    private bool deathSoundPlayed = false;

    void Awake()
    {
        transform.position = hiddenPosition;
        robotBuddy = new();
        normalCollider = GetComponent<CapsuleCollider2D>();
        chargeCollider = gameObject.AddComponent<CapsuleCollider2D>();
        chargeCollider.isTrigger = true;
    }

    IEnumerator DecreaseChargeAlpha()
    {
        while (isControllingAlpha())
        {
            //reduces 1 tick per 1 second
            SetCharge(robotBuddy.ReduceCharge(idleReduceCharge));

            //  I think we need to either get rid of 3rd TechLevel or think of something else it can do, maybe increase jump height
            // if(robotBuddy.UpdateTechTier() == 3){
            //     adjustRobotUI.Raise(robotBuddy.GainCharge(0.5f));
            // }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator DecreaseChargeBeta()
    {
        while (isControllingBeta())
        {
            //reduces 1 tick per 1 second
            SetCharge(robotBuddy.ReduceCharge(idleReduceCharge));

            //  I think we need to either get rid of 3rd TechLevel or think of something else it can do, maybe increase jump height
            // if(robotBuddy.UpdateTechTier() == 3){
            //     adjustRobotUI.Raise(robotBuddy.GainCharge(0.5f));
            // }
            yield return new WaitForSeconds(1.0f);
        }
    }

    // -------------------------------------------------------------------
    // Handle events

    // this method will set the player's last coordinates on the main asteroid scene
    public void SetPlayerCoordinates()
    {
        this.playerCoordinates = this.transform.position;
    }

    // this method will get the player's last coordinates on the main asteroid scene
    public Vector3 GetPlayerCoordinates()
    {
        return this.playerCoordinates;
    }

    public void OnRobotMove(Vector2 direction)
    {
        if (!isActive)
            return;
        horizontalInput = direction.x;
    }

    public void OnControlStateUpdated(Control.State currentControlState)
    {
        if (currentControlState == Control.State.RobotBuddyAlpha)
        {
            isActive = this.gameObject.name == "RobotBuddyAlpha";
            controllingRobotAlpha = true;
            controllingRobotBeta = false;
            if (robotBuddy.UpdateTechTier() != 4)
                StartCoroutine(DecreaseChargeAlpha());
        }
        else if (currentControlState == Control.State.RobotBuddyBeta)
        {
            isActive = this.gameObject.name == "RobotBuddyBeta";
            controllingRobotAlpha = false;
            controllingRobotBeta = true;
            if (robotBuddy.UpdateTechTier() != 4)
                StartCoroutine(DecreaseChargeBeta());
        }
        else
        {
            isActive = false;
            controllingRobotAlpha = false;
            controllingRobotBeta = false;
        }
    }

    public void OnRobotBuddyInteract(bool interacting)
    {
        if (!isActive)
            return;
        //Fix Module, look how to call repairModule
    }

    public void OnRobotJump(bool jumping)
    {
        if (isControllingAlpha() || isControllingBeta())
        {
            //Debug.Log($"Robot controller - jumping: {jumping}\t\tisGrounded: {isGrounded}");
            SetCharge(robotBuddy.ReduceCharge(jumpReduceCharge));
            if (jumping)
            {
                if (isGrounded)
                {
                    UpdateRobotState(RobotState.Jumping);
                }
            }
            else
            {
                UpdateRobotState(isIdle ? RobotState.Idle : RobotState.Walking);
            }
        }
        //Debug.Log($"Robot controller - OnRobotJump: {robotBuddy.GetCurrentCharge()}");

    }

    private void RobotMove()
    {
        currentSpeed = walkingSpeed;

        // Calculate the movement direction based on the player's current orientation and input
        UnityEngine.Vector2 direction = transform.right * horizontalInput;
        // Calculate the actual movement amount
        UnityEngine.Vector2 movement = direction * (currentSpeed * Time.fixedDeltaTime);
        // Move the player's rigidbody
        robotBody.position += movement;
    }

    private void Interact()
    {
        if (isGrounded && currentState == RobotState.Interacting)
        {
            SetCharge(robotBuddy.ReduceCharge(interactReduceCharge));
            robotBuddyInteract.Raise(true);
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;
        if (currentState == RobotState.Jumping)
        {
            robotBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
        }
    }
    // User input, animations, moving non-physics objects, game logic


    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        if (!isActive || beingCarried || robotBuddy.IsChargeEmpty())
            return;
        // First check to make sure the Robot is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Handle possible inputs
        RobotMove();
        Jump();
        Interact();
        // Handle falling in the pit scenario
        if (isInPit)
        {
        }
    }

    // when in contact with objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"Collided with: {collision.gameObject.tag}");
        switch (collision.gameObject.tag)
        {
            case "BlackPit":
                isInPit = true;
                break;
            case "Player":
                playerCanInteract = true;
                break;
            default:
                break;
        }
    }

    // when not in contact with objects
    private void OnTriggerExit2D(Collider2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "BlackPit":
                isInPit = false;
                break;
            case "Player":
                playerCanInteract = false;
                break;
            default:
                break;
        }
    }

    public void OnPlayerInteract()
    {
        if (playerCanInteract && CyberneticsManager.Instance.HasCyberneticCharge())
        {
            CyberneticsManager.Instance.UseCharge();
            SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Robot.Pickup), this.gameObject.transform, 1f);
            deathSoundPlayed = false;
            if (this.gameObject.name == "RobotBuddyAlpha")
            {
                adjustRobotUI.Raise(new packet.RobotUIPacket(Control.State.RobotBuddyAlpha, robotBuddy.GiveFullCharge()));
            }
            else if (this.gameObject.name == "RobotBuddyBeta")
            {
                adjustRobotUI.Raise(new packet.RobotUIPacket(Control.State.RobotBuddyBeta, robotBuddy.GiveFullCharge()));
            }
        }
    }

    public void OnPlayerPickup()
    {
        
        if (playerCanInteract && beingCarried == false)
        {
            if (this.gameObject.name == "RobotBuddyAlpha" && !PlayerManager.Instance.GetIsCarryingRobotBeta())
            {
                beingCarried = true;
                DisablePhysics();
            }else if(this.gameObject.name == "RobotBuddyBeta" && !PlayerManager.Instance.GetIsCarryingRobotAlpha()){
                beingCarried = true;
                DisablePhysics();
            }
        }
        else if (beingCarried == true)
        {
            beingCarried = false;
            EnablePhysics();
        }
    }

    private void DisablePhysics()
    {
        transform.position = hiddenPosition;
        robotBody.simulated = false;
        normalCollider.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        isActive = false;
    }

    private void EnablePhysics()
    {
        TeleportRobotBuddy();
        robotBody.simulated = true;
        normalCollider.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        TeleportRobotBuddy();
    }

    private void UpdateRobotState(RobotState newState)
    {
        currentState = newState;
    }

    private bool isControllingAlpha()
    {
        if (controllingRobotAlpha && this.gameObject.name == "RobotBuddyAlpha")
        {
            return true;
        }
        return false;
    }

    private bool isControllingBeta()
    {
        if (controllingRobotBeta && this.gameObject.name == "RobotBuddyBeta")
        {
            return true;
        }
        return false;
    }

    private void SetCharge(float num)
    {
        if (isControllingAlpha())
        {
            adjustRobotUI.Raise(new packet.RobotUIPacket(Control.State.RobotBuddyAlpha, num));
        }
        else if (isControllingBeta())
        {
            adjustRobotUI.Raise(new packet.RobotUIPacket(Control.State.RobotBuddyBeta, num));
        }
        if(num <= 0 && deathSoundPlayed == false){
            SoundFXManager.Instance.PlaySound(SFX.Robot.Status.Dead, this.gameObject.transform, 1f);
            deathSoundPlayed = true;
        }
    }

    public void TeleportRobotBuddy()
    {
        Vector3 screenPos = new Vector3(475, 285, 0f);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        transform.position = PlayerManager.Instance.GetPlayerPosition();
    }
}
