using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
public class Anais : MonoBehaviour
{
    public float Speed = 0.0f;
    public Sprite Sprite;
    public Sprite HurtSprite;
    public Sprite EvilSprite;
    public int Hp = 5;
    public bool Stop = false;
    public bool hitFlag = true;
    Vector3 direction;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite;
        gameObject.AddComponent<PolygonCollider2D>();
    }
    void Update()
    {
        if (GameManager.Instance.State == GameState.Game)
        {
            this.GetComponent<Rigidbody2D>().velocity *= 0.98f;
            if (Stop)
            {
                return;
            }
            /*direction = new Vector3(0,0,0) - transform.position;
            transform.Translate(direction.normalized * Speed * Time.deltaTime);*/
            transform.position = Vector2.MoveTowards(this.transform.position, GameManager.Instance.Player.transform.position, Speed * Time.deltaTime);
        }
    }

    IEnumerator StopDuration()
    {
        hitFlag = false;
        this.GetComponent<Animator>().enabled = false;
        Stop = true;

        this.GetComponent<SpriteRenderer>().sprite = HurtSprite;

        yield return new WaitForSeconds(0.1f);
        hitFlag = true;
        yield return new WaitForSeconds(0.9f);
        this.GetComponent<Animator>().enabled = true;
        Stop = false;
        this.GetComponent<SpriteRenderer>().sprite = Sprite;
    }

    public void SetEvilSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = EvilSprite;
    }

    public void GetHit(int dmg, Vector2 kb)
    {

        if (!hitFlag) return;
        StopAllCoroutines();

        SoundManager.Instance.Play("Punch");

        Camera.main.GetComponent<CameraScript>().ScreenShake();
        Camera.main.GetComponent<CameraScript>().HitStop();

        StartCoroutine(StopDuration());
        this.GetComponent<Rigidbody2D>().AddForce(kb);
        //Debug.Log(kb);
        Hp -= dmg;
        if (Hp < 0)
        {
            GameManager.Instance.Enemies.Remove(gameObject);
            GameManager.Instance.NextWave();

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
