using UnityEngine;

public class Bullet : Projectile
{
	protected override void HitEnemy(Collider other)
	{
		if (other.TryGetComponent(out Health health))
			health.TakeDamage(damage);
	}
}
