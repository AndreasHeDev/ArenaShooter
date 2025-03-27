using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
  public event Action<Character> OnCharacterDie;

  public virtual void Die()
  {
    OnCharacterDie?.Invoke(this);
    enabled = false;
  }

  public virtual void Revive() => enabled = true;
}
