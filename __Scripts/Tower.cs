using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public abstract class Tower : MonoBehaviour
{
    [Header("Stats")]
    [Min(0)] public int maxHP;
    [Min(0)] public int atkDMG;
    [Min(0)] public float atkSPD;
    [Min(0)] public float atkRNG;
    [Space]

    [Header("Game")]
    [Min(0)] public int cost;
    [Min(0)] public float manaMult = 1f;
    [Space]

    [HideInInspector] public int health;
    [HideInInspector] private int _health;
    [HideInInspector] private float elapsed = 1f;

    [HideInInspector] public HealthBar bar;
    public GameObject barPrefab;
    [HideInInspector] public Transform worldCanvas;
    public SpriteRenderer sprite;
    [HideInInspector] public Transform parent;

    [HideInInspector] public List<Spirit> targetedBy;
    [HideInInspector] Collider2D[] spiritBuffer = new Collider2D[60];
    public LayerMask mask;

    [HideInInspector] public Spirit target;
    [HideInInspector] float elapsed2;

    void Start()
    {
        health = maxHP;
        _health = maxHP;
        worldCanvas = GameObject.Find("World Canvas").transform;
        parent = GameObject.Find("Projectiles").transform;
        targetedBy = new List<Spirit>();
    }

    void Update()
    {
        if(!SpiritController.Instance.waveStarted){ return; }

        if(_health != health)
        {
            elapsed = 0f;
            if(bar == null)
            {
                bar = Instantiate(barPrefab, (Vector2) transform.position + new Vector2(0f, -1f), Quaternion.identity, worldCanvas).GetComponent<HealthBar>();
                bar.tower = this;
            }
        }
        elapsed += Time.deltaTime;

        if(bar != null)
        {
            if(elapsed <= 1f){ bar.group.alpha = Mathf.Lerp(bar.group.alpha, 1f, Time.deltaTime * 5f); }
            else{ bar.group.alpha = Mathf.Lerp(bar.group.alpha, 0f, Time.deltaTime * 5f); }
        }

        if(health <= 0)
        {
            UpgradeManager.Instance.crystals++;
            TowerManager.Instance.activeTowers.Remove(this);
            Destroy(bar?.gameObject);
            Destroy(this.gameObject);
        }

        if(target == null)
        {
            List<Spirit> inVicinity = new List<Spirit>();

            Physics2D.OverlapCircleNonAlloc(transform.position, atkRNG, spiritBuffer, mask);
            foreach(Collider2D col in spiritBuffer)
            {
                if(col == null){ break; }
                inVicinity.Add(col.transform.parent.GetComponent<Spirit>());
            }

            if(inVicinity.Count == 0)
            {
                return;
            }

            inVicinity = inVicinity.Where(
                s => s.agr != 0).Where(
                    s => Vector2.Distance(s.transform.position, transform.position) <= atkRNG).OrderBy(
                        s => ((s.transform.position - transform.position).sqrMagnitude / s.agr) / s.health).ToList();
            
            if(inVicinity.Count == 0)
            {
                return;
            }
            target = inVicinity[0];
        }
        if(target == null){ return; }

        elapsed2 -= Time.deltaTime;
        if(elapsed2 <= 0f)
        {
            Attack(target);
            elapsed2 = atkSPD;
        }
    }

    public void StartFlash()
    {
        sprite.color = new Color(1f, 0.45f, 0.55f);
        sprite.DOColor(Color.white, 0.5f);
    }

    public abstract void Attack(Spirit victim);
    public abstract void OnProjectileHit(Spirit hit);
}
