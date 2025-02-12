using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static HiddenObject;

[System.Serializable]
public class SpritePair
{
    public Sprite sprite1;
    public Sprite sprite2;
}

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private GridController gridController;
    [SerializeField] private List<Sprite> backgroundArts;
    [SerializeField] private List<SpritePair> gridArts ;
    private SpritePair selectedPair;
    [SerializeField] private Image libraryBg;
    [SerializeField] private Sprite heroBg;
    [SerializeField] private Sprite objectPowerUpBg;
    [SerializeField] private Sprite objObstacleBg;
    [SerializeField] private Sprite pandoraboxBg;
    [SerializeField] private Sprite defaultBg;
    public static BackgroundManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        UpdateRandomArt();
    }
    public void UpdateRandomArt()
    {
        UpdateBackgroundArt();
        UpdateGridArt();
        gridController.GetSprite();
    }
    public void SetDefaultBg()
    {
        libraryBg.sprite = defaultBg;
    }
    public void SetHeroBg()
    {
        libraryBg.sprite = heroBg;
    }
    public void SetObjectBg(string id)
    {
        HiddenObject hiddenObject = HiddenObjectManager.instance.GetById(id);
        if (hiddenObject != null)
        {
            switch (hiddenObject.type)
            {
                case ObjectType.PowerUp:
                    libraryBg.sprite = objectPowerUpBg;
                    break;
                case ObjectType.Obstacle:
                    libraryBg.sprite = objObstacleBg;
                    break;
                case ObjectType.Pandora:
                    libraryBg.sprite = pandoraboxBg;
                    break;
            }
           
        }
       
    }
    private void UpdateGridArt()
    {
        if (gridArts.Count > 0)
        {
            int randomIndex = Random.Range(0, gridArts.Count);
            selectedPair = gridArts[randomIndex];
            int index = 0; 
            foreach (var cell in gridController.grid)
            {
                var spriteRenderer = cell.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    if (index % 2 == 0)
                    {
                        spriteRenderer.sprite = selectedPair.sprite1; 
                    }
                    else
                    {
                        spriteRenderer.sprite = selectedPair.sprite2; 
                    }
                }
                index++;
            }
        }
        else
        {
            Debug.LogWarning("Danh sách map arts rỗng!");
        }
    }
    private void UpdateBackgroundArt()
    {
        if (backgroundArts.Count > 0)
        {
            int randomIndex = Random.Range(0, backgroundArts.Count);
            background.sprite = backgroundArts[randomIndex];
        }
        else
        {
            Debug.LogWarning("Danh sách background arts rỗng!");
        }
    }
}
