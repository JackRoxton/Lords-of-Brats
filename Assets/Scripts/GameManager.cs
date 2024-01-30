using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpawnMode { ChainSpawn, PrecisSpawn }
public enum Waves { Kids, Parents, Polices }
public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    public SpawnMode spawnMode;
    public Waves wave;
    public List<Spawner> Spawners = new List<Spawner>();
    public GameObject EnemyObject;
    public float ChainSpawnTime;
    float chainSpawnTimer = 0;
    List<float> timers = new List<float>();
    private void Awake() {
        Instance = this;
        for (int i = 0; i < Spawners.Count; i++) timers.Add(0);
    }
    void Update(){
        switch (spawnMode) {
            case SpawnMode.ChainSpawn:
                chainSpawnTimer += Time.deltaTime;
                if (chainSpawnTimer >= ChainSpawnTime) {
                    chainSpawnTimer = 0;
                    SpawnEnemy(Random.Range(0, Spawners.Count));
                }
                break;
            case SpawnMode.PrecisSpawn:
                for (int i = 0; i < timers.Count; i++) {
                    timers[i] += Time.deltaTime;
                    if (timers[i] > Spawners[i].SpawnTime) {
                        timers[i] = 0;
                        SpawnEnemy(i);
                    }
                }
                break;
        }
    }
    void SpawnEnemy(int index) {
        GameObject settings = Instantiate(EnemyObject, Spawners[index].transform.position, Quaternion.identity);
        settings.GetComponent<Enemy>().Type = (EnemyType)Random.Range(0, 3);
        settings.GetComponent<Enemy>().Speed = 2;
    }
}
