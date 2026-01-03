using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 4f;
    private float dashSpeed = 12f;
    public Vector2 moveInput;//stores ongoing/current input of the gamer
    public Vector2 LastmoveInput;//stores last input of the gamer
    public Transform playerFacingTowards;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
    private Animator anim;
    [SerializeField]
    private ParticleSystem dashParticles;
    private PlayerStates currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        LastmoveInput = new Vector2(0, -1);
        anim.SetFloat("LastInputX", LastmoveInput.x);
        anim.SetFloat("LastInputY", LastmoveInput.y);
        currentState = PlayerStates.Idle;
    }
    void FixedUpdate()
    {
        if(moveInput != Vector2.zero)
        {
            LastmoveInput = moveInput;
            anim.SetFloat("LastInputX", LastmoveInput.x);
            anim.SetFloat("LastInputY", LastmoveInput.y);
        }

        switch (currentState)
        {
            case PlayerStates.Run:
                rb.linearVelocity = moveInput.normalized * moveSpeed;
                break;
            case PlayerStates.Idle:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerStates.Attacking:
                rb.linearVelocity = Vector2.zero;
                break;
            case PlayerStates.Dashing:
                break;
            
        }
        AimRotation();
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            anim.SetTrigger("triggerAttack");
            ChangeState(PlayerStates.Attacking);
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled && currentState==PlayerStates.Run)
        {
            ChangeState(PlayerStates.Idle);
        }
        else if(context.performed)
        {
            ChangeState(PlayerStates.Run);
        }
            moveInput = context.ReadValue<Vector2>();
        anim.SetFloat("InputX", moveInput.x);
        anim.SetFloat("InputY", moveInput.y);
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.performed)
        {
            ChangeState(PlayerStates.Dashing);
            dashParticles.Play();
            StartCoroutine(Dash());
        }
    }

    void AimRotation()
    {
        //angle between moveInput and x-axis
        //if not zero then rotate the about z-axis with that angle 
        //Vector3 v3 = Vector3.left * LastmoveInput.x + Vector3.down * LastmoveInput.y;
        float angle = Vector2.SignedAngle(Vector2.right, LastmoveInput);
        playerFacingTowards.eulerAngles = new Vector3(0, 0, angle);
    }

    public IEnumerator Dash()
    {
        rb.linearVelocity = LastmoveInput.normalized * dashSpeed;
        yield return waitForSeconds;
        ChangeState(PlayerStates.Idle);
        rb.linearVelocity = Vector2.zero;
    }
    public void ChangeState(PlayerStates newState)
    {
        // Reset all bools first
        anim.SetBool("isIdle", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isAttacking", false);
        anim.SetBool("isDashing", false);

        // Set the correct one
        switch (newState)
        {
            case PlayerStates.Idle: anim.SetBool("isIdle", true); break;
            case PlayerStates.Run: anim.SetBool("isRunning", true); break;
            case PlayerStates.Attacking: anim.SetBool("isAttacking", true); break;
            case PlayerStates.Dashing: anim.SetBool("isDashing", true); break;
        }
        currentState = newState;
    }
}
public enum PlayerStates {Idle, Run, Attacking, Dashing};
/*Idle will be dfault State
 * Run animation will be played if
 * 2.Move keys are used
 * 
 * Attacking animation will be played if
 * 1. 
 * 2. 
 * 
 * Dashing animation will be played if 
 * 1. Shift Key is pressed 
 * 2. Will not interupt ongoing Attack Animation
 * 3. Interupts Idle and Running Animation
 */


