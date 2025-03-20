using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerProjectileShooter : MonoBehaviour
{
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private ProjectilePool playerProjectilePool;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private Image ammoBar;

    private int currentAmmo;
    private PlayerAnimationController animationController;

    private bool isShooting = false;
    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = maxAmmo;
        animationController = GetComponentInChildren<PlayerAnimationController>();
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
        UpdateAmmoBar();
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
            yield break;
        }

        float elapsedTime = 0f;
        int startAmmo = currentAmmo;

        while (elapsedTime < dynamicReloadTime)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / dynamicReloadTime;

            currentAmmo = startAmmo + Mathf.RoundToInt(progress * (maxAmmo - startAmmo));

            UpdateAmmoBar();

            yield return null;
        }

        currentAmmo = maxAmmo;
        UpdateAmmoBar();

        isReloading = false;

        animationController.SetIsReloading(false);
    }

    private void UpdateAmmoBar()
    {
        if (ammoBar != null)
        {
            ammoBar.fillAmount = (float)currentAmmo / maxAmmo;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && !isReloading && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }
}
