using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public abstract class FriendlyProjectile : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector] public Spirit owner;
    [HideInInspector] float lifetime;

    public virtual void Update()
    {
        lifetime += Time.deltaTime;
        if(lifetime > 10f){ Destroy(gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Tower")
        {
            OnHit(other.gameObject.GetComponent<Tower>());
        }
    }

    public abstract void OnHit(Tower hit);
}
