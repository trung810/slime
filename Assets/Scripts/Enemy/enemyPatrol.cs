using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private float spd;
    [SerializeField] private float range;
    [SerializeField] private float idleCD;
    [SerializeField] private float idleTime;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask collideLayer;
    private Animator anim;
    private Vector3 initScale;
    private float idleTimer = 0;
    private int dir = 1;

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
        if(objectInSight())
        {
            dir = -dir;
        }
        if(idleTimer >= idleCD){
            anim.SetBool("walking", false);
            spd = 0;
            StartCoroutine(resetTimer());
        }
        else{
            spd = 20;
            anim.SetBool("walking", true);
            MoveInDirection(dir);
        }
    }
    private IEnumerator resetTimer()
    {
        yield return new WaitForSeconds(idleTime);
        idleTimer = 0;
    }

    private bool objectInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x* range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.right, 0, collideLayer);

        return hit.collider!=null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x* range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void MoveInDirection(int _direction)
    {        
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * Mathf.Sign(_direction), initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * spd, enemy.position.y, enemy.position.z);
    }
}
