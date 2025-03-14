using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField]
    private Transform projectileSpawnPoint;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        // Get Bullet from Pool
        GameObject projectile = ProjectilePool.Instance.GetBullet();

        if (projectile == null)
        {
            return;
        }

        // set position of bullet to spawn point
        projectile.transform.position = projectileSpawnPoint.position;

        // calculate direction to shoot (mousecursor position)
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = (worldMousePos - (Vector2)projectileSpawnPoint.position).normalized;

        projectile.GetComponent<Projectile>().Fire(direction);
    }
}