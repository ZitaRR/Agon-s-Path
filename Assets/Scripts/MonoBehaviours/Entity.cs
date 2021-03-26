using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(EdgeCollider2D))]
public abstract class Entity : MonoBehaviour, IDamagable
{
    public delegate void DestroyDelegate();
    public event DestroyDelegate OnDestroyed;

    public Transform Offset { get; private set; }
    public ResourceStat Health { get => health; }
    public ResourceStat Mana { get => mana; }
    public AttributeStat KineticMultipler { get => kineticMultiplier; }
    public AttributeStat SpellMulitplier { get => spellMultiplier; }
    public List<Spell> Spells { get => spells; }
    public float ViewDistance { get => viewDistance.TotalValue; }
    public int ID { get; private set; }
    public bool IsDead { get => health.IsEmpty; }

    protected new Rigidbody2D rigidbody;
    protected new SpriteRenderer renderer;
    protected Animator animator;
    protected PlayerEntity player;
    protected Vector2 movement;
    protected Vector2 direction;
    protected Color color;
    private static int entities = 0;

    [Header("Stats")]
    [SerializeField]
    protected ResourceStat health;
    [SerializeField]
    protected ResourceStat mana;
    [SerializeField]
    protected AttributeStat kineticMultiplier;      //Currently using kinetic as our total melee damage
    [SerializeField]                                //This should mutliply the current equipped weapon 
    protected AttributeStat spellMultiplier;        //base damage in the future
    [SerializeField]
    protected AttributeStat speed;
    [SerializeField]
    protected AttributeStat viewDistance;
    [SerializeField]
    protected AttributeStat attackRange;

    [Header("Spells")]
    [SerializeField]
    protected List<Spell> spells;

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

        health = new ResourceStat("Health", health.BaseValue, this);
        mana = new ResourceStat("Mana", mana.BaseValue, this);
        kineticMultiplier = new AttributeStat("Kinetic", kineticMultiplier.BaseValue, this);
        spellMultiplier = new AttributeStat("Spell", spellMultiplier.BaseValue, this);
        speed = new AttributeStat("Speed", speed.BaseValue, this);
        viewDistance = new AttributeStat("View Distance", viewDistance.BaseValue, this);
        attackRange = new AttributeStat("Attack Range", attackRange.BaseValue, this);

        Health.OnDepletion += OnHealthDepletion;
        Mana.OnDepletion += OnManaDepletion;
    }

    protected virtual void Update()
    {
        SetAlpha();

        if (IsDead)
            return;

        Movement();
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

    public virtual void Movement()
    {
        Rotation();
    }

    public abstract void Attack();

    public IEnumerator Damage(float damage, Vector2 direction)
    {
        if (IsDead)
            yield break;

        Health.Decrease(damage);
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

    public virtual void Kill()
    {
        animator.SetBool("Dead", IsDead);
        CombatSystem.RemoveCombatant(this);
        Destroy(gameObject, 100f);
    }

    protected virtual void OnHealthDepletion(ResourceStat stat)
    {
        Debug.Log($"{name}'s {stat.Name} is depleted!");
        Kill();
    }

    protected virtual void OnManaDepletion(ResourceStat stat)
    {
        Debug.Log($"{name}'s {stat.Name} is depleted!");
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance.TotalValue);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange.TotalValue);
    }
}
