using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    [SerializeField] private float destroyTime;
    public float force;
    private Animator anim;
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(shoot());
    }

    private IEnumerator shoot()
    {   
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousePos - transform.position;

        rb.velocity = new Vector2(dir.x, dir.y).normalized * force/3;

        yield return new WaitForSeconds(.3f);

        rb.velocity = new Vector2(dir.x, dir.y).normalized * force;

        anim.SetBool("loop", true);

        Destroy(gameObject, destroyTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Wall")
        {
            anim.SetBool("onHit", true);
            rb.velocity = Vector2.zero;
            Destroy(gameObject, .5f);
        }
    }
}
