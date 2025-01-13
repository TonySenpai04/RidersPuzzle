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
[Serializable]
public class HeroSound
{
    public int id;
    public List<AudioClip> clip;
}
public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<Sound> sounds;
    [SerializeField] private List<HeroSound> heroSounds;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sFXSource;
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    private Sound GetSound(string name)
    {
        return sounds.Find(sound => sound.name == name);
    }
    private HeroSound GetHeroSound(int id)
    {
        return heroSounds.Find(sound => sound.id == id);
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
            musicSource.volume = 0.5f;
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
    public void PlayHeroSFX(int id)
    {
        HeroSound sound = GetHeroSound(id);
        if (sound != null && sound.clip != null)
        {
            sFXSource.PlayOneShot(sound.clip[UnityEngine.Random.Range(0,sound.clip.Count)]);
        }
        else
        {
            Debug.LogWarning($"Âm thanh {name} không tìm thấy hoặc clip chưa được gán!");
        }
    }

    public void SetAudio(bool isOn)
    {
        musicSource.mute = !isOn;
        sFXSource.mute = !isOn;

        //PlayerPrefs.SetInt("AudioMuted", isOn ? 0 : 1); // 0 = bật, 1 = tắt
        //PlayerPrefs.Save();
    }

    public bool IsAudioOn()
    {
        return PlayerPrefs.GetInt("AudioMuted", 0) == 0; // Mặc định là bật âm thanh
    }
}
