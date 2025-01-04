using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    int currentPathIndex ;
    public void EnterState(Enemy enemy)
    {
        Debug.Log("estoy en modo patrol");
        currentPathIndex = 0;
    }
    public void UpdateState(Enemy enemy)
    {
        if (enemy.path.Count > 0)
        {
            Transform target = enemy.path[currentPathIndex];
            Vector3 direction = target.position - enemy.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.position, enemy.Speed * Time.deltaTime);
            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, enemy.RotationSpeed * Time.deltaTime);

            if (Vector3.Distance(enemy.transform.position, target.position) < 0.1f)
            {
                currentPathIndex++;
            }
        }
        if (currentPathIndex == enemy.path.Count)
        {
            currentPathIndex = 0;
        }

    }
    public void ExitState(Enemy enemy)
    {

    }
}
