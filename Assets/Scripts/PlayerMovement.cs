using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    public Vector3 gravity;
    public Vector3 playerVelocity;

    public bool isOnGround = false;
    public float gravityValue = -9.81f;

    public float walkSpeed = 5;
    public float runSpeed = 8; 
    

    private CharacterController _controller;
    private CameraMovement _cameraMovement;
    private Animator _animator;
    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _cameraMovement = GetComponent<CameraMovement>();
        _animator = GetComponent<Animator>();
    }
    public void Update()
    {
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
        if(Input.GetButton("Fire1")){
            _animator.applyRootMotion = true;
            _animator.SetTrigger("DoRoll");
        }
        
    
        _animator.SetBool("IsGrounded", isOnGround);
        
    }

    void ProcessMovement()
    { 
        // Moving the character foward according to the speed
        float speed = GetMovementSpeed();

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Turning the character
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity =  move * (Time.deltaTime * speed);
        
        playerVelocity.y = gravityValue * Time.deltaTime;
        
        _controller.Move(playerVelocity);
        isOnGround = _controller.isGrounded; 

    }

    float GetMovementSpeed()
    {
        return Input.GetButton("Fire3") ? // Left shift
            runSpeed : walkSpeed;
    }
}