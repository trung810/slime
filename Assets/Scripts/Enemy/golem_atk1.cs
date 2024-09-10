using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class golemAI : MonoBehaviour
{
    [SerializeField] private float atkcd;
    [SerializeField] private float dmg;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private float knockBackPower;

    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask plrLayer;


    private float cdTimer = Mathf.Infinity;
    private Animator anim;
    private Transform plr;
    private Health plrHealth;
    private Rigidbody2D rb;
    private float horizontalInput;
    private enemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<enemyPatrol>();
    }

    private void Update()
    {
        cdTimer += Time.deltaTime;

        horizontalInput = Input.GetAxis("Horizontal");

        if(plrInSight())
        {
            if(cdTimer >= atkcd)
            {
                cdTimer = 0;
                anim.SetTrigger("atk");
                Invoke("DamagePlr", .5f);
            }
        }

        if(enemyPatrol != null)
        {
            if(plrInSight())
            {
                enemyPatrol.enabled = false;
                StartCoroutine(movementEnable());
            }
        }
    }
    private IEnumerator movementEnable()
    {
        yield return new WaitForSeconds(1f);
        enemyPatrol.enabled = true;
    }
    private bool plrInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x* range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.right, 0, plrLayer);

        if(hit.collider != null)
        {
            plr = hit.transform;
            plrHealth = plr.GetComponent<Health>();
            rb = plr.GetComponent<Rigidbody2D>();
        }

        return hit.collider!=null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x* range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlr()
    {
        if(plrInSight())
        {
            plrHealth.TakeDamage(dmg);
            rb.velocity = new Vector2(-Mathf.Sign(plr.localScale.x) * knockBackPower, knockBackPower*2);
        }
    }
}
