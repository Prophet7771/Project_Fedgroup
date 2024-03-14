using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSystem : MonoBehaviour
{
    #region Variables

    [Header("References")]
    public PlayerInputActions inputActions;
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsDamagable;

    [Header("Gun Stats")]
    public bool automaticFire;
    public bool multiShot;
    public int damage, magazineSize, bulletsPerTap;
    private int bulletsLeft;
    public float spread, range, reloadTime, timeBetweenShooting, timeBetweenShots;

    [Header("Weapon Preferences")]
    public GameObject muzzleFlash, bulletTracer, bulletImpact;
    // public GameObject muzzleFlash, bulletHole;

    // Basic Booleans
    bool shooting, readyToShoot = true, reloading, mouseClicked;

    [Header("UI")]
    [SerializeField] GameObject reloadText;
    Coroutine rText;

    #endregion

    #region Event Handlers

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.BasicActions.Shoot.performed += ShootStart;
        inputActions.BasicActions.Shoot.canceled += ShootStop;
        inputActions.BasicActions.Reload.performed += WeaponReload;
    }

    private void OnDisable()
    {
        inputActions.BasicActions.Shoot.performed -= ShootStart;
        inputActions.BasicActions.Shoot.canceled -= ShootStop;
        inputActions.BasicActions.Reload.performed -= WeaponReload;

        inputActions.Disable();
    }

    #endregion

    #region Basic Functions

    private void Awake()
    {
        inputActions = new PlayerInputActions();

        fpsCam = Camera.main;
    }

    private void Start()
    {
        ReloadFinnished();
    }

    // private void Update()
    // {

    // }

    private void FixedUpdate()
    {
        if (automaticFire)
        {
            WeaponShoot(mouseClicked);
        }
        else
        {
            WeaponShoot(mouseClicked);
            mouseClicked = false;
        }
    }

    private void WeaponShoot(bool shoot)
    {
        if (Time.timeScale == 0) return;

        shooting = shoot;

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            Instantiate(muzzleFlash, attackPoint);

            readyToShoot = false;

            if (!multiShot)
                FireShot();
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    FireShot();
                }
            }

            bulletsLeft--;

            Invoke("ResetShot", timeBetweenShooting);
        }

        if (bulletsLeft <= 0)
            ToggleReloadText(true);
    }

    private void FireShot()
    {
        // Storing Forward Direction
        Vector3 forward = attackPoint.forward;

        // Random Spread Value
        float x = Random.Range(-spread, spread);
        float y = 0;

        // Calculating Direction with Spread
        Vector3 direction = forward + new Vector3(x, y, 0);

        Ray ray = new Ray(attackPoint.transform.position, direction);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 10);

        RaycastHit rayHit;

        GameObject tracer = Instantiate(bulletTracer, attackPoint.position, Quaternion.identity);
        tracer.transform.LookAt(attackPoint.transform.position + direction * range);

        // RayCast
        if (Physics.Raycast(ray, out rayHit, range, whatIsDamagable))
        {
            Debug.Log($"What we Hit: {rayHit.transform.name}");

            Instantiate(bulletImpact, rayHit.point, Quaternion.identity);

            HealthSystem damageObject = rayHit.collider.GetComponent<HealthSystem>();

            if (damageObject)
                damageObject.TakeDamage(damage);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void WeaponReload(InputAction.CallbackContext ctx)
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            Debug.Log("RELOADING");
            reloading = true;
            Invoke("ReloadFinnished", reloadTime);
        }
    }

    private void ReloadFinnished()
    {
        Debug.Log("RELOAD FINNISHED");
        bulletsLeft = magazineSize;
        reloading = false;

        TurnOfReloadText();
    }

    private void ShootStart(InputAction.CallbackContext ctx) => mouseClicked = true;
    private void ShootStop(InputAction.CallbackContext ctx) => mouseClicked = false;

    private void ToggleReloadText(bool value)
    {
        if (rText != null)
            StopCoroutine(rText);

        reloadText.SetActive(value);

        rText = StartCoroutine("ToggleReloadOverTime");
    }

    private void TurnOfReloadText()
    {
        reloadText.SetActive(false);

        if (rText != null)
            StopCoroutine(rText);
    }

    IEnumerator ToggleReloadOverTime()
    {
        reloadText.SetActive(true);

        yield return new WaitForSeconds(1f);

        reloadText.SetActive(false);
    }

    #endregion
}
