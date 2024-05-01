using UnityEngine;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Events")]
    [SerializeField] public StringEvent currentAsteroidChanged;
    [SerializeField] public BoolEvent playerGroundedUpdated;
    [SerializeField] public Vector2Event playerPositionUpdated;
    [SerializeField] public BoolEvent playerInteract;
    [SerializeField] public BoolEvent playerLaunchPadInteract;
    [SerializeField] public VoidEvent playerInPit;

    [Header("Mutable")]
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private GravityBody2D gravityBody;
    [SerializeField, ReadOnly] private PlayerState currentState = PlayerState.Idle;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private GameObject carryAlphaObject;
    [SerializeField] private GameObject carryBetaObject;
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private CanvasGroup ControlGroup;

    [Header("ReadOnly")]
    [SerializeField, ReadOnly] private bool isCarryingRobotAlpha = false;
    [SerializeField, ReadOnly] private bool isCarryingRobotBeta = false;
    [SerializeField, ReadOnly] private bool canPickupAlpha = false;
    [SerializeField, ReadOnly] private bool canPickupBeta = false;
    [SerializeField, ReadOnly] private int selection = 0;
    [SerializeField, ReadOnly] private float horizontalInput = 0f;
    [SerializeField, ReadOnly] private float currentSpeed;
    [SerializeField, ReadOnly] private bool wasGrounded = false;
    [SerializeField, ReadOnly] private bool isGrounded = false;
    [SerializeField, ReadOnly] private bool canSprint = false;
    [SerializeField, ReadOnly] private bool canDoubleJump = false;
    [SerializeField, ReadOnly] private bool isFacingRight = false;

    // Not for display
    private enum PlayerState { Idle, Walking, Sprinting, Strafing, Jumping, Crouching, Crawling, Interacting }
    private float groundCheckRadius = 0.3f;
    private float sprintingSpeed = 10f;
    private float walkingSpeed = 7.5f;
    private float strafingSpeed = 5f;
    private float crawlingSpeed = 2.5f;
    private float idleSpeed = 0f;
    private float vertical;
    private bool isLadder;
    private bool isClimibing;
    private bool isInPit;
    private bool isIdle = false;
    private bool isSprinting = false;
    private bool isWalking = false;
    private bool isCrawling = false;
    private bool isStrafing = false;
    private bool isJumping = false;
    private bool isCrouching = false;
    private TextMeshProUGUI reminderText;
    private UnityEngine.Vector3 playerCoordinates;


    // -------------------------------------------------------------------
    // Handle events

    // this method will set the player's last coordinates on the main asteroid scene
    public void SetPlayerCoordinates()
    {
        this.playerCoordinates = this.transform.position;
    }

    // this method will get the player's last coordinates on the main asteroid scene
    public UnityEngine.Vector3 GetPlayerCoordinates()
    {
        return this.playerCoordinates;
    }

    public void OnPlayerMove(UnityEngine.Vector2 direction)
    {
        horizontalInput = direction.x;
        isIdle = horizontalInput == 0;
        if (isIdle)
        {
            UpdatePlayerState(isCrouching ? PlayerState.Crouching : PlayerState.Idle);
        }
        else if (isSprinting)
        {
            UpdatePlayerState(PlayerState.Sprinting);
        }
        else if (isCrouching)
        {
            UpdatePlayerState(PlayerState.Crawling);
        }
        else if (!isGrounded)
        {
            UpdatePlayerState(PlayerState.Strafing);
        }
        else
        {
            // SoundFXManager.Instance.PlaySound()
            UpdatePlayerState(PlayerState.Walking);
        }
    }

    public void OnPlayerSprint(bool sprinting)
    {
        // Debug.Log($"Player controller - OnPlayerSprint: {sprinting}");
        if (sprinting)
        {
            canSprint = true;
            if (isGrounded && !isCrouching && !isIdle)
            {
                isSprinting = true;
                UpdatePlayerState(PlayerState.Sprinting);
                // Additional logic for starting sprint
            }
        }
        else
        {
            // Handle sprint cancellation
            canSprint = false;
            isSprinting = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
            // Additional logic for cancelling sprint
        }
    }

    public void OnPlayerJump(bool jumping)
    {
        //Debug.Log($"Player controller - OnPlayerJump: {jumping}");
        if (jumping)
        {
            if (isGrounded)
            {
                isJumping = true;
                UpdatePlayerState(PlayerState.Jumping);
                SoundFXManager.Instance.PlaySound(SFX.Player.Jump, this.transform, 1f);
            }
        }
        else
        {
            isJumping = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
        }
    }

    public void OnPlayerCrouch(bool crouching)
    {
        if (crouching)
        {
            if (isGrounded)
            {
                isCrouching = true;
                UpdatePlayerState(isIdle ? PlayerState.Crouching : PlayerState.Crawling);
            }
        }
        else
        {
            isCrouching = false;
            UpdatePlayerState(isIdle ? PlayerState.Idle : PlayerState.Walking);
        }
    }

    public void OnPlayerInteract(bool interacting)
    {
        if (interacting)
        {
            if (currentState != PlayerState.Idle) return;
            UpdatePlayerState(PlayerState.Interacting);
        }
        else
        {
            if (currentState == PlayerState.Interacting)
            {
                UpdatePlayerState(PlayerState.Idle);
            }
        }
    }

    public void OnPlayerPickupRobot()
    {
        if (isCarryingARobot() == false)
        {
            if(TryPickUpAlpha() == false){
                TryPickUpBeta();
            }
        }
        else
        {
            PlayerManager.Instance.SetCarryingAlpha(false);
            PlayerManager.Instance.SetCarryingBeta(false);
            if (isCarryingRobotAlpha)
            {
                carryAlphaObject.SetActive(false);
                isCarryingRobotAlpha = false;
                TryPickUpBeta();
            }else if (isCarryingRobotBeta)
            {
                carryBetaObject.SetActive(false);
                isCarryingRobotBeta = false;
                TryPickUpAlpha();
            }

            SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Robot.Dropped), this.gameObject.transform, 1f);
        }
    }

    private bool TryPickUpAlpha(){
        if (canPickupAlpha == true)
        {
            isCarryingRobotAlpha = true;
            PlayerManager.Instance.SetCarryingAlpha(true);
            carryAlphaObject.SetActive(true);
            SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Robot.Pickup), this.gameObject.transform, 1f);
            return true;
        }
        return false;
    }
    private bool TryPickUpBeta(){
        if (canPickupBeta == true)
        {
            isCarryingRobotBeta = true;
            PlayerManager.Instance.SetCarryingBeta(true);
            carryBetaObject.SetActive(true);
            SoundFXManager.Instance.PlayRandomSoundOfType(typeof(SFX.Robot.Pickup), this.gameObject.transform, 1f);
            return true;
        }
        return false;
    }
    private bool isCarryingARobot(){
        if(isCarryingRobotAlpha || isCarryingRobotBeta){
            return true;
        }else return false;
    }
    // -------------------------------------------------------------------
    // Class

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (currentAsteroidChanged == null)
        {
            currentAsteroidChanged = ScriptableObject.CreateInstance<StringEvent>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found on the object!");
        }
    }

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selection = 0;
            animator.SetBool("John", true);
        }
        else
        {
            selection = PlayerPrefs.GetInt("selectedOption");
            switch (selection)
            {
                case 0:
                    animator.SetBool("John", true);
                    break;

                case 1:
                    animator.SetBool("Joy", true);
                    break;
            }
        }
        //LoadSelectedCharacter(selection);
        carryAlphaObject.SetActive(false);
        carryBetaObject.SetActive(false);
    }

    private void UpdatePlayerState(PlayerState newState)
    {
        currentState = newState;
        UpdateAnimatorParameters();
    }

    private void UpdateAnimatorParameters()
    {
        // Reset all animator parameters
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("canSprint", false);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isCrouching", false);
        animator.SetBool("isCrawling", false);
        animator.SetBool("isStrafing", false);
        animator.SetBool("isJumping", false);
        animator.SetBool("isInteracting", false);

        animator.SetBool("canSprint", canSprint);

        // Update based on current state
        switch (currentState)
        {
            case PlayerState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case PlayerState.Walking:
                animator.SetBool("isWalking", true);
                break;
            case PlayerState.Sprinting:
                animator.SetBool("isSprinting", true);
                break;
            case PlayerState.Crouching:
                animator.SetBool("isIdle", true);
                animator.SetBool("isCrouching", true);
                break;
            case PlayerState.Crawling:
                animator.SetBool("isCrawling", true);
                break;
            case PlayerState.Strafing:
                animator.SetBool("isStrafing", true);
                break;
            case PlayerState.Jumping:
                animator.SetBool("isJumping", true);
                break;
            case PlayerState.Interacting:
                animator.SetBool("isIdle", true);
                animator.SetBool("isInteracting", true);
                break;
        }
    }

    private void Move()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                currentSpeed = idleSpeed;
                break;
            case PlayerState.Crawling:
                currentSpeed = crawlingSpeed;
                break;
            case PlayerState.Strafing:
                currentSpeed = strafingSpeed;
                break;
            case PlayerState.Walking:
                currentSpeed = walkingSpeed;
                break;
            case PlayerState.Sprinting:
                currentSpeed = sprintingSpeed;
                break;
        }

        // Calculate the movement direction based on the player's current orientation and input
        UnityEngine.Vector2 direction = transform.right * horizontalInput;
        UnityEngine.Vector2 movement;
        // Calculate the actual movement amount
        movement = DetermineMovementSpeed(direction);

        // Move the player's rigidbody
        playerBody.position += movement;
        playerPositionUpdated.Raise(playerBody.position);
    }

    private UnityEngine.Vector2 DetermineMovementSpeed(UnityEngine.Vector2 direction)
    {
        if (BuildManager.Instance.HasBuiltExosuit() == false)
        {
            return direction * (currentSpeed * Time.fixedDeltaTime);
        }
        else if (BuildingTierManager.Instance.GetTierOf(BuildingComponents.BuildingType.Exosuit) < 2)
        {
            return direction * (currentSpeed * 1.10f * Time.fixedDeltaTime);
        }
        else
        {
            return direction * (currentSpeed * 1.20f * Time.fixedDeltaTime);
        }
    }

    private void Jump()
    {
        if (!isGrounded) return;

        if (currentState == PlayerState.Jumping)
        {
            // Debug.Log("JumpForce of: " + jumpForce);
            playerBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
        }
        else if (canDoubleJump)
        {
            // // Double jump
            // playerBody.AddForce(-gravityBody.GravityDirection * jumpForce, ForceMode2D.Impulse);
            // animator.SetTrigger("Jump-Press");
            // canDoubleJump = false;
        }
    }

    private void Crouch()
    {
        if (isGrounded)
        {

            // do something
        }
    }

    private void Interact()
    {
        if (isGrounded && currentState == PlayerState.Interacting)
        {
            playerInteract.Raise(true);
            //playerLaunchPadInteract.Raise(true);
        }
    }

    // User input, animations, moving non-physics objects, game logic
    private void Update()
    {
        UpdateAnimations();
        //cybernetics.StartCorRoutine();
        // below is the code for climbing ladders
        vertical = Input.GetAxis("Vertical");

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimibing = true;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            switch (ControlGroup.alpha)
            {
                case 0:
                    ControlGroup.alpha = 1;
                    ControlGroup.blocksRaycasts = true;
                    break;
                case 1:
                    ControlGroup.alpha = 0;
                    ControlGroup.blocksRaycasts = false;
                    break;
                default:
                    break;
            }
        }
    }

    // Physics calculations, ridigbody movement, collision detection
    private void FixedUpdate()
    {
        // First check to make sure the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Check if the grounded state has changed
        if (isGrounded != wasGrounded)
        {
            playerGroundedUpdated.Raise(isGrounded);  // Raise event with the new state
            wasGrounded = isGrounded;  // Update the wasGrounded to the current state
        }

        // Handle possible inputs
        Move();
        Jump();
        Crouch();
        Interact();

        // Handle falling in the pit scenario
        if (isInPit)
        {
            playerInPit.Raise();
        }

        // Handle ladder movement
        if (isClimibing && Input.GetKey(KeyCode.W))
        {
            this.playerBody.velocity = new UnityEngine.Vector2(playerBody.velocity.x, 4f);
        }
        else if (isClimibing && Input.GetKey(KeyCode.S))
        {
            this.playerBody.velocity = new UnityEngine.Vector2(playerBody.velocity.x, -4f);
        }
    }

    public void UpdateJumpForce(float newJumpForce)
    {
        this.jumpForce = newJumpForce;
    }

    public float GetJumpForce()
    {
        return this.jumpForce;
    }

    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.layer == LayerMask.NameToLayer("GravityField"))
        {
            string asteroidName = collison.transform.parent.name;  // Assuming the parent of the gravity field is the asteroid
            currentAsteroidChanged.Raise(asteroidName);
            playerPositionUpdated.Raise(transform.position);
            //Debug.Log("Entered gravity field of: " + asteroidName);
        }

        switch (collison.gameObject.tag)
        {
            case "Ladder":
                isLadder = true;
                break;

            case "BlackPit":
                isInPit = true;
                break;
            case "RobotBuddy":
                if (collison.gameObject.name == "RobotBuddyAlpha")
                {
                    canPickupAlpha = true;
                }
                else if (collison.gameObject.name == "RobotBuddyBeta")
                {
                    canPickupBeta = true;
                }
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
            case "Ladder":
                isLadder = false;
                isClimibing = false;
                break;

            case "BlackPit":
                isInPit = false;
                break;
            case "RobotBuddy":
                if (Collision.gameObject.name == "RobotBuddyAlpha")
                {
                    canPickupAlpha = false;
                }
                else if (Collision.gameObject.name == "RobotBuddyBeta")
                {
                    canPickupBeta = false;
                }
                break;
            default:
                break;
        }

        // if (Collision.gameObject.tag == "Ladder")
        // {
        //     isLadder = false;
        //     isClimibing = false;
        // }
    }

    private void UpdateAnimations()
    {
        if (!isFacingRight && horizontalInput > 0f)
        {
            Flip();
        }
        else if (isFacingRight && horizontalInput < 0f)
        {
            Flip();
        }

        animator.SetBool("isFacingRight", isFacingRight);
        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing
        isFacingRight = !isFacingRight;

        UnityEngine.Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void LoadSelectedCharacter(int selection)
    {
        if (characterDatabase == null)
        {
            Debug.LogError("Character Database is not assigned!");
            return;
        }

        if (selection < 0 || selection >= characterDatabase.CharacterCount)
        {
            Debug.LogError("Selection index out of range!");
            return;
        }

        Character character = characterDatabase.GetSelectedCharacter(selection);
        if (character == null)
        {
            Debug.LogError("No character found at the selected index!");
            return;
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer not found!");
            return;
        }

        spriteRenderer.sprite = character.characterSprite;
    }
}
