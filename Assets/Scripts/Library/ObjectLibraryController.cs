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

    public List<ObjectLibrary> ObjectLibraries { get => objectLibraries; set => objectLibraries = value; }

    void Start()
    {
        int count = HiddenObjectManager.instance.ObjectQuantity();
        for (int i = 0; i < count; i++)
        {
            ObjectLibrary objectLib = Instantiate(ObjectLibraryPrefabs, content.transform);
            objectLib.SetObject(HiddenObjectManager.instance.AllObjects[i].id, 
                HiddenObjectManager.instance.AllObjects[i].sprite);
            var hiddenObject = HiddenObjectManager.instance.GetById(objectLib.Id);
            if (hiddenObject.isSeen)
            {
                objectLib.GetComponent<Button>().onClick.AddListener(() => SetObjectView(objectLib.Id));
            }
            objectLibraries.Add(objectLib);

        }

        objectSeenTxt.text = "Seen:"+HiddenObjectManager.instance.GetSeenObject().Count ;
        UpdateSeenOject();
    }
    private void OnEnable()
    {
        UpdateSeenOject();
    }
  
    public void UpdateSeenOject()
    {
        foreach (var obj in objectLibraries)
        {
            var hiddenObject = HiddenObjectManager.instance.GetById(obj.Id);
            if (hiddenObject.isSeen)
            {
                obj.SetSeenObject(true);
                obj.UpdateVisibility();
                obj.GetComponent<Button>().onClick.AddListener(() => SetObjectView(obj.Id));

            }
            else
            {
                obj.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
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
