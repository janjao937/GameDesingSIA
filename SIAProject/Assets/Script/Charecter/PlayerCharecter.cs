using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PlayerCharecter : Charecter
{

    private Skill skill;
    private float timeCountSkill = 0;
    [SerializeField] private PlayerBullet bulletType;
    [SerializeField] private PoolingOBJ bullet;

    [SerializeField] private ParticalManager partical;
    


    private Animator animator;

    private Rigidbody rb;
    private bool CanUseSkill;
    public bool CanGetDamage = true;
    public GameObject CheckCollisionForSkill;
   
    public GameManager GM;
    //public void GetBullet(PoolingOBJ bullet)
    //{
    //    this.bullet = bullet;
    //}
    private SoundManager soundManager;
    #region GetPlayer Property

    public event Action playerDie; 

    #endregion
    public void Init(int hp, float speed, Skill skill)
    {
        base.Init(hp, speed);
        this.skill = skill;
    }
    private void Awake()    
    {
        GM = GameManager.Instance;
        partical = ParticalManager.Instance;

        bullet.GetBulletType(bulletType);

        animator = GetComponent<Animator>();

        CanUseSkill = true;
        soundManager = SoundManager.Instance;
    }

    protected override void Update()
    {
        base.Update();

        CoolDown();

        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkill();
        }
        
    }
    public void Attack()
    {
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PLAYER_ATK);
        bullet.GetPool();
        animator.SetTrigger("ATK");
        //Instantiate(bulletType, bullet.transform.position, Quaternion.identity);
    }
    public void ChangeBullet()
    {
        //For Change Bullet in pool
    }

    public void Healing(int hp)
    {
        Hp = hp;
        animator.SetBool("IsDie", false);
        CanGetDamage = true;
        print(Hp);
        partical.PlayParticle(ParticalManager.PlayerParticle.HEALING);
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.BUY_HEALING);
    }

   // ParticalManager.PlayerParticle a = ParticalManager.PlayerParticle.IMMORTAL;
    public void UseSkill()
    {
        if (!CanUseSkill) return;
        
        skill.AboutSkill(this);
        print("Skill");
        CanUseSkill = false;
        timeCountSkill = skill.CoolDownSkill;
        
    }

    private void CoolDown()
    {
       
        if (!CanUseSkill)
        {
            GM.playerSkillImg.fillAmount -= 1 / skill.CoolDownSkill * Time.deltaTime; //Control fill
            timeCountSkill -= Time.deltaTime;
            if (timeCountSkill <= 0)
            {
                GM.playerSkillImg.fillAmount = 0; 
                
                CanUseSkill = true;
            }
        }
        else GM.playerSkillImg.fillAmount = 1;


    }
    public override void TakeDamage(int damage)
    {
        if (!CanGetDamage) return;
        soundManager.Play(soundManager.AudioSorceForPlayerAction, SoundManager.Sound.PLAYER_TAKEDAMAGE);
        base.TakeDamage(damage);
    }

    public override void IsDie()
    {

        //Debug.Log("Die");
      
        animator.SetBool("IsDie",true);
        CanGetDamage = false;
        //GM.TimeSlow(0);

        playerDie();
    }

    public void GetSkill(Skill skill)
    {
        this.skill = skill;
        print(skill.Name);
    }
    
}
