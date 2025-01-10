using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundAssigner : MonoBehaviour
{
    public List<Button> butons;
    private void Start()
    {
        foreach (var button in butons)
        {
            button.onClick.AddListener(() => SoundManager.instance.PlaySFX("Click Sound"));
        }
    }
}
