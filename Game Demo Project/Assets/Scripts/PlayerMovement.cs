using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //For walking and moving left and right
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private Vector3 moveDirection;

    //For gravity and jumping
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;

    //Caharacter controller reference
    private CharacterController controller;
    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    //FixedUpdate used instead of Update for rendering process to eliminate screen refresh rate
    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        //Return true when we are standing on the ground, 
        //Return false when we are not on the ground but on air or on top of an obstacle
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        //Cheks if we are grounded and if we are grounded stops applying gravity
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }


        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(horizontal, 0, vertical);
        //Rotating player move direction to player's forward not the world axis anymore
        moveDirection = transform.TransformDirection(moveDirection);

        //We can run only when we are on the ground
        if (isGrounded)
        {
            //Walking
            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            //Running
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            //Idle
            else if (moveDirection == Vector3.zero)
            {
                Idle();
            }
            //Update the moveSpeed based on user input
            moveDirection *= moveSpeed;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }

        
        //Actually move the character at last
        controller.Move(moveDirection * Time.deltaTime);
        //Calculate gravity
        velocity.y += gravity * Time.deltaTime;
        //Apply gravity to our character
        controller.Move(velocity * Time.deltaTime);
    }

    private void Idle()
    {
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime); 
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.50f, 0.1f, Time.deltaTime);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
    }

    private void Jump()
    {
        //Jumping equation
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}
