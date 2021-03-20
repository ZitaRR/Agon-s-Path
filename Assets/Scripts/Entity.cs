using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EdgeCollider2D))]
public abstract class Entity : MonoBehaviour, IDamagable
{
    public Transform Offset { get; private set; }
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
    public int ID { get; private set; }

    protected new Rigidbody2D rigidbody;
    protected new SpriteRenderer renderer;
    protected Animator animator;
    protected PlayerEntity player;
    protected Vector2 movement;
    protected Vector2 direction;
    protected Vector2Int position;
    protected Color color;
    private static int entities = 0;

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
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        Offset = gameObject.transform.Find("Offset");

        ID = entities++;
    }

    protected virtual void Start()
    {
        player = GameManager.Player;
        color = renderer.material.color;
    }

    protected virtual void Update()
    {
        position = Vector2Int.FloorToInt(transform.position);
        Movement();
        Rotation();
        SetAlpha();
    }

    //This should be moved into a base class for NPC's/AI
    private void SetAlpha()
    {
        var alpha = GameManager.Environment.LightIntensity;
        if (GameManager.Environment.IsDay)
        {
            renderer.material.color = new Color(color.r, color.g, color.b, alpha);
            return;
        }

        if (this is PlayerEntity)
            return;

        var distance = Vector2.Distance(transform.position, player.transform.position);
        alpha = Mathf.Clamp(player.viewDistance - distance, 0, 1);
        renderer.material.color = new Color(color.r, color.g, color.b, alpha);
    }

    //Set direction here
    protected virtual void Rotation()
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

    public abstract void Attack();

    public IEnumerator Damage(int damage, Vector2 direction)
    {
        Health -= damage;
        StartCoroutine(DamageEffect());

        float time = 0;
        while (time < .1f)
        {
            transform.position += (Vector3)direction.normalized * 13 * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator DamageEffect()
    {
        color = Color.red;
        yield return new WaitForSeconds(.1f);
        color = new Color(1, 1, 1);
    }

    public void Kill()
    {
        CombatSystem.RemoveCombatant(this);
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
