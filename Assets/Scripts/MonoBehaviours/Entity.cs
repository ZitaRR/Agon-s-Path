using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EdgeCollider2D))]
public abstract class Entity : MonoBehaviour, IDamagable
{
    public Transform Offset { get; private set; }
    public float Health
    {
        get => health.Value;
        set => health.Value = value;
    }
    public float ViewDistance { get => viewDistance.TotalValue; }
    public int ID { get; private set; }
    public bool IsDead { get => health.IsEmpty; }

    protected new Rigidbody2D rigidbody;
    protected new SpriteRenderer renderer;
    protected Animator animator;
    protected PlayerEntity player;
    protected Vector2 movement;
    protected Vector2 direction;
    protected Vector2Int position;
    protected Color color;
    private static int entities = 0;

    [Header("Stats")]
    [SerializeField]
    protected AttributeStat speed;
    [SerializeField]
    protected ResourceStat health;
    [SerializeField]
    protected AttributeStat viewDistance;
    [SerializeField]
    protected AttributeStat attackRange;

    [Header("Misc")]
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

        speed = new AttributeStat(nameof(speed), speed.BaseValue);
        health = new ResourceStat(nameof(health), health.BaseValue);
        viewDistance = new AttributeStat(nameof(viewDistance), viewDistance.BaseValue);
        attackRange = new AttributeStat(nameof(attackRange), attackRange.BaseValue);

        health.OnDepletion += (ResourceStat stat) =>
        {
            Debug.Log($"{name}'s {stat.Name} is depleted!");
            Kill();
        };
    }

    protected virtual void Update()
    {
        SetAlpha();

        if (IsDead)
            return;

        position = Vector2Int.FloorToInt(transform.position);
        Movement();
        Rotation();
    }

    //This should be moved into a base class for NPC's/AI
    private void SetAlpha()
    {
        if (this is PlayerEntity)
            return;

        float alpha = GameManager.Environment.LightIntensity;
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= player.viewDistance.TotalValue)
        {
            alpha = Mathf.Clamp(player.viewDistance.TotalValue - distance, alpha, 1);
            renderer.color = new Color(color.r, color.g, color.b, alpha);
            return;
        }

        renderer.color = new Color(color.r, color.g, color.b, alpha);
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

    public IEnumerator Damage(float damage, Vector2 direction)
    {
        if (IsDead)
            yield break;

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
        if(this is PlayerEntity)
        {
            GameManager.Instance.LoadScene("Menu");
            return;
        }

        animator.SetBool("Dead", IsDead);
        CombatSystem.RemoveCombatant(this);
        Destroy(gameObject, 100f);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance.TotalValue);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange.TotalValue);
    }
}
