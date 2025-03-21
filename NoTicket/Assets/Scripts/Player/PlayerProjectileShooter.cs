using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerProjectileShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] 
    private Transform projectileSpawnPoint;
    [SerializeField] 
    private ProjectilePool playerProjectilePool;

    [Header("Ammo Settings")]
    [SerializeField] 
    private int maxAmmo;
    [SerializeField] 
    private float reloadTime;

    [Header("UI Needle Settings")]
    [SerializeField] 
    private RectTransform ammoNeedle;
    [SerializeField] 
    private float minNeedleRotation = -90f;
    [SerializeField] 
    private float maxNeedleRotation = 90f;

    private int currentAmmo;
    private PlayerAnimationController animationController;

    private bool isShooting = false;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        animationController = GetComponentInChildren<PlayerAnimationController>();

        UpdateAmmoNeedle();
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentAmmo > 0)
        {
            ShootProjectile();

            if (!isShooting)
            {
                isShooting = true;
                animationController.SetIsShooting(true);
            }
        }

        if (context.canceled)
        {
            if (isShooting)
            {
                isShooting = false;
                animationController.SetIsShooting(false);
            }
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = playerProjectilePool.GetBullet();
        if (projectile == null)
            return;

        projectile.transform.position = projectileSpawnPoint.position;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = (worldMousePos - (Vector2)projectileSpawnPoint.position).normalized;

        projectile.GetComponent<Projectile>().Fire(direction);

        currentAmmo--;
        UpdateAmmoNeedle();
    }

    private System.Collections.IEnumerator Reload()
    {
        if (isReloading)
            yield break;

        isReloading = true;
        animationController.SetIsReloading(true);

        int missingAmmo = maxAmmo - currentAmmo;
        float dynamicReloadTime = reloadTime * ((float)missingAmmo / maxAmmo);

        if (dynamicReloadTime <= 0)
        {
            isReloading = false;
            animationController.SetIsReloading(false);
            yield break;
        }

        float elapsedTime = 0f;
        int startAmmo = currentAmmo;

        while (elapsedTime < dynamicReloadTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / dynamicReloadTime;
            currentAmmo = startAmmo + Mathf.RoundToInt(progress * (maxAmmo - startAmmo));

            UpdateAmmoNeedle();

            yield return null;
        }

        currentAmmo = maxAmmo;
        UpdateAmmoNeedle();

        isReloading = false;
        animationController.SetIsReloading(false);
    }

    private void UpdateAmmoNeedle()
    {
        if (ammoNeedle == null)
            return;

        float ammoPercent = (float)currentAmmo / maxAmmo;
        float needleRotation = Mathf.Lerp(minNeedleRotation, maxNeedleRotation, ammoPercent);

        ammoNeedle.localEulerAngles = new Vector3(0f, 0f, needleRotation);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }
}
