using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector] public Tower owner;
    [HideInInspector] float lifetime;

    public virtual void Update()
    {
        lifetime += Time.deltaTime;
        if(lifetime > 10f){ Destroy(gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.parent.gameObject.tag == "Spirit")
        {
            OnHit(other.transform.parent.gameObject.GetComponent<Spirit>());
        }
    }

    public abstract void OnHit(Spirit hit);
}
