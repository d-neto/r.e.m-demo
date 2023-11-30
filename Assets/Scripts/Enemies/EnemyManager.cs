using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private List<Enemy> enemies = new List<Enemy>();

    private void Awake(){
        if(Instance == null)
            Instance = this;
        else Destroy(this);
    }

    public void RegisterEnemy(Enemy enemy){
        if (!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy){
        if (enemies.Contains(enemy)) enemies.Remove(enemy);
    }

    public List<AbleToAim> GetEnemiesInRadius(Vector2 position, float searchRadius){
        List<AbleToAim> enemiesInRadius = new List<AbleToAim>();
        if(enemies.Count == 0) return enemiesInRadius;
        for(int i = 0; i < enemies.Count; i++){
            float distance = Vector2.Distance(position, enemies[i].transform.position);
            if (distance <= searchRadius) enemiesInRadius.Add(enemies[i].GetAbleToAim());
        }
        return enemiesInRadius;
    }
}
