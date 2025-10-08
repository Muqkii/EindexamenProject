using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    private float speed;
    public float walkSpeed = 12f;
    public float sprintSpeed = 18f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public float stamina = 100;
    private float maxStamina;
    private bool sprinting;

    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;
    private bool crouching;

    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        maxStamina = stamina;
        speed = walkSpeed;
        startYScale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z= Input.GetAxis("Vertical");
        
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0&& isGrounded)
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

        if (sprinting)
        {
            speed = sprintSpeed;
            stamina = stamina - 1;
            
            if (stamina <= 0)
            {
                sprinting = false;
                speed = walkSpeed;
            }
        }
        else if (!sprinting && stamina < maxStamina)
        {
            speed = walkSpeed;
            stamina = stamina + 1;
        }
        //Debug.Log(stamina);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouching = true;
        }
        else
        {
            crouching = false;
        }

        if (crouching)
        {
            speed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
        else if (!crouching && speed == crouchSpeed)
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            speed = walkSpeed;
        }

            Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime); 

        if(Input.GetButtonDown("Jump")&& isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
