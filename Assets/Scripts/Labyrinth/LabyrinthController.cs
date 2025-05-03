
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthController : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private int totalLevels;
    [SerializeField] private GameObject playZone;
    [SerializeField] private GameObject stageZone;
    [SerializeField] private GameObject transitionLevel;
    [SerializeField] private DataHero gataiData;
    [SerializeField] private int levelIndex;
    public static LabyrinthController instance;
    [SerializeField] private List<MazeEffect> mazeEffects;

    private void Awake()
    {
        instance = this;
      
    }
    public void SetGataiData(DataHero gataiData)
    {
        this.gataiData = gataiData;
    }
    public void Randomlevel()
    {
        levelIndex = levelManager.RandomLevelIndex();
        StartLevel(levelIndex);
    }
    private void StartLevel(int levelIndex)
    {
        playZone.gameObject.SetActive(true);
        transitionLevel.gameObject.SetActive(true);
        levelManager.StartCoroutine(HideAfterDelay(1f));
        levelManager.SetLevel(levelIndex);
        PlayerController.instance.SetCurrentData(gataiData);
        GameManager.instance.LoadLevel();
        levelManager.ClearObject();
        FirstPlayManager.instance.FirstPlay(() =>
        {
            levelManager.LoadObject(levelManager.GetLevel(levelIndex - 1));
        });
        Level level = levelManager.GetLevel(levelIndex - 1);

        foreach (var levelData in level.hiddenObjects)
        {
            HiddenObjectManager.instance.SetSeenObjectById(levelData.objectPrefab.GetComponent<HiddenObject>().id);
        }

        stageZone.gameObject.SetActive(false);

    }
    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transitionLevel.gameObject.SetActive(false);
    }
}
