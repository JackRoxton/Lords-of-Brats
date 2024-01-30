using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [NonSerialized] public bool hitFlag = false;
    public int damage = 1;
    [NonSerialized] public bool isThrown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if(enemy != null && hitFlag)
        {
            //hit
            //knockback
            hitFlag = false;
            if(isThrown) Destroy(this.gameObject);
        }
    }
}
