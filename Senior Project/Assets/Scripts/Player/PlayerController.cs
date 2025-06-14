using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    public PlayerInputActions playerControls;

    private InputAction move;
    public float moveSpeed = 5f;
    Vector2 moveDirection = Vector2.zero;
    private Vector2 collisionNormal;

    private InputAction jump;
    public float jumpForce = 10f;
    public bool isGrounded = false;

    private InputAction attack;
    public bool attackReady = true;
    public float attackDelay = 1.5f;
    public float attackLength = .1f;
    public GameObject sword;

    public bool facingRight = true;
    public Vector2 wallDetectionSize = new Vector2(2.5f, 1.5f);
    public Vector2 wallDetectionOffset = new Vector2(1.25f, 0f);
    private Vector2 wallDetectionCenter;
    public LayerMask wallLayer;
    Collider2D[] wallHits;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        attack = playerControls.Player.Attack;
        attack.Enable();
        attack.performed += Attack;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(attackReady && wallHits.Length == 0 && !PauseMenu.isPaused)
        {
            StartCoroutine(Attack());
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        attack.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
        if (moveDirection.x > 0)
        {
            facingRight = true;
            wallDetectionCenter = new Vector2(transform.position.x + wallDetectionOffset.x, transform.position.y);
            FaceRight();
        }
        if (moveDirection.x < 0)
        {
            facingRight = false;
            wallDetectionCenter = new Vector2(transform.position.x - wallDetectionOffset.x, transform.position.y);
            FaceLeft();
        }

        wallHits = Physics2D.OverlapBoxAll(wallDetectionCenter, wallDetectionSize, 0f, wallLayer);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }


    void OnCollisionEnter2D(Collision2D collision) 
    {
        collisionNormal = collision.GetContact(0).normal;
        if (collisionNormal.y > 0)
        {
            isGrounded = true; 
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    IEnumerator Attack()
    {
        {
            attackReady = false;
            // Activate the sword
            sword.SetActive(true);

            // Wait for the attack length duration
            yield return new WaitForSeconds(attackLength);

            // Deactivate the sword
            sword.SetActive(false);

            // Wait for the remaining attack interval duration
            yield return new WaitForSeconds(attackDelay - attackLength);
            attackReady = true;
            StopCoroutine(Attack());
        }
    }

    void FaceRight()
    {
        Vector3 scaler = transform.localScale;
        scaler.x = 5.117683f;
        transform.localScale = scaler;
    }

    void FaceLeft()
    {
        Vector3 scaler = transform.localScale;
        scaler.x = -5.117683f;
        transform.localScale = scaler;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallDetectionCenter, wallDetectionSize);
    }
}
