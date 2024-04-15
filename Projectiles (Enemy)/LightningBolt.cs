using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LightningBolt : EnemyProjectile
{
	public void Stretch(Vector3 start, Vector3 end, bool flip)
    {
		Vector3 centerPos = (start + end) / 2f;
		transform.position = centerPos;

		Vector3 direction = end - start;
		direction = Vector3.Normalize(direction);
		transform.right = direction;

		if(flip){ transform.right *= -1f; }
		Vector3 scale = new Vector3(1, 1, 1);
		scale.x = Vector3.Distance(start, end);
		transform.localScale = scale;
	}

	public override void OnHit(Spirit hit)
    {
		StartCoroutine(DestroyWhenReady());
        owner.OnProjectileHit(hit);
    }

	IEnumerator DestroyWhenReady()
	{
		yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
	}
}
