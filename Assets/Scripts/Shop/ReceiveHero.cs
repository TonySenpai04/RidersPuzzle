using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ReceiveHero : MonoBehaviour
{
    [SerializeField] private Image heroImage;
    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private int receivedIdHero;
    public void SetData(int id)
    {
        this.receivedIdHero = id;
        var receivedHero = HeroManager.instance.GetHero(receivedIdHero);
        receivedIdHero = receivedHero.Value.id;
        heroImage.sprite = receivedHero.Value.heroImage;
        heroName.text = (receivedHero.Value.id+ " "+receivedHero.Value.name).ToUpper();
        StartCoroutine( ShowRewardTemporarily());
    }
    private IEnumerator ShowRewardTemporarily()
    {
        // rewardObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
