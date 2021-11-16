using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyCharecter
{
    //[SerializeField] Bullet enemyBullet;
    private float fireRate;
    private float countFireRate = 0;

    [SerializeField] GameObject firePosition;
    private EnemyBulletPooling enemyBulletPooling;

    private float oldFireRate;

    protected override void Awake()
    {
        base.Awake();
        enemyBulletPooling = EnemyBulletPooling.Instance;
        
        fireRate = gM.EnemyFireRate;
    }
    protected override void Update()
    {
        base.Update();

        if (countFireRate>=fireRate)
        {
            UseBullet();
            soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMY_FIRE);
            countFireRate = 0;
        }
        countFireRate += Time.deltaTime;
    }
   
    private void UseBullet()
    {
        //Check RayCast if foundPlayer => fire SS
        enemyBulletPooling.GetBullet(firePosition, gM.EnemyDamage);
    }

    public override void OnStunt()
    {
        base.OnStunt();
        oldFireRate = fireRate;
        fireRate = 0;
    }
    public override void ExitStunt(float speed)
    {
        base.ExitStunt(speed);
        fireRate = oldFireRate;
    }
}
