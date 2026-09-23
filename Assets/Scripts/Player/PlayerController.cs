using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    public Vector2 lookValue;
    private Vector2 moveInput;

    private PlayerInput playerInput;
    private InputAction sprintAction;
    public InputAction parkourAction;

    private bool isSprinting=false;
    private CharacterController characterController;
    private Vector3 moveDirection;
    private Animator anim;
    [SerializeField]private CameraController cam;



    //private Rigidbody rb;

    [Header("------Ground Check------")]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround = false;
    private float groundCheckRadius = 2f;

    private float yspeed = -2f;
    public bool hasControl = true;
    Quaternion targetRotation;
    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        sprintAction = playerInput.actions["Sprint"];
        parkourAction = playerInput.actions["Parkour"];
    }

    private void Update()
    {
        onGroundCheck();
        handleMovement();
        isSprinting = false;

    }
    void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    void OnMove(InputValue value)
    {
        
        moveInput = value.Get<Vector2>();
    }

    public void SetControl(bool value)
    {
        hasControl = value;
        characterController.enabled = hasControl;
        if (!hasControl)
        {
            //moveInput = Vector2.zero;
            anim.SetFloat("MoveAmount", 0f);
            targetRotation = transform.rotation;
            
        }
    }
    private void handleMovement()
    {
        //direction part
        Vector3 moveDirection = cam.planerRotation *
            new Vector3(moveInput.x, 0f, moveInput.y);

        moveDirection.Normalize();
        
        if (!hasControl)
            return; 

        //gravity part
        handleGravity();
        if (sprintAction.IsPressed())
        {
            moveSpeed = 10f;
        }
        else
        {
            moveSpeed = 5f;
        }

        Debug.Log("Move Speed: " + moveSpeed);
        var velocity = moveDirection * moveSpeed;
        
        velocity.y = yspeed;
        
        characterController.Move(velocity * Time.deltaTime);

        //rotation part
        if (moveDirection.magnitude > 0.1f)
        {
            targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,Time.deltaTime * 10f);
        }

        float moveAmount = Mathf.Clamp01(moveInput.magnitude);

        anim.SetFloat("MoveAmount",moveAmount,0.2f,Time.deltaTime);
    }

    public void onGroundCheck()
    {
        isOnGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius,groundLayer);

    }

    public void handleGravity()
    {
    
        if(isOnGround && yspeed < 0)
        {
            
            yspeed = -2f;
            anim.SetBool("onGround", true);
        }
        else
        {
            
            yspeed += Physics.gravity.y * Time.deltaTime;
            anim.SetBool("onGround", false);
        }
     
    }

    private void OnDrawGizmos() //drawing gizmos
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

    }
}
