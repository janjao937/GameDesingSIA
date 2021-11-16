using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource AudioSorceForPlayerAction;
    public AudioSource AudioSorceForEnemyAction;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SoundClip[] soundClips;


    public enum Sound
    {
      BGM_MAINMANU,
      BGM_SCENEGAME,
      BGM_SPAWNBOSS,
      BGM_DIE,//No loop

      SCREENSHOT,
      PUSH_BUTTON,
      PLAYER_TAKEDAMAGE,
      PLAYER_ATK,
      BUY_HEALING,
      BUY_DAMAGE,

      SKILL_SPEED,
      SKILL_IMMORTAL,
      SKILL_STUNT,
      SKILL_SLOWENEMY,
      SKILL_ADDDAMAGE,

      ENEMYRANGE_ROLL,//15second 
      ENEMY_FIRE,
      ENEMY_BOMB,
      ENEMY_BOOSTSPEED,
      ENEMY_SPAWN,
      ENEMY_DIE,
      BOSS_SPAWN,
      Boss_SPAWNMINIAN,
      Boss_Healing,


    }
      

    


  
    [Serializable]
    public struct SoundClip
    {
        public Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)] public float soundVolume;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PlayBGM(Sound.BGM_MAINMANU);
    }



    public void Play(AudioSource audioSource, Sound sound)
    {

        var soundClip = GetAudioClip(sound);
        audioSource.clip = soundClip.audioClip;
        audioSource.volume = soundClip.soundVolume;
        audioSource.Play();
        audioSource.loop = false;

    }

    public void PlayBGM(Sound bgm)
    {
        audioSource.loop = true;
        Play(audioSource, bgm);
    }

    private SoundClip GetAudioClip(Sound sound)
    {
        foreach (var soundClip in soundClips)
        {
            if (soundClip.sound == sound)
            {
                return soundClip;
            }
        }

        return default(SoundClip);
    }

}
