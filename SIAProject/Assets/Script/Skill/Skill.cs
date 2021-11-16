using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [Header("Property")]
    public string Name;
    public Image SkillImage;
    public Image SkillButtonImg;

    [Header("About Skill")]
    public float CoolDownSkill = 0;
    public float timeOfSkill;
    public int SkillPrice;

    private ParticalManager particleManager;
    [SerializeField]private ParticalManager.PlayerParticle SkillParticle;

    private SoundManager soundManager;
    [SerializeField] private SoundManager.Sound sound;

    private bool isCoolDown = false;
    private void Awake()
    {
        particleManager = ParticalManager.Instance;
        soundManager = SoundManager.Instance;
        
    }
    //protected float timeCount = 0;
    // [SerializeField] protected ParticleSystem Particle;

  

    public virtual void AboutSkill(PlayerCharecter player)
    {
        UseParticle(SkillParticle);//Use Partical
        soundManager.Play(soundManager.AudioSorceForPlayerAction, sound); //ADD Sound
    }

    //Add particle to skill
    private void UseParticle(ParticalManager.PlayerParticle particle)
    {
        particleManager.PlayParticle(particle);
    }

    private void StopPartical(ParticalManager.PlayerParticle particle)
    {
        particleManager.StopPartical(particle);
    }

    protected virtual void EndSkill(PlayerCharecter player)
    {
        StopPartical(SkillParticle);
    }

    //protected virtual void CoolDown(float time)
    //{
    //    time = timeOfSkill;
    //    if (time >= 0) return;
    //    while(time<=0)
    //    {
    //        time -= Time.deltaTime;
    //    }
    //    return;
    //    //End Skill or Can Use Skill
    //}

   
}
