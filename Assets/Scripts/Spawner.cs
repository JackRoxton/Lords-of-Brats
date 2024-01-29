using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour{
    public List<GameObject> Enemies = new List<GameObject>();
    public float SpawnTime;
    float timer = 0;
    void Update(){
        timer += Time.deltaTime;
        if(timer >= SpawnTime) {
            timer = 0;
            Instantiate(Enemies[0], transform.position, Quaternion.identity);
        }
    }
}
