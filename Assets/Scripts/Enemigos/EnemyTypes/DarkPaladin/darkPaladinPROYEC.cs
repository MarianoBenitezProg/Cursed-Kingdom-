using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkPaladinPROYEC : Proyectile
{
    private float Lifetimer;
    public GameObject player;
    public GameObject Father;
    Vector3 pullDirection;
    [SerializeField] private float pullForce = 1f; 
    [SerializeField] private float pullDuration = 0.2f;
    private bool isPulling = false;
    private float pullTimer = 0f;

    public override void Behaviour()
    {
        Lifetimer += Time.deltaTime;
        if (Lifetimer >= destroyTimer)
        {
            ProyectilePool.Instance.ReturnObstacle(gameObject, ProjectileType.DarkPaladinAtack);
            Lifetimer = 0;
        }

        if (gameObject.activeSelf)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }

        if (isPulling)
        {
            pullTimer += Time.deltaTime;
            if (pullTimer <= pullDuration)
            {
                float pullAmount = (pullForce * Time.deltaTime) / pullDuration;
                player.transform.position += pullDirection * pullAmount;
            }
            else
            {
                isPulling = false;
                pullTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            player = other.gameObject;
            Debug.Log("Jugador detectado");
            pullDirection = (Father.transform.position - player.transform.position).normalized;

            // Initialize pull effect
            isPulling = true;
            pullTimer = 0f;

            // Apply damage
            var p_behaviour = player.GetComponent<P_Behaviour>();
            p_behaviour.TakeDamage(dmg);
        }
    }
}