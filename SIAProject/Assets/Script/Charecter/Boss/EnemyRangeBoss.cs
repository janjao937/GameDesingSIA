using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeBoss : Boss
{
    private float fireRate;
    private float countFireRate = 0;

    [SerializeField] GameObject firePosition;
    private EnemyBulletPooling enemyBulletPooling;
    private float oldSpeed;
    [SerializeField] private float rotateSpeed;
 
    protected override void Awake()
    {
        base.Awake();
        enemyBulletPooling = EnemyBulletPooling.Instance;
        oldSpeed = Speed;
        fireRate = gM.EnemyFireRate;
    }
    protected override void Update()
    {
        base.Update();

        if (countFireRate >= fireRate)
        {
            UseBullet();
            countFireRate = 0;
        }
        countFireRate += Time.deltaTime;
    }
    private void UseBullet()
    {
        enemyBulletPooling.GetBullet(firePosition, gM.getBossBulletDamage);
    }

    public override void NormalState()
    {
        
        Speed = 0;
        transform.Rotate(0, 1 * rotateSpeed * Time.deltaTime, 0);
        SpawnMinian();
        base.NormalState();
    }
    public override void StateTwo()
    {
       
        Speed = oldSpeed;
        base.StateTwo();
    }
}
