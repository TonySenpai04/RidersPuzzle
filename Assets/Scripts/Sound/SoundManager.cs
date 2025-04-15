using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private string soundDataPath => Path.Combine(Application.persistentDataPath , "SoundData.json");
    [SerializeField] private List<Sound> sounds;
    [SerializeField] private List<HeroSound> heroSounds;
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource sFXSource;
    private bool isHeroSFXPlaying = false;
    public static SoundManager instance;
    private bool isMute;
    private void Awake()
    {
        instance = this;
        LoadData();
        musicSource.mute = isMute;
        sFXSource.mute = isMute;
    }
    public void SaveData()
    {
        var data=new SoundData { isMute = this.isMute };
        string json=JsonUtility.ToJson(data,true);
        File.WriteAllText(soundDataPath, json);
    }
    public void LoadData()
    {
        if (File.Exists(soundDataPath))
        {
            string json = File.ReadAllText(soundDataPath);
            var data=JsonUtility.FromJson<SoundData>(json);
            this.isMute = data.isMute;

        }
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
            musicSource.volume = 0.3f;
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
        if (isHeroSFXPlaying) return;
        HeroSound sound = GetHeroSound(id);
        if (sound != null && sound.clip != null && sound.clip.Count > 0)
        {
            AudioClip selectedClip = sound.clip[UnityEngine.Random.Range(0, sound.clip.Count)];
            sFXSource.PlayOneShot(selectedClip);
           isHeroSFXPlaying = true;
            // Gọi hàm chờ đến khi clip phát xong
            instance.StartCoroutine(ResetHeroSFXFlag(selectedClip.length));
        }
        else
        {
            Debug.LogWarning($"Âm thanh {id} không tìm thấy hoặc clip chưa được gán!");
        }
        //if (sound != null && sound.clip != null)
        //{
        //    sFXSource.PlayOneShot(sound.clip[UnityEngine.Random.Range(0,sound.clip.Count)]);
        //}
        //else
        //{
        //    Debug.LogWarning($"Âm thanh {name} không tìm thấy hoặc clip chưa được gán!");
        //}
    }
    private IEnumerator ResetHeroSFXFlag(float delay)
    {
        yield return new WaitForSeconds(delay);
        isHeroSFXPlaying = false;
    }

    public void SetAudio(bool isOn)
    {
        musicSource.mute = !isOn;
        sFXSource.mute = !isOn;
        this.isMute = !isOn;
        SaveData();
    }

    public bool IsAudioOn()
    {
        return isMute;
    }
}
