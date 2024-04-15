using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Water : Spirit
{
    [Header("Projectile")]
    public float force;
    public GameObject prefab;
    public LayerMask newMask;

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
        Collider2D[] nearbySpirits = Physics2D.OverlapCircleAll(transform.position, 3f, newMask);
        foreach(Collider2D col in nearbySpirits)
        {
            Spirit spirit = col.transform.parent.gameObject.GetComponent<Spirit>();
            spirit.health += atk;
        }
        ManaManager.Instance.mana += (int) Mathf.Round(UpgradeManager.ManaGain);
    }
}
