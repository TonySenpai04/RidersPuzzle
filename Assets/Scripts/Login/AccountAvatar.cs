using UnityEngine;
using UnityEngine.UI;

public class AccountAvatar:MonoBehaviour
{
    public int index;
    public Image selectImg;
    public Image avt;
    public Image checkSelect;
    public Button button;

    public void SetData(int index,Sprite avt)
    {
        this.index = index;
        this.avt.sprite = avt;

    }

    public void UnSelect()
    {
        selectImg.enabled = false;
        checkSelect.enabled = false;
    }
    public void Select()
    {
        selectImg.enabled = true;
        checkSelect.enabled = true;
    }


}