using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharecter : Charecter
{
    public int Damage;
    protected GameManager gM;
    [SerializeField] private float knockBack;
    [SerializeField] private int scoreInFirstRound = 1;
    [SerializeField] protected ParticleSystem dieParticle;

    protected SoundManager soundManager;

    protected virtual void Awake()
    {
        gM = GameManager.Instance;
        gM.OnSlow += SlowEnemyAndExit;
        knockBack = gM.KnockBackForce;
        soundManager = SoundManager.Instance;
}

    public void Init(int hp, float speed,int damage,int scoreEnemyInRound)
    {
        base.Init(hp, speed);
        Damage = damage;
        scoreInFirstRound = scoreEnemyInRound;
    }
    public virtual void OnStunt()
    {
        Speed = 0;
  
    }
    public virtual void ExitStunt(float speed)
    {
        Speed = speed;
       
    }

    protected void SlowEnemyAndExit(float speed)
    {
        Speed = speed;
        //Slow and Exit Slow Animation
        //Slow and Exitr Slow Atk
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") return;
        //if (collision.gameObject.tag == "Player")
        //{
            
            Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
            Vector3 direction = collision.transform.position - transform.position;
            direction.y = 0;
            rb.AddForce(direction.normalized * knockBack, ForceMode.Impulse);

            //collision.transform.Translate(direction*knockBack*Time.deltaTime);
            //rb.transform.Translate(direction);

            var player = collision.gameObject.GetComponent<ITakeDamage>();
            player?.TakeDamage(Damage);

        //}
     
    }

    public override void IsDie()
    {
        Instantiate(dieParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        ScoreManager.Instance.AddScore(scoreInFirstRound);
        soundManager.Play(soundManager.AudioSorceForEnemyAction, SoundManager.Sound.ENEMY_DIE);

    }


    //var playerDie = collision.gameObject.GetComponent<PlayerCharecter>();
    //playerDie.KnockBack(direction);

    ////MockUp

    //{
    //    Speed = 0;
    //}
}
