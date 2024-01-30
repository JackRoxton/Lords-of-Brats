using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Sprite Sprite;
    public Sprite pickeUpSprite;
    [NonSerialized] public bool hitFlag = false;
    public int dmg = 1;
    public float strength = 5f;
    [NonSerialized] public bool isThrown = false;

    public void ChangeSprite()
    {
        this.GetComponent<SpriteRenderer>().sprite = pickeUpSprite;
    }

    public void Throw()
    {
        this.GetComponent<SpriteRenderer>().sprite = Sprite;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!hitFlag) return;
        if (collision == null) return;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.GetHit(dmg);
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.transform.position.x-enemy.transform.position.x,this.transform.position.y-enemy.transform.position.y).normalized * strength);
            if(isThrown) Destroy(this.gameObject);
        }
    }
}
