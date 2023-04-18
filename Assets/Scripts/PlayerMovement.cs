using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    [Header("Ground Check")]
    public float groundDrag;
    public float playerHeight;
    public LayerMask isGround;
    bool grounded;
    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    [Header("Crouching")]
    public float crouchSpeed;
    private bool isCrouching;
    [Header("Animations")]
    public Animator animator;
    bool isWalking;
    [Header("State Handler")]
    public Vector3 playerCurrentSpeed;
    public MovementState currentState;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    [Header("Other")]
    Rigidbody rb;
    

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air,
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.5f, isGround);
        //handle drag
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        rb.drag = 0;

        MyInput();
        SpeedControl();
        StateHandler();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if(Input.GetKey(crouchKey))
        {
            animator.SetBool("isCrouching", true);
            readyToJump = false;
        }
        if(Input.GetKeyUp(crouchKey))
        {
            animator.SetBool("isCrouching", false);
            readyToJump = true;
        }
    }

    private void StateHandler()
    {
        //mode - crouch
        if(Input.GetKeyDown(crouchKey))
        {
            currentState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        //mode - sprint
        if(grounded && Input.GetKey(sprintKey) && currentState != MovementState.crouching && Input.GetKey(KeyCode.W))
        {
            currentState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        //mode - walk
        if(grounded && !Input.GetKey(sprintKey) && !Input.GetKey(crouchKey))
        {
            currentState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        //mode - air
        if(Input.GetKey(jumpKey))
        {
            currentState = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        isWalking = rb.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
        
        //on ground
        if(grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        //in air
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        //jump force upwards
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("isJumping");
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
