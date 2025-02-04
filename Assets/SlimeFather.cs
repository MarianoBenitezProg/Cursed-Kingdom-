using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SlimeFather : MonoBehaviour, ItakeDamage
{
    [Header("Slime Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private int generationLevel = 0;  
    [SerializeField] private float slimeScale = 1f;    

    [Header("Split Settings")]
    [SerializeField] private GameObject slimePrefab;   
    [SerializeField] private float splitForce = 3f;   

    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float detectionRange = 5f;


    private float currentHealth;



    private void Start()
    {
        currentHealth = maxHealth;
        transform.localScale = Vector3.one * slimeScale;
    }

    private void Update()
    {
    }

    private void Die()
    {
        if (generationLevel < 2) 
        {
            SpawnChildren();
        }
        Destroy(gameObject);
    }

    private void SpawnChildren()
    {
        Instantiate(slimePrefab);
        Instantiate(slimePrefab);
        Instantiate(slimePrefab);
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log(currentHealth);
    }
}
