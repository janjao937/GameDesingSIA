using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomb : EnemyCharecter
{

    protected override void Awake()
    {
        base.Awake();
        Damage = gM.GetBombDamage;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMY_BOMB);
        if (collision.gameObject.tag == "Player") IsDie();
    }
    public override void IsDie()
    {
        //For implement Bomb
        base.IsDie();
    }
}
