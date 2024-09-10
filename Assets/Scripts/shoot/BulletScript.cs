using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    [SerializeField] private float destroyTime;
    [SerializeField] private float dmg;

    public float force;
    private bool shooting = false;
    private Animator anim;
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(gameObject.activeInHierarchy && !shooting)
        {
            StartCoroutine(shoot());
            shooting = true;
        }
    }

    private IEnumerator shoot()
    {   
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;

        rb.velocity = new Vector2(dir.x, dir.y).normalized * force/3;

        yield return new WaitForSeconds(.3f);

        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;

        anim.SetTrigger("loop");

        //Invoke("DisableBullet", destroyTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy")
        {
            anim.SetTrigger("onHit");
            rb.velocity = Vector2.zero;

            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<Health>().TakeDamage(dmg);
            }
            //Destroy(gameObject, 1f);
            Invoke("DisableBullet", 1f);
        }
    }

    private void DisableBullet()
    {
        gameObject.SetActive(false);
        shooting = false;
    }
}
