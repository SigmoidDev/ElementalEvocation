using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Fire : Spirit
{
    [Header("Projectile")]
    public float force;
    public GameObject prefab;

    public override void Attack()
    {
        float flipped = sprite.flipX ? -1f : 1f;

        FriendlyProjectile projectile = Instantiate(prefab, (Vector2) transform.position + Vector2.right * flipped * 0.2f, Quaternion.identity, parent).GetComponent<FriendlyProjectile>();
        projectile.owner = this;
        projectile.transform.right = (target.transform.position - projectile.transform.position).normalized;
        projectile.rb.AddForce(projectile.transform.right * force, ForceMode2D.Impulse);
    }

    public override void OnProjectileHit(Tower hit)
    {
        if(hit == null){ return; }
        hit.health -= atk;
        hit.StartFlash();
        ManaManager.Instance.mana += (int) Mathf.Ceil(atk * 0.2f * hit.manaMult * UpgradeManager.ManaGain);
    }
}
