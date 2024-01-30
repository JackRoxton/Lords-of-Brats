using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour {
    public Waves Type;
    public float Speed = 0.0f;
    public List<GameObject> Loot;
    public List<Sprite> KidsSprites = new List<Sprite>();
    public List<Sprite> KidsHurtSprites = new List<Sprite>();
    public List<Sprite> AdultsSprites = new List<Sprite>();
    public List<Sprite> AdultsHurtSprites = new List<Sprite>();
    public List<Sprite> PolicesSprites = new List<Sprite>();
    public List<Sprite> PolicesHurtSprites = new List<Sprite>();
    public int Hp = 1;
    public bool Stop = false;
    Vector3 direction;
    int refSprite;
    void Start(){
        switch (Type) {
            case Waves.Kids:
                refSprite = Random.Range(0,KidsSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = KidsSprites[refSprite];
                Hp = 2;
                break;
            case Waves.Parents:
                refSprite = Random.Range(0, AdultsSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = AdultsSprites[refSprite];
                Hp = 2;
                break;
            case Waves.Polices:
                refSprite = Random.Range(0, PolicesSprites.Count);
                gameObject.GetComponent<SpriteRenderer>().sprite = PolicesSprites[refSprite];
                Hp = 3;
                break;
        }
        gameObject.AddComponent<PolygonCollider2D>();
    }
    void Update(){
        this.GetComponent<Rigidbody2D>().velocity *= 0.99f;
        if (Stop)
        {
            return;
        }
        //direction = new Vector3(0,0,0) - transform.position;
        //transform.Translate(direction.normalized * Speed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(this.transform.position,Vector2.zero,Speed * Time.deltaTime);
    }

    IEnumerator StopDuration()
    {
        Stop = true;
        yield return new WaitForSeconds(1f);
        Stop = false;
    }

    public void GetHit(int dmg, Vector2 kb) {

        StopAllCoroutines();//

        StartCoroutine(StopDuration());
        this.GetComponent<Rigidbody2D>().AddForce(kb);
        Debug.Log(kb);
        Hp -= dmg;
        if(Hp < 0)
        {
            GameManager.Instance.Enemies.Remove(gameObject);
            if (GameManager.Instance.waves[GameManager.Instance.thisWave].enemies <= 0 &&
                                GameManager.Instance.Enemies.Count <= 0) GameManager.Instance.NextWave();

            if(Random.Range(0,100) >= 90)
            {
                switch (Type)
                {
                    case Waves.Kids:
                        Instantiate(Loot[Random.Range(0,2)],this.transform.position,Quaternion.identity);
                        break;
                    case Waves.Parents:
                        Instantiate(Loot[2], this.transform.position, Quaternion.identity);
                        break;
                    case Waves.Polices:
                        Instantiate(Loot[3], this.transform.position, Quaternion.identity);
                        break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
