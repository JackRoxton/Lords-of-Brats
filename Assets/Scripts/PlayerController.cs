using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 Movement;
    float speed = 0.5f;
    float throwStrength = 0.5f;
    Vector2 MousePos;

    GameObject SavedWeapon;

    public GameObject Arm;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Attack();
            Debug.Log("Attack");
        }

        if (Input.GetMouseButtonDown(1))
        {
            Throw();
            Debug.Log("Throw");
        }
    }

    private void FixedUpdate()
    {
        Movement.x = Input.GetAxis("Horizontal");
        Movement.y = Input.GetAxis("Vertical");

        Movement *= 0.9f;
        this.transform.position += Movement * speed;

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(MousePos);
    }

    void Attack()
    {
        //
    }

    void Throw()
    {
        if (SavedWeapon == null) return;
        GameObject go;
        go = Instantiate(SavedWeapon, Arm.transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = new Vector2(MousePos.x - this.transform.position.x,MousePos.y - this.transform.position.y);
        SavedWeapon = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pickable")
        {
            Debug.Log(collision.gameObject.name);
            SavedWeapon = collision.gameObject;
            SavedWeapon.tag = "Weapon";
        }
    }

}
