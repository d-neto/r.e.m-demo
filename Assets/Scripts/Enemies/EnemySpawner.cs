using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> enemies;
    public List<Transform> spawns;

    void Start(){
        StartCoroutine(InfiniteSpawn());
    }

    IEnumerator InfiniteSpawn(){
        while(true){
            RandomSpawn();
            yield return new WaitForSeconds(1f);
        }
    }

    void RandomSpawn(){
        GameObject clone = Instantiate(enemies[Random.Range(0, enemies.Count)], spawns[Random.Range(0, spawns.Count)].position, Quaternion.identity);
    }

}
