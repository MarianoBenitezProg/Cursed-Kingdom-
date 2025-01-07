using System.Collections.Generic;
using UnityEngine;

public class ProyectilePool : MonoBehaviour
{
    public static ProyectilePool Instance;

    [System.Serializable]
    public class ProjectilePrefab
    {
        public ProjectileType type;
        public GameObject prefab;
    }

    [SerializeField] public List<ProjectilePrefab> projectilePrefabs; // Lista de prefabs por tipo
    [SerializeField] private int poolSizePerType = 3;

    private Dictionary<ProjectileType, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<ProjectileType, Queue<GameObject>>();

        foreach (var projectilePrefab in projectilePrefabs)
        {
            var poolQueue = new Queue<GameObject>();

            for (int i = 0; i < poolSizePerType; i++)
            {
                GameObject obj = Instantiate(projectilePrefab.prefab, transform);
                obj.SetActive(false);
                poolQueue.Enqueue(obj);
            }

            poolDictionary.Add(projectilePrefab.type, poolQueue);
        }
    }

    public GameObject GetObstacle(ProjectileType type)
    {
        if (poolDictionary.ContainsKey(type) && poolDictionary[type].Count > 0)
        {
            GameObject obj = poolDictionary[type].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        Debug.LogWarning($"No hay proyectiles disponibles para el tipo {type}. Considera aumentar el tamaño del pool.");
        return null;
    }

    public void ReturnObstacle(GameObject obj, ProjectileType type)
    {
        obj.SetActive(false);

        if (poolDictionary.ContainsKey(type))
        {
            poolDictionary[type].Enqueue(obj);
        }
        else
        {
            Debug.LogError($"El tipo {type} no está registrado en el pool.");
        }
    }
}