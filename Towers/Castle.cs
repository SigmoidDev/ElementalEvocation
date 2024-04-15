using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Castle : Tower
{
    public GameObject prefab;

    public override void Attack(Spirit victim)
    {
        EnemyProjectile projectile = Instantiate(prefab, (Vector2) transform.position - Vector2.right, Quaternion.identity, parent).GetComponent<EnemyProjectile>();
        projectile.owner = this;
        (projectile as LightningBolt).Stretch(projectile.transform.position, victim.transform.position, false);
    }

    public override void OnProjectileHit(Spirit hit)
    {
        hit.StartFlash();
        hit.health -= atkDMG;
    }
}
