using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private string projectileType;
    [SerializeField]
    private string projectileSound;
    private SpriteRenderer mySpriteRenderer;
    private Queue<Monster> monsters = new Queue<Monster>();
    private bool canAttack = true;
    private float attackTimer;
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float projectileSpeed;
    public float ProjectileSpeed { get => projectileSpeed; }
    private Monster target;
    [SerializeField]
    private int damage;
    public Monster Target { get => target; }
    public int Damage { get => damage; }
    public int Price { get; set; }
    public float AttackCooldown { get => attackCooldown; }

    private void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Attack();
    }
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }
    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= AttackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (target == null && monsters.Count > 0)
        {
            target = monsters.Dequeue();
        }
        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();
                canAttack = false;
            }
        }
        else if (monsters.Count>0)
        {
            target = monsters.Dequeue();
        }
        if (target != null && !target.Alive)
        {
            target = null;
        }
    }
    private void Shoot()
    {
        SoundManager.Instance.PlaySfx(projectileSound);
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            monsters.Enqueue(collision.GetComponent<Monster>());
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            GameObject go = other.gameObject;

            if (go.activeInHierarchy)
            {
                target = null;
            }
        }
    }
}
