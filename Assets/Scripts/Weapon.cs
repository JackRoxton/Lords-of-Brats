using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    GameObject Owner;
    public Sprite Sprite;
    public Sprite pickeUpSprite;
    [NonSerialized] public bool hitFlag = false;
    public int dmg = 1;
    float strength = 1000f;
    [NonSerialized] public bool isThrown = false;

    public void ChangeSprite(GameObject newOwner)
    {
        Owner = newOwner;
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
            Vector2 kb = new Vector2(enemy.transform.position.x - Owner.transform.position.x, enemy.transform.position.y - Owner.transform.position.y).normalized;
            enemy.GetHit(dmg, kb * strength);
            if(isThrown) Destroy(this.gameObject);
        }
    }
}
