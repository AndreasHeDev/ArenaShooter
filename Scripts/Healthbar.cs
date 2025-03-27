using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
	[SerializeField]
	private Image healthbar;

	void Start() => Player.Instance.GetComponent<Health>().OnHealthChange += OnHealthChange;
	void OnDestroy() => Player.Instance.GetComponent<Health>().OnHealthChange -= OnHealthChange;
	private void OnHealthChange(int maxHealth, int currentHealth) => healthbar.fillAmount = (float)currentHealth / maxHealth;
}
