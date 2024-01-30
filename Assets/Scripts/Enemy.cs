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
    public bool hitFlag = true;
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
        this.GetComponent<Rigidbody2D>().velocity *= 0.98f;
        if (Stop)
        {
            return;
        }
        /*direction = new Vector3(0,0,0) - transform.position;
        transform.Translate(direction.normalized * Speed * Time.deltaTime);*/
        transform.position = Vector2.MoveTowards(this.transform.position,Vector2.zero/*player here*/,Speed * Time.deltaTime);
    }

    IEnumerator StopDuration()
    {
        hitFlag = false;
        this.GetComponent<Animator>().enabled = false;
        Stop = true;
        switch (Type)
        {
            case Waves.Kids:
                this.GetComponent<SpriteRenderer>().sprite = KidsHurtSprites[refSprite];
                break; 
            case Waves.Parents:
                this.GetComponent<SpriteRenderer>().sprite = AdultsHurtSprites[refSprite];
                break; 
            case Waves.Polices:
                this.GetComponent<SpriteRenderer>().sprite = PolicesHurtSprites[refSprite];
                break;
        }
        yield return new WaitForSeconds(0.1f);
        hitFlag = true;
        yield return new WaitForSeconds(0.9f);
        this.GetComponent<Animator>().enabled = true;
        Stop = false;
        switch (Type)
        {
            case Waves.Kids:
                this.GetComponent<SpriteRenderer>().sprite = KidsSprites[refSprite];
                break;
            case Waves.Parents:
                this.GetComponent<SpriteRenderer>().sprite = AdultsSprites[refSprite];
                break;
            case Waves.Polices:
                this.GetComponent<SpriteRenderer>().sprite = PolicesSprites[refSprite];
                break;
        }
    }

    public void GetHit(int dmg, Vector2 kb) {

        if(!hitFlag) return;
        StopAllCoroutines();

        Camera.main.GetComponent<CameraScript>().ScreenShake();
        Camera.main.GetComponent<CameraScript>().HitStop();

        StartCoroutine(StopDuration());
        this.GetComponent<Rigidbody2D>().AddForce(kb);
        //Debug.Log(kb);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;
        if (Stop) return;

        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.GetHit(1, new Vector2(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y).normalized * 10f);
        }
    }
}
