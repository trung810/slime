using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class waterFlow : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float knockBackPower;

    private Animator anim;
    private bool canDamage = true;
    private float horizontalInput;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "plr" && canDamage)
        {
            StartCoroutine(onHit());
            collision.GetComponent<Health>().TakeDamage(damage);
            var rb = collision.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(-horizontalInput * knockBackPower /3, knockBackPower);
        }
    }

    private IEnumerator onHit()
    {
        anim.SetBool("onHit", true);
        canDamage = false;
        yield return new WaitForSeconds(2);
        anim.SetBool("onHit", false);
        canDamage = true;
    }
}
