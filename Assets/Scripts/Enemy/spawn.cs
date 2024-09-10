using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyType;
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float spawnCD;
    private GameObject enemy;
    private bool canSpawn = true;
    private Animator anim;

    private void Update()
    {
        if(enemy == null && canSpawn)
        {
            canSpawn = false;
            StartCoroutine(spawnEnemy());
        }
    }

    private IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(spawnCD);
        enemy = Instantiate(enemyType, spawnpoint.position, Quaternion.Euler(0, 0, 0));
        canSpawn = true;
        anim = enemy.GetComponent<Animator>();
        anim.SetTrigger("spawn");
    }
}
