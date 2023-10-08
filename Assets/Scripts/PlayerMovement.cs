using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{

   // public Vector3 gravity;
    public Vector3 playerVelocity;
    private ControllerColliderHit _contact;

    // Checks if the player is on the ground
    public bool isOnGround = false;


    public float walkSpeed = 5;
    public float runSpeed = 8; 
    
    // Minimum falling speed
    public float minFall = -1.5f;
    private float _vertSpeed;
    
    // Jumping speed (Goes up as its positive)
    public float jumpHeight = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float rotSpeed = 1.0f;
    private bool _isJumping = false;

    private bool _canJump = true; // Whether the character can jump now
    private float _jumpCooldown = 0.2f; // The duration of the jump cooldown in seconds
    private float _jumpCooldownTimer = 0f; // Timer to keep track of the cooldown

    private bool _isDoubleJumping = false;
    public int canDoubleJump = 0;
    
    

    private CharacterController _controller;
    private CameraMovement _cameraMovement;
    private Animator _animator;
    private void Start()
    {

        _controller = GetComponent<CharacterController>();
        _cameraMovement = GetComponent<CameraMovement>();
        _animator = GetComponent<Animator>();
    //    _animator.enabled = false;
    
        // Sets the vertical speed as the minimum falling speed
        _vertSpeed = minFall;
    }
    void Update()
    {
      /*  if (isOnGround)
        {
            canDoubleJump = 1;
        }
        */
       // Debug.Log(canDoubleJump);
        // Update the jump cooldown timer
        if (!_canJump)
        {
            _jumpCooldownTimer += Time.deltaTime;

            // Check if the cooldown period has passed
            if (_jumpCooldownTimer >= _jumpCooldown)
            {
                _canJump = true;
                _jumpCooldownTimer = 0f; // Reset the timer
            }
        }

        // Check for jump input
        if (_canJump && Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
            _canJump = false; // Set to false to enforce cooldown
        }

        if (_isJumping && Input.GetButtonDown("Jump") && canDoubleJump == 1)
        {
            _isDoubleJumping = true;
            _animator.SetBool("IsDoubleJumping", true);

            Debug.Log("Double jump activated");
            canDoubleJump = 0;
        }

        ProcessMovement();
    }

    public void LateUpdate()
    {
       
       UpdateAnimator();
       
    }
    
    void UpdateAnimator()
    {
        isOnGround = _controller.isGrounded; 
        // TODO 
        Vector3 characterXandZMotion = new Vector3(playerVelocity.x,0.0f, playerVelocity.z);
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f){
            if(Input.GetButton("Fire3")){
                _animator.SetFloat("Speed",1.0f);
            }
            else{
                _animator.SetFloat("Speed",0.5f);

            }
        } else {
            _animator.SetFloat("Speed", 0.0f);
        }
        if (Input.GetKeyUp("space") && _isDoubleJumping == false)
        {
            _animator.applyRootMotion = true;
            _animator.SetBool("IsJumping", true);
        }
        
        if (Input.GetKeyUp("space") && _isDoubleJumping == true)
        {
            _animator.applyRootMotion = true;
            _animator.SetBool("IsDoubleJumping", true);
        }

        if (isOnGround && _animator.GetBool("IsJumping") == true)
        {
            _animator.SetBool("IsJumping", false);

        }

        if (_isDoubleJumping)
        {
            _animator.SetBool("IsJumping", true);
        }
        
        if (isOnGround  && _animator.GetBool("IsDoubleJumping") == true)
        {
            _animator.SetBool("IsDoubleJumping", false);
            _animator.SetBool("IsJumping", false);
        }

        if (_animator.GetBool("IsGrounded") && _animator.GetBool("IsJumping"))
        {
            _animator.SetBool("IsDoubleJumping", false);
            _animator.SetBool("IsJumping", false);
        }
        
        
    }

    void ProcessMovement()
    {
        float speed = GetMovementSpeed();
        Vector3 movement = CalculateMovementVector();

        RotateCharacter(movement);
        HandleJump();

        HandleGravity();
        
        ApplyMovementToController(movement);
        _animator.SetBool("IsGrounded", isOnGround);


    }

    Vector3 CalculateMovementVector()
    {
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        Vector3 right = _controller.transform.right;
        Vector3 forward = Vector3.Cross(right, Vector3.up);

        Vector3 movement = (right * horInput) + (forward * vertInput);
        movement *= GetMovementSpeed();
        movement = Vector3.ClampMagnitude(movement, GetMovementSpeed());

        return movement;
    }

    void RotateCharacter(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotSpeed * Time.deltaTime);
        }
    }

    void HandleJump()
    {
        if (_controller.isGrounded && _isJumping)
        {
            _vertSpeed = jumpHeight;
            _isJumping = false; // Reset the jump flag
        }
        else if (_isJumping && _isDoubleJumping)
        {
            _vertSpeed = jumpHeight; 
            gravity = -4.9f; // Reduce gravity during double jump
            _isJumping = false;
            _isDoubleJumping = false;
        }
        else if (!_controller.isGrounded)
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            _vertSpeed = Mathf.Max(_vertSpeed, terminalVelocity);
        }
    }



    void HandleGravity()
    {
        RaycastHit hit;
        float raycastDistance = _controller.height * 0.6f;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            float check = (_controller.height + _controller.radius) / 1.9f;
            isOnGround = hit.distance <= check;
            Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.red);
        }
        else
        {
            isOnGround = false;
        }

    }

    void ApplyMovementToController(Vector3 movement)
    {
        movement.y = _vertSpeed;
        movement *= Time.deltaTime;
        _controller.Move(movement);
    }

    float GetMovementSpeed()
    {
        return Input.GetButton("Fire3") ? runSpeed : walkSpeed;
    }
}
