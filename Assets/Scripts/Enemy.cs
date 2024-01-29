using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public enum EnemyType { Kid, Parent, Police }
public class Enemy : MonoBehaviour {
    public EnemyType Type;
    public float Speed = 0.0f;
    Vector3 direction;
    void Start(){
        
    }
    void Update(){
        direction = new Vector3(0,0,0) - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime);
    }
}
