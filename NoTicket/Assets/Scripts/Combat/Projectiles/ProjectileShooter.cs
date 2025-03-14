using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField] 
    private ProjectilePool playerProjectilePool;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        GameObject projectile = playerProjectilePool.GetBullet();

        if (projectile == null)
            return;

        projectile.transform.position = projectileSpawnPoint.position;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = (worldMousePos - (Vector2)projectileSpawnPoint.position).normalized;

        projectile.GetComponent<Projectile>().Fire(direction);
    }
}
