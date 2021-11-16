using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyCharecter,IBossState
{
    enum StateBoss
    {
        NORMALSTATE,
        STATETWO,
    }

    [SerializeField] private EnemyCharecter minian;
    
    private int count = 0;
    private int minianAmount;
    private int hpForHeal;
    float numForChangeState;
    StateBoss stateBoss;

    protected override void Awake()
    {
        base.Awake();
        hpForHeal = gM.HpForBossHealing;
        minianAmount = gM.minianAmount;

        count = 0;
        //minian = gM.MinianOfBoss;
        minian.Init(20,3,10,0);
        stateBoss = StateBoss.NORMALSTATE;
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.BOSS_SPAWN);
    }

    protected override void Update()
    {
        base.Update();

        if(stateBoss == StateBoss.NORMALSTATE)
        {
            NormalState();
        }
        else if(stateBoss == StateBoss.STATETWO)
        {
            StateTwo();
        }

        if (Hp <= 0)
        {
            soundManager.PlayBGM(SoundManager.Sound.BGM_SCENEGAME);
        }
    }
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        count++;
        if(count >= 2 && collision.gameObject.tag==("Player"))
        {
            soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.Boss_Healing);
            Hp += hpForHeal;
            count = 0;
        }
    }
    public override void TakeDamage(int damage)
    {
        LowHp();
        base.TakeDamage(damage);
        
    }
    public override void IsDie()
    {
        base.IsDie();
    }
    protected void SpawnMinian()
    {

        while (minianAmount > 0)
        {

            var xPosition = Random.Range(gM.MinSpawnEnemyForRandomX, gM.MaxSpawnEnemyForRandomX);
            var zPosition = Random.Range(gM.MinSpawnEnemyForRandomZ, gM.MaxSpawnEnemyForRandomZ);

            soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.Boss_SPAWNMINIAN);

            Instantiate(minian, new Vector3(xPosition, 0, zPosition), Quaternion.identity);
            minianAmount--;
        }
    }

    

    public virtual void LowHp()
    {
        if (Hp <= (gM.GetBossHp / 2))
        {
            Debug.Log("Spawn");
            SpawnMinian();

        }
    }
    public virtual void NormalState()
    {
        numForChangeState += Time.deltaTime;
        if (numForChangeState <= 15) return;
        stateBoss = StateBoss.STATETWO;
    }

    public virtual void StateTwo()
    {
        numForChangeState -= Time.deltaTime;
        if (numForChangeState >= 0) return;
        stateBoss = StateBoss.NORMALSTATE;
    }

}
