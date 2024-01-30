using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour {
    public Waves Type;
    public float Speed = 0.0f;
    public List<Sprite> Sprites = new List<Sprite>();
    public int Hp = 1;
    Vector3 direction;
    int randomNumber;
    void Start(){
        switch (Type) {
            case Waves.Kids:
                randomNumber = 0;
                break;
            case Waves.Parents:
                randomNumber = 1;
                break;
            case Waves.Polices:
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
            GameManager.Instance.Enemies.Remove(gameObject);
            if (GameManager.Instance.waves[GameManager.Instance.thisWave].enemies <= 0 &&
                                GameManager.Instance.Enemies.Count <= 0) GameManager.Instance.NextWave();
            Destroy(this.gameObject);
        }
    }
}
