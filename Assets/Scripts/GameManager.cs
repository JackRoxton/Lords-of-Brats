using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum SpawnMode { ChainSpawn, PrecisSpawn }
public enum Waves { Kids, Parents, Polices }
[System.Serializable]
public class Wave {
    public Waves wave;
    public int enemies;
}
public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    public SpawnMode spawnMode;
    public List<Wave> waves;
    public List<Spawner> Spawners = new List<Spawner>();
    public GameObject EnemyObject;
    public List<GameObject> Enemies = new List<GameObject>();
    public float ChainSpawnTime;
    [HideInInspector] public int thisWave = 0;
    float chainSpawnTimer = 0;
    public PlayableDirector ShovelAnimation;
    public GameObject ShovelforAnimation;
    List<float> timers = new List<float>();
    private void Awake() {
        Instance = this;
        for (int i = 0; i < Spawners.Count; i++) timers.Add(0);
    }
    void Update(){
        if (thisWave < waves.Count && waves[thisWave].enemies > 0 && !UIManager.Instance.DialogBox.activeSelf) {
            switch (spawnMode) {
                case SpawnMode.ChainSpawn:
                    chainSpawnTimer += Time.deltaTime;
                    if (chainSpawnTimer >= ChainSpawnTime) {
                        chainSpawnTimer = 0;
                        SpawnEnemy(Spawners[Random.Range(0, Spawners.Count)]);
                    }
                    break;
                case SpawnMode.PrecisSpawn:
                    for (int i = 0; i < timers.Count; i++) {
                        timers[i] += Time.deltaTime;
                        if (timers[i] > Spawners[i].SpawnTime) {
                            timers[i] = 0;
                            SpawnEnemy(Spawners[i]);
                        }
                    }
                    break;
            }
        }
    }
    void SpawnEnemy(Spawner spawner) {
        Enemies.Add(Instantiate(EnemyObject, spawner.transform.position, Quaternion.identity));
        Enemies[Enemies.Count - 1].GetComponent<Enemy>().Type = waves[thisWave].wave;
        Enemies[Enemies.Count - 1].GetComponent<Enemy>().Speed = 2;
        waves[thisWave].enemies--;
    }
    public void NextWave() {
        switch (thisWave) {
            case 0:
                UIManager.Instance.LaunchDialogue("FinVague1");
                break;
            case 1:
                UIManager.Instance.LaunchDialogue("FinVague2");
                break;
            case 2:
                UIManager.Instance.LaunchDialogue("FinVague3");
                break;
        }
        thisWave++;
        
    }
    private IEnumerator PlayTimeline() {
        ShovelAnimation.Play();
        yield return new WaitForSeconds((float)ShovelAnimation.duration);
        ShovelforAnimation.SetActive(false);
        UIManager.Instance.LaunchDialogue("Intro2");
    }
    public void PlayAnimation() {
        StopAllCoroutines();
        StartCoroutine(PlayTimeline());
    }
}
