using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLibraryController : MonoBehaviour
{
    [SerializeField] private ObjectLibrary ObjectLibraryPrefabs;
    [SerializeField] private Transform content;
    [SerializeField] private ObjectView obejctView;
    [SerializeField] private GameObject objectParent;
    [SerializeField] private List<ObjectLibrary> objectLibraries;
    [SerializeField] private TextMeshProUGUI objectSeenTxt;

    public List<ObjectLibrary> HeroLibraries { get => objectLibraries; set => objectLibraries = value; }

    void Start()
    {
        int count = HiddenObjectManager.instance.ObjectQuantity();
        for (int i = 0; i < count; i++)
        {
            ObjectLibrary objectLib = Instantiate(ObjectLibraryPrefabs, content.transform);
            objectLib.SetObject(HiddenObjectManager.instance.AllObjects[i].id, 
                HiddenObjectManager.instance.AllObjects[i].sprite);
            objectLib.GetComponent<Button>().onClick.AddListener(() => SetObjectView(objectLib.Id));



        }

        objectSeenTxt.text = "Seen:"+HiddenObjectManager.instance.ObjectQuantity() ;
    }
    private void OnEnable()
    {
        
    }
    public void SetObjectView(string id)
    {

        HiddenObject hiddenObject = HiddenObjectManager.instance.GetById(id);
        obejctView.SetObject(id, hiddenObject.sprite,hiddenObject.name);
        this.obejctView.gameObject.SetActive(true);
        objectParent.gameObject.SetActive(false);
        BackgroundManager.instance.SetObjectBg(id);
    }
}
