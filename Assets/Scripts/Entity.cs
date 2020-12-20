using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Entity : MonoBehaviour, IDamagable
{
    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health <= 0)
                Kill();
        }
    }
    public float ViewDistance { get => viewDistance; }

    protected new Rigidbody2D rigidbody;
    protected new SpriteRenderer renderer;
    protected Animator animator;
    protected GameManager game;
    protected PlayerEntity player;
    protected Vector2 movement;
    protected Vector2 direction;
    protected Vector2Int position;

    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float viewDistance;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected LayerMask targetMask;

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        game = GameManager.Instance;
        player = game.Player;
    }

    protected virtual void FixedUpdate()
    {
        position = Vector2Int.FloorToInt(transform.position);
        Movement();
        Rotation();
        SetAlpha();
    }

    //This should be moved into a base class for NPC's/AI
    private void SetAlpha()
    {
        if (this is PlayerEntity)
            return;

        var distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance > player.viewDistance)
        {
            var light = Mathf.Clamp(game.Environment.LightIntensity * 4, 0, 1);
            renderer.material.color = new Color(1, 1, 1, light);
            return;
        }

        var alpha = Mathf.Clamp(player.viewDistance - distance, 0, 1);
        renderer.material.color = new Color(1, 1, 1, alpha);
    }

    private void Rotation()
    {
        if (movement.x > 0)
            renderer.flipX = false;
        else if (movement.x < 0)
            renderer.flipX = true;
    }

    public void PlayAttack()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        animator.Play("Attack");
    }

    protected abstract void Movement();

    protected abstract void Attack();

    public void Damage(int damage, Vector2 direction)
    {
        Health -= damage;
        rigidbody.AddForce(direction * 40f, ForceMode2D.Force);
    }

    public IEnumerator DamageEffect()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(.3f);
        renderer.material.color = new Color();
    }

    public void Kill()
        => Destroy(gameObject);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
