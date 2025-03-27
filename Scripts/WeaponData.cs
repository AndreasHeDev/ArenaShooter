using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "ScriptableObjects/Weapon")]
public class WeaponData : ScriptableObject
{
  public Sprite Image;
  public int ReloadAmmoAmount;
  public float ReloadTime;
  public float ShootDelay;
}
