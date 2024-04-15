using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public abstract class Spirit : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask mask;
    public SpriteRenderer sprite;
    [Space]

    [Header("Info")]
    [Min(0)] public int manaCost;
    [Space]

    [HideInInspector] public int health;
    [HideInInspector] public bool random;

    [Header("Stats")]
    [Min(0)] public int hp;
    [Min(0)] public int atk;
    [Min(0)] public int spd;
    [Min(0)] public int agr;
    [Min(0)] public float rng;
    [Min(0)] public float cd;
    [Space]

    protected Transform parent;
    protected float cooldown;
    [HideInInspector] public Tower target;

    void Start()
    {
        health = hp;
        StartCoroutine(SetRandom());
        parent = GameObject.Find("Projectiles").transform;
    }

    void Update()
    {
        if(health <= 0)
        {
            SpiritController.Instance.troops.Remove(this);
            Destroy(gameObject);
        }

        if(rb.velocity.x < 0){ sprite.flipX = true; }
        else if(rb.velocity.x > 0){ sprite.flipX = false; }

        if(target == null && TowerManager.Instance.activeTowers.Count > 0)
        {
            List<Tower> vulnerableTowers = TowerManager.Instance.activeTowers.Where(
                t => t.targetedBy.Count <= 5).OrderBy(
                    t => Vector2.Distance(transform.position, t.transform.position) * t.health).ToList();
            if(vulnerableTowers.Count == 0){ return; }

            if(vulnerableTowers[0] == TowerManager.Instance.castle && vulnerableTowers.Count > 1){ target = vulnerableTowers[1]; }
            else{ target = vulnerableTowers[0]; }
            target.targetedBy.Add(this);
        }

        if(target == null || !SpiritController.Instance.waveStarted){ return; }

        cooldown -= Time.deltaTime;
        float distance = Vector2.Distance(transform.position, target.transform.position);
        if(distance <= rng)
        {
            rb.velocity = Vector2.zero;
            if(cooldown <= 0f)
            {
                Attack();
                cooldown = cd;
            }
            return;
        }

        Vector2 direction = (target.transform.position - transform.position);
        RaycastHit2D fhit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 0.8f), CapsuleDirection2D.Vertical, 0f, direction, 2f, mask);
        if(fhit.collider == null){ rb.velocity = direction.normalized * spd; return; }

        Vector2 left = Rotate(direction, -45f);
        RaycastHit2D lhit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 0.8f), CapsuleDirection2D.Vertical, 0f, left, 2f, mask);
        if(lhit.collider == null){ rb.velocity = left.normalized * spd; return; }

        Vector2 right = Rotate(direction, 45f);
        RaycastHit2D rhit = Physics2D.CapsuleCast(transform.position, new Vector2(0.5f, 0.8f), CapsuleDirection2D.Vertical, 0f, right, 2f, mask);
        if(rhit.collider == null){ rb.velocity = right.normalized * spd; return; }

        rb.velocity = (Rotate(direction, (random ? 135f : 225f))).normalized * spd;
    }

    Vector2 Rotate(Vector2 orig, float ang)
    {
        float rad = Mathf.Deg2Rad * ang;
        return new Vector2(
            (Mathf.Cos(rad) * orig.x) - (Mathf.Sin(rad) * orig.y),
            (Mathf.Sin(rad) * orig.x) + (Mathf.Cos(rad) * orig.y));
    }

    IEnumerator SetRandom()
    {
        random = Random.Range(0, 2) > 0;
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        StartCoroutine(SetRandom());
    }

    public void StartFlash()
    {
        sprite.color = new Color(1f, 0.45f, 0.55f);
        sprite.DOColor(Color.white, 0.5f);
    }

    public abstract void Attack();
    public abstract void OnProjectileHit(Tower hit);
}
