using System;
using UnityEngine;

public class Health : MonoBehaviour
{
  private Character character;
  [SerializeField]
  private int maxHealth;
  private int currentHealth;
  private bool isDead;
  public event Action<int, int> OnHealthChange;

  void Awake()
  {
    character = GetComponent<Character>();
    currentHealth = maxHealth;
  }

  public void Revive()
  {
    currentHealth = maxHealth;
    OnHealthChange?.Invoke(maxHealth, currentHealth);
    isDead = false;
    character.Revive();
  }

  public void TakeDamage(int damage)
  {
    if (isDead)
      return;
    currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
    OnHealthChange?.Invoke(maxHealth, currentHealth);
    if (currentHealth <= 0)
    {
      isDead = true;
      character.Die();
    }
  }
}
