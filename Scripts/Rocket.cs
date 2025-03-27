using UnityEngine;

public class Rocket : Projectile
{
	[SerializeField]
	private float range;

	protected override void HitEnemy(Collider other)
	{
		var colliderHit = Physics.OverlapSphere(transform.position, range);
		foreach (var col in colliderHit)
			if (col.tag.Equals("Enemy") && col.TryGetComponent(out Health enemy))
				enemy.TakeDamage(damage);
	}
}
