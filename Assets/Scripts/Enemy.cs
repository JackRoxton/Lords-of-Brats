using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public enum EnemyType { Kid, Parent, Police }
public class Enemy : MonoBehaviour {
    public EnemyType Type;
    public float Speed = 0.0f;
    public List<Sprite> Sprites = new List<Sprite>();
    public int Hp = 1;
    Vector3 direction;
    int randomNumber;
    void Start(){
        switch (Type) {
            case EnemyType.Kid:
                randomNumber = 0;
                break;
            case EnemyType.Parent:
                randomNumber = 1;
                break;
            case EnemyType.Police:
                randomNumber = 2;
                break;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprites[randomNumber];
    }
    void Update(){
        direction = new Vector3(0,0,0) - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime);
    }
    public void GetHit(int dmg) {
        Hp -= dmg;
        if(Hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
