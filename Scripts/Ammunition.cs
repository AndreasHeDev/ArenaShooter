using UnityEngine;
using System.Collections.Generic;

public class Ammunition : MonoBehaviour
{
  public static List<Ammunition> SpawnedAmmunition = new List<Ammunition>();
  [SerializeField]
  private WeaponType type;
  [SerializeField]
  private int baseAmount;
  [SerializeField]
  private float randomScalingFactor = 1f;

  void Start()
  {
    SpawnedAmmunition.Add(this);
    var position = transform.position;
    position.y = 0;
    transform.position = position;
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.tag.Equals("Player"))
    {
      Player.Instance.AddAmmunition(type, CalculateAmmunitionAmount());
      Destroy(gameObject);
    }
  }

  void OnDestroy() => SpawnedAmmunition.Remove(this);
  private int CalculateAmmunitionAmount() => (int)(baseAmount * Random.Range(1f, randomScalingFactor));
}
