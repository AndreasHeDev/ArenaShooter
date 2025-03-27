using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUi : MonoBehaviour
{
	[SerializeField]
	private Image weaponImage;
	[SerializeField]
	private Image reloadBar;
	[SerializeField]
	private TextMeshProUGUI totalAmmunition;
	[SerializeField]
	private TextMeshProUGUI currentAmmunition;
	private Weapon currentWeapon;
	private Color fullAlpha;
	private Color halfAlpha;

	void Start()
	{
		Player.Instance.OnWeaponChange += OnWeaponChange;
		var color = weaponImage.color;
		fullAlpha = color;
		color.a = 0.5f;
		halfAlpha = color;
	}

	void OnDestroy()
	{
		Player.Instance.OnWeaponChange -= OnWeaponChange;
		if (currentWeapon != null)
			UnsubscribeCurrentWeapon();
	}

	private void OnWeaponChange(Weapon newWeapon)
	{
		if (currentWeapon != null)
			UnsubscribeCurrentWeapon();
		currentWeapon = newWeapon;
		currentWeapon.OnReloadTime += OnReloadTime;
		currentWeapon.OnAmmunitionChange += OnAmmunitionChange;
		weaponImage.sprite = currentWeapon.GetWeaponImage();
		if (!currentWeapon.IsReloading)
			OnReloadTime(1);
		OnAmmunitionChange(currentWeapon.TotalAmmunition, currentWeapon.CurrentAmmunition);
	}

	private void UnsubscribeCurrentWeapon()
	{
		currentWeapon.OnReloadTime -= OnReloadTime;
		currentWeapon.OnAmmunitionChange -= OnAmmunitionChange;
	}

	private void OnReloadTime(float percentage)
	{
		reloadBar.fillAmount = 1 - percentage;
		if (percentage == 1f)
			weaponImage.color = fullAlpha;
		else
			weaponImage.color = halfAlpha;
	}

	private void OnAmmunitionChange(int totalAmmunition, int currentAmmunition)
	{
		this.totalAmmunition.text = totalAmmunition.ToString();
		this.currentAmmunition.text = currentAmmunition.ToString();
		if (currentAmmunition == 0)
			weaponImage.color = halfAlpha;
	}
}
