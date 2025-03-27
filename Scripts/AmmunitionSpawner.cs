using UnityEngine;

public class AmmunitionSpawner : MonoBehaviour
{
  [SerializeField]
  private int ChanceForAmmoSpawn = 30;
  private const int ChanceForMinigun = 45;
  private const int ChanceForRocket = 35;
  private const int ChanceForSniper = 20;
  [SerializeField]
  private Ammunition[] ammunitionPrefabs;

  public void SpawnRandomAmmunition()
  {
    if (Random.Range(0, 100) < ChanceForAmmoSpawn)
    {
      var weaponTypeChance = Random.Range(0, 100);
      if (weaponTypeChance < ChanceForMinigun)
        Instantiate(ammunitionPrefabs[(int)WeaponType.Minigun], transform.position, Quaternion.identity);
      else if (weaponTypeChance < ChanceForMinigun + ChanceForRocket)
        Instantiate(ammunitionPrefabs[(int)WeaponType.RocketLauncher], transform.position, Quaternion.identity);
      else
        Instantiate(ammunitionPrefabs[(int)WeaponType.Sniper], transform.position, Quaternion.identity);
    }
  }
}
