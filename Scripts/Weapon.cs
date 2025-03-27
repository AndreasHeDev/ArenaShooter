using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  [SerializeField]
  private WeaponData weapon;
  [SerializeField]
  private GameObject model;
  [SerializeField]
  private Transform spawnPoint;
  [SerializeField]
  private Projectile bulletPrefab;
  public int TotalAmmunition {get; private set;}
  public int CurrentAmmunition {get; private set;}
  private float lastTimeShot;
  private bool isReloading;
  public bool IsReloading => isReloading;
  public event Action<float> OnReloadTime;
  public event Action<int, int> OnAmmunitionChange;

  void Start() => SetStartValues();

  public void SetStartValues()
  {
    CurrentAmmunition = weapon.ReloadAmmoAmount;
    TotalAmmunition = 0;
    isReloading = false;
    OnAmmunitionChange?.Invoke(TotalAmmunition, CurrentAmmunition);
    lastTimeShot -= weapon.ShootDelay;
    StopAllCoroutines();
  }

  public void SetModelsVisible(bool visible) => model.SetActive(visible);
  public Sprite GetWeaponImage() => weapon.Image;

  public void AddAmmunition(int amount)
  {
    TotalAmmunition += amount;
    OnAmmunitionChange?.Invoke(TotalAmmunition, CurrentAmmunition);
    if (CurrentAmmunition <= 0)
      Reload();
  }

  public void Shoot(Vector3 targetPosition)
  {
    if (isReloading || CurrentAmmunition <= 0 || lastTimeShot + weapon.ShootDelay > Time.time)
      return;
    lastTimeShot = Time.time;
    targetPosition.y = spawnPoint.position.y;
    ShootBullet(targetPosition);
    CurrentAmmunition--;
    OnAmmunitionChange?.Invoke(TotalAmmunition, CurrentAmmunition);
    if (CurrentAmmunition <= 0)
      Reload();
  }

  private void ShootBullet(Vector3 targetPosition)
  {
    var direction = (targetPosition - spawnPoint.position).normalized;
    var bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
    bullet.Setup(direction);
  }

  public void Reload()
  {
    if (isReloading || TotalAmmunition == 0 || CurrentAmmunition == weapon.ReloadAmmoAmount)
      return;
    else
      StartCoroutine(ReloadTimer());
  }

  private IEnumerator ReloadTimer()
  {
    isReloading = true;
    var startingPoint = 0f;
    var finalPoint = 1f;
    var currentTime = 0f;
    while (weapon.ReloadTime > currentTime)
    {
      yield return null;
      currentTime += Time.deltaTime;
      OnReloadTime?.Invoke(Mathf.Lerp(startingPoint, finalPoint, currentTime / weapon.ReloadTime));
    }
    OnReloadTime?.Invoke(finalPoint);
    isReloading = false;
    UpdateAmmunition();
  }

  private void UpdateAmmunition()
  {
    int amountToReload = weapon.ReloadAmmoAmount - CurrentAmmunition;
    if (TotalAmmunition > amountToReload)
    {
      TotalAmmunition -= amountToReload;
      CurrentAmmunition += amountToReload;
    }
    else
    {
      CurrentAmmunition += TotalAmmunition;
      TotalAmmunition = 0;
    }
    OnAmmunitionChange?.Invoke(TotalAmmunition, CurrentAmmunition);
  }
}
