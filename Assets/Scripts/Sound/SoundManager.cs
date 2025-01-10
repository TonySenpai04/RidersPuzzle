using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sFXSource;
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    private Sound GetSound(string name)
    {
        return sounds.Find(sound => sound.name == name);
    }

    public void PlaySFX(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null && sound.clip != null)
        {
            sFXSource.PlayOneShot(sound.clip);
        }
        else
        {
            Debug.LogWarning($"Âm thanh {name} không tìm thấy hoặc clip chưa được gán!");
        }
    }

    public void PlayMusic(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null && sound.clip != null)
        {
            musicSource.clip = sound.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Nhạc nền {name} không tìm thấy hoặc clip chưa được gán!");
        }
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
    public void StopSFX()
    {
        if (sFXSource.isPlaying)
        {
            sFXSource.Stop();
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp01(volume);
    }
    public void SetSFXVolume(float volume)
    {
        sFXSource.volume = Mathf.Clamp01(volume);
    }
}
