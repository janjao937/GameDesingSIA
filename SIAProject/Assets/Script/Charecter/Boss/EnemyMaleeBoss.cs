using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaleeBoss : Boss
{
    float normalSpeed;

    protected override void Awake()
    {
        base.Awake();
        normalSpeed = Speed;
      
    }
    public override void LowHp()
    {
        base.LowHp();
       
    }
    public override void NormalState()
    {
        soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMY_BOOSTSPEED);
        Speed = 20;
        base.NormalState();
    }
    public override void StateTwo()
    {
        Speed = normalSpeed;
        base.StateTwo();
        
    }

}
