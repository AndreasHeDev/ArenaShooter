using System;
using UnityEngine;

public class Player : Character
{
  public static Player Instance {get; private set;}
  private Camera cam;
  private Rigidbody rb;
  [SerializeField]
  private float speed = 10f;
  [SerializeField]
  private LayerMask groundMask;
  [SerializeField]
  private Weapon[] weapons;
  private Weapon currentWeapon;
  private float horizontal;
  private float vertical;
  public event Action<Weapon> OnWeaponChange;

  void Awake()
  {
    if (Instance == null)
      Instance = this;
    else
      Debug.Log("There are more than 1 Instance of Player");
  }

  void Start()
  {
    cam = Camera.main;
    rb = GetComponent<Rigidbody>();
    EquipWeapon(WeaponType.Pistole);
    enabled = false;
  }

  void Update()
  {
    MouseInput();
    KeyInput();
    MovementInput();
  }

  void FixedUpdate() => rb.velocity = new Vector3(-vertical, 0, horizontal) * speed;

  public override void Revive()
  {
    base.Revive();
    foreach (var weapon in weapons)
      weapon.SetStartValues();
    SwitchWeapon(WeaponType.Pistole);
  }

  private void MouseInput()
  {
    RaycastHit hit;
    var ray = cam.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, 100, groundMask))
      transform.LookAt(hit.point);
    if (Input.GetMouseButton(0))
      currentWeapon.Shoot(hit.point);
  }

  private void KeyInput()
  {
    if (Input.GetKeyDown(KeyCode.R))
      currentWeapon.Reload();
    if (Input.GetKeyDown(KeyCode.Alpha1))
      SwitchWeapon(WeaponType.Pistole);
    if (Input.GetKeyDown(KeyCode.Alpha2))
      SwitchWeapon(WeaponType.Minigun);
    if (Input.GetKeyDown(KeyCode.Alpha3))
      SwitchWeapon(WeaponType.RocketLauncher);
    if (Input.GetKeyDown(KeyCode.Alpha4))
      SwitchWeapon(WeaponType.Sniper);
  }

  private void MovementInput()
  {
    horizontal = Input.GetAxisRaw("Horizontal");
    vertical = Input.GetAxisRaw("Vertical");
  }

  public void SwitchWeapon(WeaponType type)
  {
    currentWeapon.SetModelsVisible(false);
    EquipWeapon(type);
  }

  private void EquipWeapon(WeaponType type)
  {
    currentWeapon = weapons[(int)type];
    currentWeapon.SetModelsVisible(true);
    OnWeaponChange?.Invoke(currentWeapon);
  }

  public void AddAmmunition(WeaponType type, int amount) => weapons[(int)type].AddAmmunition(amount);
  public void ReloadCurrentWeapon() => currentWeapon.Reload();
}
