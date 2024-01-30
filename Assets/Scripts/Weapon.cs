using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [NonSerialized] public bool hitFlag = false;
    public int dmg = 1;
    [NonSerialized] public bool isThrown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null && hitFlag)
        {
            enemy.GetHit(dmg);
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.transform.position.x-enemy.transform.position.x,this.transform.position.y-enemy.transform.position.y));
            hitFlag = false;
            if(isThrown) Destroy(this.gameObject);
        }
    }
}
