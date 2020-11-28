using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rb;
    
    [Header("Renderer")]
    public ParticleSystem featherBurst;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [Header("Collisions")]
    [SerializeField] private LayerMask groundLayer;

    public GameObject planet;
    public GameObject projectile;

    [Header("Gravity")]
    public float maxGravity;
    public enum GravityOrientation {Inner = 1, Outer = -1};
    public GravityOrientation gravityOrientation;
    private Vector3 _gravityDirection;

    [Header("Jump")]
    public float jumpForce;
    public float throwForce;
    private bool _isGrounded;

    [Header("Speed")]
    public float speed;
    public float maxSpeed;

    public int GetGravitySign()
    {
        return (gravityOrientation == GravityOrientation.Inner) ? 1 : -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int direction = GetGravitySign();
        _gravityDirection = direction * (planet.transform.position - transform.position).normalized;
        
        // Freeze rotation in the right direction
        Vector3 rotation = transform.rotation.eulerAngles;
        // float angle = - Mathf.Sign(transform.position.x) * Vector3.Angle(new Vector3(0, 1, 0), -_gravityDirection);
        float angle = -Mathf.Sign(transform.position.x - planet.transform.position.x) * 
                      (Vector3.Angle(new Vector3(0, 1, 0), direction * -_gravityDirection) +
                       ((gravityOrientation == GravityOrientation.Inner) ? 0: 1) * 180);
        transform.rotation = Quaternion.Euler(rotation.x, rotation.y, angle);
        
        // Jump
        if (_isGrounded && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2 jumpDirection = -_gravityDirection.normalized;
            _rb.AddForce((jumpForce * 100) * jumpDirection, ForceMode2D.Force);
            gravityOrientation = (gravityOrientation == GravityOrientation.Inner)
                ? GravityOrientation.Outer
                : GravityOrientation.Inner;
            _spriteRenderer.flipX = _spriteRenderer.flipX ? false : true;
        }
        
        // Orientation
        if (Input.GetAxisRaw("Horizontal") != 0f)
        {
            _spriteRenderer.flipX = Mathf.Sign(GetGravitySign() * Input.GetAxisRaw("Horizontal")) > 0 ? false : true;
        }
        
        // Animation
        _animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
    }

    private void FixedUpdate()
    {
        // Gravity
        // float dist = Vector2.Distance(planet.transform.position, transform.position);
        // _rb.AddForce(gravityDirection * (1f - dist / maxGravityDist) * maxGravity);
        _rb.AddForce(_rb.mass * maxGravity * _gravityDirection);
        
        // Horizontal movement controller
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movementDirection = GetGravitySign() * Vector2.Perpendicular(_gravityDirection).normalized;

        float gravityVelocity = Vector2.Dot(_rb.velocity, _gravityDirection);
        float movementVelocity = moveHorizontal * Mathf.Min(Mathf.Abs(Vector2.Dot(_rb.velocity, movementDirection)) + speed, maxSpeed);
        
        _rb.velocity = new Vector2(
            gravityVelocity * _gravityDirection.x + movementVelocity * movementDirection.x,
            gravityVelocity * _gravityDirection.y + movementVelocity * movementDirection.y
        );

        // Check _isGrounded
        _isGrounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
            }
        }
    }

    public void ThrowProjectile(ProjectileController source)
    {
        Transform egg = Instantiate(projectile.transform, transform.position, transform.rotation);
        egg.GetComponent<SpriteRenderer>().color = source.color;
        Rigidbody2D rb = egg.GetComponent<Rigidbody2D>();
        rb.velocity = GetGravitySign() * _gravityDirection * throwForce;
        
        // Feather burst
        ParticleSystem.MainModule main = featherBurst.main;
        main.startColor = source.color;
        featherBurst.Play();
    }
}
