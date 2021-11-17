using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeBoss : Boss
{
    private float fireRate;
    private float countFireRate = 0;
    private float oldFireRate;

    [SerializeField] GameObject firePosition;
    private EnemyBulletPooling enemyBulletPooling;
    private float oldSpeed;

    protected override void Awake()
    {
        base.Awake();
        enemyBulletPooling = EnemyBulletPooling.Instance;
        oldSpeed = Speed;

    }
    protected override void Update()
    {
        base.Update();
        fireRate = gM.EnemyFireRate;

        if (countFireRate >= fireRate)
        {
            UseBullet();

            countFireRate = 0;
        }
        countFireRate += Time.deltaTime;
    }
    private void UseBullet()
    {
        enemyBulletPooling.GetBullet(firePosition, Damage);
        soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMY_FIRE);
    }

    public override void NormalState()
    {
        soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMYRANGE_ROLL);
        Speed = 0;
        transform.Rotate(0, 1 * gM.EnemyRangeRotateSpeed * Time.deltaTime, 0);
        SpawnMinian();
        base.NormalState();
    }
    public override void StateTwo()
    {
        Speed = oldSpeed;
        base.StateTwo();
    }
    public override void OnStunt()
    {
        base.OnStunt();
        gM.EnemyRangeRotateSpeed = 0;
        oldFireRate = fireRate;
        fireRate = 0;
    }
    public override void ExitStunt(float speed)
    {
        base.ExitStunt(speed);
        gM.EnemyRangeRotateSpeed = 500;
        fireRate = oldFireRate;
    }
   
}
