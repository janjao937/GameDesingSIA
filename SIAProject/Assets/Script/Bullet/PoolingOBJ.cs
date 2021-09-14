using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingOBJ : MonoBehaviour
{
     private PlayerBullet bullet;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private PlayerBullet[] bulletInPool;
    [SerializeField] private int poolAmount = 0;
    

    private int ControlPool = 0;
    float timeCountForFire = 0.5f;//Player => GM => Pool (If we want to manage fireRate in plyer)
    float fireRate = 0;

    void Start()
    {
        bulletInPool = new PlayerBullet[poolAmount];
        Pooling();
    }
    private void Pooling()
    {
        for (var i = 0; i < poolAmount; i++)
        {
            bulletInPool[i] = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInPool[i].gameObject.SetActive(false);
        }
    }
    public void GetPool()
    {
        if (ControlPool == poolAmount)
        {
            ControlPool = 0;
        }

        if (Time.time < fireRate) return;

        bulletInPool[ControlPool].transform.position = transform.position;
        bulletInPool[ControlPool].gameObject.SetActive(true);
        bulletInPool[ControlPool].rb.velocity = transform.forward * bulletInPool[ControlPool].GetSpeed(bulletSpeed);
        fireRate = Time.time + timeCountForFire;
        ControlPool++;
       
    }

    public void GetBulletType(PlayerBullet bulletType)
    {
        bullet = bulletType;
    }

}