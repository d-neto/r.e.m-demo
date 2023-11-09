using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<AbleAim> enemies = new List<AbleAim>();

    private void Awake(){
        if(Instance == null)
            Instance = this;
        else Destroy(this);
    }

    public void RegisterEnemy(AbleAim enemy){
        if (!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void UnregisterEnemy(AbleAim enemy){
        if (enemies.Contains(enemy)) enemies.Remove(enemy);
    }

    public List<AbleAim> GetEnemiesInRadius(Vector2 position, float searchRadius){
        List<AbleAim> enemiesInRadius = new List<AbleAim>();
        foreach (AbleAim enemy in enemies)
        {
            float distance = Vector2.Distance(position, enemy.transform.position);
            if (distance <= searchRadius) enemiesInRadius.Add(enemy);
        }
        return enemiesInRadius;
    }
}
