using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;

    public float currHealth {get; private set; }
    private Animator anim;
    private Rigidbody2D rb;

    private bool dead = false;
    private void Awake()
    {
        currHealth = startingHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float _damage)
    {
        currHealth = Mathf.Clamp(currHealth - _damage, 0, startingHealth);
        
        if(currHealth > 0)
        {
            anim.SetTrigger("hurt");
            //iframe
        }
        else
        {
            if(!dead)
            {
                anim.SetTrigger("die");
                
                if(GetComponent<plrMovement>() != null)
                {
                    GetComponent<plrMovement>().enabled = false;
                }
                
                //enemy
                if(gameObject.tag == "Enemy")
                {
                    Invoke("die", 1f);
                }
                dead = true;
            }
        }
    }

    private void die()
    {
        Destroy(gameObject);
    }
}
