using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour {
    public Waves Type;
    public float Speed = 0.0f;
    public List<Sprite> KidsSprites = new List<Sprite>();
    public List<Sprite> KidsHurtSprites = new List<Sprite>();
    public List<Sprite> AdultsSprites = new List<Sprite>();
    public List<Sprite> AdultsHurtSprites = new List<Sprite>();
    public List<Sprite> PolicesSprites = new List<Sprite>();
    public List<Sprite> PolicesHurtSprites = new List<Sprite>();
    public int Hp = 1;
    Vector3 direction;
    int refSprite;
    void Start(){
        switch (Type) {
            case Waves.Kids:
                refSprite = Random.Range(0,KidsSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = KidsSprites[refSprite];
                break;
            case Waves.Parents:
                refSprite = Random.Range(0, AdultsSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = AdultsSprites[refSprite];
                break;
            case Waves.Polices:
                refSprite = Random.Range(0, PolicesSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = PolicesSprites[refSprite];
                break;
        }
        gameObject.AddComponent<PolygonCollider2D>();
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
