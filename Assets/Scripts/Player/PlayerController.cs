using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;


    [Header("Movement")]
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundMaxSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float airMaxSpeed;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private GroundCheck groundCheck;

    [Header("References")]
    [SerializeField] private Transform eyes;

    private bool isInvincible;
    private float invincTimer;
    private bool flaggedForDeath;
    private float deathTimer;

    public enum State
    {
        Grounded,
        Airborne
    }

    private State state;
    private Rigidbody rb;
    private Vector3 input = Vector2.zero;
    private float jumpTimer;
    private float health;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        HUDController.instance.SetMaxHealth(maxHealth);
        Respawn();
    }

    void Update()
    {
        if (flaggedForDeath)
        {
            input = Vector3.zero;
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                SceneManager.LoadScene("GameOver Menu");
            }
            return;
        }

        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (state == State.Grounded)
        {
            if (Input.GetButton("Jump"))
            {
                SetAirborne();
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            if (!groundCheck.IsGrounded())
            {
                SetAirborne();
            }
        }

        if (state == State.Airborne)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0 && groundCheck.IsGrounded())
            {
                SetGrounded();
            }
        }

        //timer invincible
        if (invincTimer >= 0)
        {
            invincTimer -= Time.deltaTime;
            if (invincTimer <= 0)
            {
                isInvincible = false;
            }
        }
    }

    void FixedUpdate()
    {
        // Gravity
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        if (flaggedForDeath)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        if (state == State.Grounded)
        {
            if (rb.velocity.magnitude < groundMaxSpeed)
            {
                rb.AddRelativeForce(input * groundAcceleration);
            }
        }

        if (state == State.Airborne)
        {
            // Makes it so air strafing is possible but player input can still steer player without breaching max speed
            if (Mathf.Abs(transform.InverseTransformDirection(rb.velocity).x) > airMaxSpeed)
            {
                if (Mathf.Sign(input.x) == Mathf.Sign(transform.InverseTransformDirection(rb.velocity).x))
                {
                    input.x = 0;
                }
            }

            if (Mathf.Abs(transform.InverseTransformDirection(rb.velocity).z) > airMaxSpeed)
            {
                if (Mathf.Sign(input.z) == Mathf.Sign(transform.InverseTransformDirection(rb.velocity).z))
                {
                    input.z = 0;
                }
            }
            rb.AddRelativeForce(input * airAcceleration);
        }
    }

    private void SetAirborne()
    {
        state = State.Airborne;
        rb.drag = airDrag;
        jumpTimer = 0.2f;
    }

    private void SetGrounded()
    {
        state = State.Grounded;
        rb.drag = groundDrag;
    }

    public void Damage(float damage)
    {
        if (isInvincible || flaggedForDeath)
        {
            return;
        }

        health -= damage;
        CameraController.instance.Shake(damage / 30f);
        HUDController.instance.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        flaggedForDeath = true;
        deathTimer = 3f;
        SetAirborne();
        transform.DORotate(Vector3.forward * 90, 1.2f).SetEase(Ease.OutBounce);
        CameraController.instance.DeathAnimation();
        ItemBelt.instance.FlagForDeath();
    }

    public void Respawn()
    {
        health = maxHealth;
        flaggedForDeath = false;
        SetAirborne();
        var respawnPosition = ProgressionManager.instance.GetActiveCheckpointPosition();
        if (respawnPosition != Vector3.zero)
        {
            transform.position = respawnPosition;
        }
    }

    public void AddExplosionForce(float explosionForce, Vector3 explosionPosition)
    {
        // Send player off the ground if grounded.
        if (state == State.Grounded && explosionPosition.y > transform.position.y)
        {
            explosionPosition = new Vector3(explosionPosition.x, transform.position.y - 1, explosionPosition.z);
        }
        var direction = (transform.position - explosionPosition).normalized;
        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Adds an Impulse force to the player.
    /// </summary>
    /// <param name="force"></param>
    public void AddForce(Vector3 force)
    {
        SetAirborne();
        bool hor = force.x != 0 || force.z != 0;
        rb.velocity = new Vector3(hor ? force.x : rb.velocity.x, force.y, hor ? force.z : rb.velocity.z);
    }

    public Transform GetEyes()
    {
        return eyes;
    }

    public State GetState()
    {
        return state;
    }

    public void MakeInvincible(float duration)
    {
        isInvincible = true;
        invincTimer = duration;
        HUDController.instance.InvincibleOverlay(duration);
    }

    public void Heal(float healpoints)
    {
        health += healpoints;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
        HUDController.instance.SetHealth(health);
    }
}

   
