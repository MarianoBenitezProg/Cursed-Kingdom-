using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Proyectile
{
    protected GameObject shootingPivot;
    protected P_Behaviour playerRef;
    protected Direction shootingDir;

    private void Awake()
    {
        shootingPivot = GameObject.Find("Shooting Pivot");
        playerRef = FindObjectOfType<P_Behaviour>();
    }

    private void OnEnable()
    {
        shootingDir = playerRef.lookingDir;
        transform.position = shootingPivot.transform.position;
        isActive = true;
    }

    public void ShootingDirection()
    {
        switch(shootingDir)
        {
            case Direction.Up:
                transform.position += transform.up * speed * Time.deltaTime;
                break;
            case Direction.Down:
                transform.position -= transform.up * speed * Time.deltaTime;
                break;
            case Direction.Right:
                transform.position += transform.right * speed * Time.deltaTime;
                break;
            case Direction.Left:
                transform.position -= transform.right * speed * Time.deltaTime;
                break;
        }
    }
}
