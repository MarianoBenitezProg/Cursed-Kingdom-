using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("estoy en modo seek");
    }
    public void UpdateState(Enemy enemy)
    {
        enemy.Seek();
    }
    public void ExitState(Enemy enemy)
    {

    }
}
