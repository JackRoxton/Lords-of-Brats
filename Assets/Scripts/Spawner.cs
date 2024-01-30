using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Spawner : MonoBehaviour {
    public float SpawnTime;
    public void Update() {
        if(Input.GetKeyDown(KeyCode.I)) UIManager.Instance.LaunchDialogue("Intro");
        if(Input.GetKeyDown(KeyCode.F)) UIManager.Instance.LaunchDialogue("Fin");
    }
}
