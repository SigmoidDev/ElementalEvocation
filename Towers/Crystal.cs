using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Crystal : Tower
{
    public GameObject prefab;
    public float force;

    public override void Attack(Spirit victim)
    {
        EnemyProjectile projectile = Instantiate(prefab, (Vector2) transform.position - Vector2.right, Quaternion.identity, parent).GetComponent<EnemyProjectile>();
        projectile.owner = this;
        projectile.transform.right = (victim.transform.position - projectile.transform.position).normalized;
        projectile.rb.AddForce(projectile.transform.right * force, ForceMode2D.Impulse);
    }

    public override void OnProjectileHit(Spirit hit)
    {
        hit.StartFlash();
        hit.health -= atkDMG;
    }
}
