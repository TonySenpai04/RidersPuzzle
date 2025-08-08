using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonSpriteGrouper : MonoBehaviour
{
    [System.Serializable]
    public class SpriteGroup
    {
        public Sprite sprite;
        [HideInInspector] public Sprite lastSprite; // Lưu sprite cũ để so sánh
        public List<Button> buttons = new List<Button>();
    }

    public List<SpriteGroup> groups = new List<SpriteGroup>();

#if UNITY_EDITOR
    [ContextMenu("Find and Group Buttons")]
    public void FindAndGroupButtons()
    {
        groups.Clear();

        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (var btn in allButtons)
        {
            Image img = btn.GetComponent<Image>();
            if (img != null && img.sprite != null)
            {
                SpriteGroup group = groups.FirstOrDefault(g => g.sprite == img.sprite);
                if (group == null)
                {
                    group = new SpriteGroup { sprite = img.sprite, lastSprite = img.sprite };
                    groups.Add(group);
                }
                group.buttons.Add(btn);
            }
        }

        Debug.Log($"🔍 Đã nhóm {groups.Count} sprite khác nhau từ Button.");
    }

    private void OnValidate()
    {
        // Chỉ chạy khi đang trong Editor, không play
        if (!Application.isPlaying)
        {
            foreach (var group in groups)
            {
                // Nếu sprite mới khác sprite cũ → đổi hết button
                if (group.sprite != group.lastSprite && group.sprite != null)
                {
                    foreach (var btn in group.buttons)
                    {
                        Image img = btn.GetComponent<Image>();
                        if (img != null)
                        {
                            img.sprite = group.sprite;
                            EditorUtility.SetDirty(img); // Đánh dấu thay đổi
                        }
                    }

                    group.lastSprite = group.sprite;
                    Debug.Log($"✅ Đã đổi sprite cho {group.buttons.Count} button.");
                }
            }
        }
    }
#endif
}
