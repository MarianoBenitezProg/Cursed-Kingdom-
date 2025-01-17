using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Proyectile
{
    protected GameObject shootingPivot;
    protected P_Behaviour playerRef;
    protected Direction shootingDir;
    [SerializeField]protected bool isFerana;

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
        transform.position += transform.up * speed * Time.deltaTime;
        switch(shootingDir)
        {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(0,0,180f);
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(0,0,-90f);
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(0,0,90f);
                break;
        }
    }
}
