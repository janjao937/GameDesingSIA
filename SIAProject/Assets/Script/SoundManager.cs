using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private SoundClip[] soundClips;
    [SerializeField] private AudioSource audioSource;
    public AudioSource audioSourceAction;

    public enum Sound
    {
        BGM,
        Playerdeath,
        Enemydeath,
        Playerfire,
        Enemyfire,
        Playerhit,
        Enemyhit,
    }

    public struct SoundClip
    {
        public Sound sound;
        public AudioClip audioClip;
        [Range(0, 1)] public float soundVolume;
    }

    public void Start()
    {
        Debug.Assert(soundClips != null && soundClips.Length != 0, "sound clips need cannot be null");
        Debug.Assert(audioSource != null, "audioSource cannot be null");
        Debug.Assert(audioSourceAction != null, "audioSourceAction cannot be null");
    }

    public void Play(AudioSource audioSource, Sound sound)
    {
        var soundClip = GetSoundClip(sound);
        audioSource.clip = soundClip.audioClip;
        audioSource.volume = soundClip.soundVolume;
        audioSource.Play();
    }

    public void PlayBGM()
    {
        audioSource.loop = true;
        Play(audioSource, Sound.BGM);
    }

    private SoundClip GetSoundClip(Sound sound)
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
