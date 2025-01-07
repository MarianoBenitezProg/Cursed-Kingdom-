using UnityEngine;

public class ProyectileFactory : MonoBehaviour
{
    [SerializeField] private ProyectilePool projectilePool;

    public GameObject CreateObstacle(ProjectileType type)
    {
        GameObject projectile = projectilePool.GetObstacle(type);

        if (projectile == null)
        {
            // Crear un nuevo proyectil si el pool está vacío
            foreach (var prefab in projectilePool.projectilePrefabs)
            {
                if (prefab.type == type)
                {
                    projectile = Instantiate(prefab.prefab);
                    break;
                }
            }

            if (projectile == null)
            {
                Debug.LogError($"No se pudo encontrar un prefab para el tipo {type}.");
                return null;
            }
        }

        return projectile;
    }
}