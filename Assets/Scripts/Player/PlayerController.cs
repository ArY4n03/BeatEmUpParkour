using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    public Vector2 lookValue;
    private Vector2 moveInput;


    private CharacterController characterController;
    private Vector3 moveDirection;
    private Animator anim;
    [SerializeField]private CameraController cam;



    //private Rigidbody rb;

    [Header("--Ground Check---")]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isOnGround = false;
    private float groundCheckRadius = 2f;

    private float yspeed = -2f;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        onGroundCheck();
        handleMovement();
        
    }
    void OnLook(InputValue value)
    {
        lookValue = value.Get<Vector2>();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        Debug.Log("btn");
        if(isOnGround == true)
        {
            Debug.Log("Jump pressed ");
            yspeed = jumpForce * Time.deltaTime;
        }
    }

    private void handleMovement()
    {
        //direction part
        Vector3 moveDirection = cam.planerRotation *
            new Vector3(moveInput.x, 0f, moveInput.y);

        moveDirection.Normalize();
        
        //gravity part
        handleGravity();
        var velocity = moveDirection * moveSpeed;
        
        velocity.y = yspeed;
        Debug.Log(velocity.y);
        characterController.Move(velocity * Time.deltaTime);

        //rotation part
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

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
        if (isOnGround)
        {
            characterController.height = 1f;
        }
        else
        {
            characterController.height = 2.5f;
        }
        if(isOnGround && yspeed < 0)
        {
            
            yspeed = -2f;
            anim.SetBool("onGround", true);
        }
        else
        {
            Debug.Log("Not on ground");
            yspeed += Physics.gravity.y * Time.deltaTime;
            anim.SetBool("onGround", false);
        }
     
    }
}
