using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private float spd;
    [SerializeField] private float idleCD;
    [SerializeField] private float idleTime;
    [SerializeField] private float range1;
    [SerializeField] private float colliderDistance1;
    [SerializeField] private float range2;
    [SerializeField] private float colliderDistance2;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask collideLayer;
    private Animator anim;
    private Vector3 initScale;
    private float idleTimer = 0;
    private int dir = 1;
    private float local_spd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("walking", false);
    }

    private void Update()
    {
        idleTimer += Time.deltaTime;
        if(!objectbelow() || objectInSight())
        {
            dir = -dir;
        }
        if(idleTimer >= idleCD){
            anim.SetBool("walking", false);
            local_spd = 0;
            StartCoroutine(resetTimer());
        }
        else{
            local_spd = spd;
            anim.SetBool("walking", true);
            MoveInDirection(dir);
        }
    }

    private IEnumerator resetTimer()
    {
        yield return new WaitForSeconds(idleTime);
        idleTimer = 0;
    }

    private bool objectbelow()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range1 * Mathf.Sign(transform.localScale.x) * colliderDistance1 * 5 - transform.up * colliderDistance1,
        new Vector3(boxCollider.bounds.size.x* range1, boxCollider.bounds.size.y*range1, boxCollider.bounds.size.z), 0, Vector2.right, 0, collideLayer);

        return hit.collider!=null;
    }

    private bool objectInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range2 * Mathf.Sign(transform.localScale.x) * colliderDistance2,
        new Vector3(boxCollider.bounds.size.x* range2, boxCollider.bounds.size.y/2, boxCollider.bounds.size.z), 0, Vector2.right, 0, collideLayer);

        return hit.collider!=null;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range1 * Mathf.Sign(transform.localScale.x) * colliderDistance1 * 5 - transform.up * colliderDistance1,
        new Vector3(boxCollider.bounds.size.x* range1, boxCollider.bounds.size.y*range1, boxCollider.bounds.size.z));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range2 * Mathf.Sign(transform.localScale.x) * colliderDistance2,
        new Vector3(boxCollider.bounds.size.x* range2, boxCollider.bounds.size.y/2, boxCollider.bounds.size.z));
    }

    private void MoveInDirection(int _direction)
    {        
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * Mathf.Sign(_direction), initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * local_spd, enemy.position.y, enemy.position.z);
    }
}
