using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
	[SerializeField]
	private float speed = 100;
	private Vector3 direction;
	[SerializeField]
	protected int damage;
	private Rigidbody rb;

	public void Setup(Vector3 direction)
	{
		rb = GetComponent<Rigidbody>();
		this.direction = direction;
		transform.LookAt(transform.position + direction);
		StartCoroutine(DestroyShortly());
	}

	void FixedUpdate() => rb.velocity = direction * speed;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Enemy"))
		{
			StopAllCoroutines();
			HitEnemy(other);
			Destroy(gameObject);
		}
	}

	protected abstract void HitEnemy(Collider other);

	private IEnumerator DestroyShortly()
	{
		yield return new WaitForSeconds(10);
		Destroy(gameObject);
	}
}
