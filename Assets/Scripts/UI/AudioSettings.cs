using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Toggle toggleOn;  // Toggle để bật âm thanh
    [SerializeField] private Toggle toggleOff; // Toggle để tắt âm thanh

    private void Start()
    {
        bool isMuted = SoundManager.instance.musicSource.mute;
        toggleOn.isOn = !isMuted;
        toggleOff.isOn = isMuted;

        toggleOn.onValueChanged.AddListener(OnToggleOnChanged);
        toggleOff.onValueChanged.AddListener(OnToggleOffChanged);
    }

    private void OnToggleOnChanged(bool isOn)
    {
        if (isOn)
        {
            toggleOff.isOn = false; 
            SoundManager.instance.SetAudio(true); 
        }
    }

    private void OnToggleOffChanged(bool isOn)
    {
        if (isOn)
        {
            toggleOn.isOn = false; 
            SoundManager.instance.SetAudio(false); 
        }
    }
}
