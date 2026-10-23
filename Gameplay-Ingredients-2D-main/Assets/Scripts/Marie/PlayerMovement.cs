using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpStrength = 8f;
    [SerializeField] private float upGravity = 1f, downGravity = 5f;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform feet, headTop;
    [SerializeField] private Checkpoint checkpoint;

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private SoundPlayer _audioPlayer;

    private LayerMask voidLayer;
    private Collider2D _holeCollider;

    private float horizontalMovement = 0;
    private float verticalMovement = 0;
    private bool jump = false, isJumping = false, isGrounded = false, dash = false, isDashing = false, canDash = true;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioPlayer = GetComponent<SoundPlayer>();
        voidLayer = LayerMask.NameToLayer("Void");
    }

    void Update()
    {
        _animator.SetBool("Walk", (horizontalMovement != 0 && isGrounded));
        transform.position += new Vector3(horizontalMovement * Time.deltaTime, 0, 0);

        CheckGround();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.x < 0f)
        {
            horizontalMovement = -_speed;
            _spriteRenderer.flipX = true;
        }
        else if (moveInput.x > 0f)
        {
            horizontalMovement = _speed;
            _spriteRenderer.flipX = false;
        }
        else
        {
            horizontalMovement = 0;
        }

        if (moveInput.y < 0f)
        {
            verticalMovement = -_speed;
        }
        else if (moveInput.y > 0f)
        {
            verticalMovement = _speed;
        }
        else
        {
            verticalMovement = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            jump = true;
            isGrounded = false;
            _animator.SetBool("Jump", true);
            _audioPlayer.PlayAudio(SoundFX.Jump);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && canDash)
        {
            isDashing = true;
            canDash = false;
            dash = true;
        }
        Debug.Log(horizontalMovement + " " + verticalMovement + " " + dash);
    }

    public void FixedUpdate()
    {
        if (jump && isDashing)
        {
            isJumping = true;
        }

        if (jump)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpStrength), ForceMode2D.Impulse);
            _rigidbody.gravityScale = upGravity;
            jump = false;
        }

        if (dash)
        {
            Vector2 force = new Vector2(horizontalMovement, verticalMovement)*6;
            if (force.x != 0f && force.y != 0f) force *= 0.707f;
            _rigidbody.gravityScale = 0;
            downGravity = 0f;
            _rigidbody.linearVelocity = force;
            dash = false;
            Invoke("StopDash", 0.1f);
        }

        if (!isGrounded && _rigidbody.linearVelocity.y < 0)
        {
            _rigidbody.gravityScale = downGravity;
        }
        else if (_rigidbody.linearVelocity.y > 0)
        {
            CheckPlatformAbove();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == groundLayers)
        {
            isGrounded = true;
            _animator.SetBool("Jump", false);
            _rigidbody.gravityScale = upGravity;
        } 
        else if(collision.gameObject.layer == voidLayer)
        {
            collision.collider.isTrigger = true;
            GetComponent<PlayerLife>().Hurt(1);
            Invoke("Respawn", 1f);
            _holeCollider = collision.collider;
        }
    }

    public void Respawn()
    {
        transform.position = checkpoint.transform.position;
        if(_holeCollider != null)
        {
            _holeCollider.isTrigger = false;
            _holeCollider = null;
        }
    }

    public void PlayWalkingSound()
    {
        _audioPlayer.PlayAudio(SoundFX.Walk);
    }
    
    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.transform.position, Vector2.down, 0.1f, groundLayers);
        if (hit)
        {
            isGrounded = true;
            isJumping = false;
            _animator.SetBool("Jump", false);
            _rigidbody.gravityScale = upGravity;
            if (!isDashing) canDash = true;
        }
        else {
            isGrounded = false;
            _animator.SetBool("Jump", true);
            _rigidbody.gravityScale = downGravity;
        }
    }

    private void CheckPlatformAbove()
    {
        RaycastHit2D hit = Physics2D.Raycast(headTop.position, transform.up, 0.3f, groundLayers);
        if (hit)
        {
            GetComponent<Collider2D>().enabled = false;
            InvokeRepeating("CheckForFalldown", 0, Time.fixedDeltaTime);
        }
    }

    private void CheckForFalldown()
    {
        //Am I still just below a platform ?
        RaycastHit2D hit = Physics2D.Raycast(headTop.position, transform.up, 0.1f, groundLayers);
        //If not, is my head still inside a platform ?
        if(!hit) hit = Physics2D.Raycast(headTop.position, -transform.up, 1f, groundLayers);
        //If any of these is true, don't activate the collider just yet !
        if (_rigidbody.linearVelocity.y < 0 && !hit)
        {
            GetComponent<Collider2D>().enabled = true;
            CancelInvoke("CheckForFalldown");
        }
    }

    public void ChangeCheckpoint(Checkpoint newCheckpoint)
    {
        if (checkpoint != newCheckpoint)
        {
            Destroy(checkpoint.gameObject);
        }
        checkpoint = newCheckpoint;
    }

    private void StopDash()
    {
        isDashing = false;
        downGravity = 5f;
        if (!isJumping)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            _rigidbody.linearVelocityX /= 5;
        }
    }
}
