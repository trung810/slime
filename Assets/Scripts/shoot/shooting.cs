using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class shooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletspawnpoint;
    private Vector3 mousePos;
    private Camera mainCam;
    public bool canFire;
    private float timer;
    public float fireRate;
    private float angle;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position;

        angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        if(!canFire)
        {
            timer += Time.deltaTime;
            if(timer > fireRate)
            {
                canFire = true;
                timer = 0;
            }
        }

        HandleGunShooting();
    }

    private void HandleGunShooting()
    {
        if(Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            //Instantiate(bullet, bulletspawnpoint.position, Quaternion.Euler(0, 0, angle));

            GameObject bullet = ObjectPool.instance.GetPooledObject();
            if(bullet!= null)
            {
                bullet.transform.position = bulletspawnpoint.position;
                bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
                bullet.SetActive(true);
            }
        }
    }
}
