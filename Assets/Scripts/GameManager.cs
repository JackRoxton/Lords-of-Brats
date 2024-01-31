using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public enum GameState { Pause, Game}
public enum Waves { Kids, Parents, Polices }
[System.Serializable]
public class Wave {
    public Waves wave;
    public int enemies;
}
public class GameManager : MonoBehaviour{
    public static GameManager Instance;
    public GameObject Player;
    public List<Wave> waves;
    public List<Spawner> Spawners = new List<Spawner>();
    public GameObject EnemyObject;
    public List<GameObject> Enemies = new List<GameObject>();
    public float ChainSpawnTime;
    [HideInInspector] public int thisWave = 0;
    float chainSpawnTimer = 0;
    public PlayableDirector ShovelAnimation;
    public GameObject ShovelforAnimation;
    public GameObject ShovelToPick;
    public PlayableDirector EndingAnimation;
    [HideInInspector] public GameState State;
    private void Awake() {
        Instance = this;
    }
    void Update(){
        if (State == GameState.Pause) return;
        if (thisWave < waves.Count && waves[thisWave].enemies > 0) {
            chainSpawnTimer += Time.deltaTime;
            if (chainSpawnTimer >= ChainSpawnTime) {
                chainSpawnTimer = 0;
                SpawnEnemy(Spawners[Random.Range(0, Spawners.Count)]);
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
        State = GameState.Pause;
        UIManager.Instance.SwitchActiveObject(UIManager.Instance.Pause);
        if (thisWave == 2) {
            Player.transform.position = Vector3.zero;
            PlayAnimation(1);
        }
        UIManager.Instance.PlayAnimation(thisWave + 1);
        thisWave++;    
    }   
    public void ChangeState(int state) {
        State = (GameState)state;
    }
    private IEnumerator PlayTimeline(int anim) {
        switch (anim) {
            case 0:
                ShovelAnimation.Play();
                yield return new WaitForSeconds((float)ShovelAnimation.duration);
                ShovelforAnimation.SetActive(false);
                ShovelToPick.SetActive(true);
                UIManager.Instance.PlayAnimation(0);
                yield break;
            case 1:
                EndingAnimation.Play();
                yield return new WaitForSeconds((float)EndingAnimation.duration);
                UIManager.Instance.SwitchActiveObject(UIManager.Instance.Ending);
                yield break;
        }

    }
    public void PlayAnimation(int anim) {
        StopAllCoroutines();
        StartCoroutine(PlayTimeline(anim));
    }
}
