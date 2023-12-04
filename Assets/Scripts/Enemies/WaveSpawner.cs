using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public int wave = 0;
    public List<GameObject> AllEnemies;
    public List<Transform> spawns;
    public bool isSpawning;
    void Update(){
        if(EnemyManager.Instance.TotalEnemiesAlive() <= 0 && !isSpawning){
            wave++;
            StartCoroutine(WaveSpawn());
        }
    }

    IEnumerator WaveSpawn(){
        isSpawning = true;
        for(int k = 0; k < wave; k++){
            for(int i = 0; i < spawns.Count; i++)
                RandomSpawnOn(spawns[i].position, wave, (wave*0.005f));
            yield return new WaitForSeconds(1.5f);
        }
        isSpawning = false;
        yield return null;
    }

    void RandomSpawn(){
        GameObject clone = Instantiate(AllEnemies[Random.Range(0, AllEnemies.Count)], spawns[Random.Range(0, spawns.Count)].position, Quaternion.identity);
    }
    void RandomSpawnOn(Vector3 position){
        GameObject clone = Instantiate(AllEnemies[Random.Range(0, AllEnemies.Count)], position, Quaternion.identity);
    }
    void RandomSpawnOn(Vector3 position, float addLife, float addDamage){
        GameObject clone = Instantiate(AllEnemies[Random.Range(0, AllEnemies.Count)], position, Quaternion.identity);
        Enemy enemy = clone.GetComponent<Enemy>();
        enemy.GetData().life += addLife;
        enemy.GetData().damage += addDamage;
        if(wave%5 == 0) enemy.GetData().speed += 0.5f;
    }
}
