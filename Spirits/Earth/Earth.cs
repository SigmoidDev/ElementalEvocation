using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Earth : Spirit
{
    [Header("Projectile")]
    public GameObject prefab;

    public override void Attack()
    {
        FriendlyProjectile projectile = Instantiate(prefab, (Vector2) target.transform.position + new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.identity, parent).GetComponent<FriendlyProjectile>();
        projectile.owner = this;
    }

    public override void OnProjectileHit(Tower hit)
    {
        if(hit == null){ return; }
        hit.StartFlash();
        hit.health -= atk;
        ManaManager.Instance.mana += (int) Mathf.Ceil(atk * 0.2f * hit.manaMult * UpgradeManager.ManaGain);
    }
}
