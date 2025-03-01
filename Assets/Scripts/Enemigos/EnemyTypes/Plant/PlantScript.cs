using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantScript : Enemy, ItakeDamage
{
    private float cooldownTimer;

    public override void Attack()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= 3f) // Dispara cada 3 segundos
        {
            GameObject disparo = ProyectilePool.Instance.GetObstacle(ProjectileType.PlantAtack);
            disparo.transform.position = player.transform.position; // Spawnea sobre el jugador
            cooldownTimer = 0f;
        }

        if (isDying)
        {
            _animator.SetBool("IsDying", true);
        }
    }

    public override void Seek()
    {
        _animator.SetBool("IsAwake", true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var ability = collision.gameObject.GetComponent<Ability>();
        if (ability != null)
        {
            TakeDamage(ability.dmg);
        }
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(health);
        _tintMaterial.SetTintColor(Color.red);
    }
}
