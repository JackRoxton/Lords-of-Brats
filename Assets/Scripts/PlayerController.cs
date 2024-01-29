using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 Movement;
    float speed = 10f;
    float throwStrength = 10f;
    int hp = 3;
    Vector2 MousePos;

    GameObject SavedWeapon;

    public GameObject Arm;
    public GameObject PickupCollision;
    bool pickingUp;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Pickup());
        }

        if (SavedWeapon != null)
        {
            SavedWeapon.transform.position = Arm.transform.position;
        }

    }

    private void FixedUpdate()
    {
        Movement.x = Input.GetAxis("Horizontal");
        Movement.y = Input.GetAxis("Vertical");

        Movement *= 0.9f;
        this.transform.position += Movement * speed * Time.deltaTime;

        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(MousePos);
    }

    public void GetHit()
    {
        hp -= 1;
        if(hp <= 0)
        {
            Debug.Log("ono am ded");
        }
    }

    void Attack()
    {
        Arm.GetComponent<Animator>().Play("WeaponAttack");
    }

    void Throw()
    {
        if (SavedWeapon == null) return;
        SavedWeapon.GetComponent<Rigidbody2D>().velocity = new Vector2(MousePos.x - this.transform.position.x,MousePos.y - this.transform.position.y).normalized * throwStrength;
        SavedWeapon = null;
    }

    IEnumerator Pickup()
    {
        PickupCollision.SetActive(true);
        pickingUp = true;
        yield return new WaitForSeconds(1);
        PickupCollision.SetActive(false);
        pickingUp = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.tag == "Pickable" && pickingUp)
        {
            SavedWeapon = collision.gameObject;
            SavedWeapon.tag = "Weapon";
        }
    }

}
