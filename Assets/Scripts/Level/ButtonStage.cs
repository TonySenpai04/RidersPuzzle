using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonStage : MonoBehaviour
{
    public int Index { get; private set; } 

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Initialize(int index, System.Action<int> onClickAction)
    {
        Index = index;
        button.onClick.AddListener(() => onClickAction(Index)); 
    }
}