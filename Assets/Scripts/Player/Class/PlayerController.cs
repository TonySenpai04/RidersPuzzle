using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public MovementController movementController;
    public IHitPoint hitPoint;
    public DataHero currentHero;
    private string currentHPState = "";
    [SerializeField] private float fadeDuration = 1.0f;
    private Coroutine fadeCoroutine;
    private void Awake()
    {
        instance = this;
        hitPoint = new HitPoint(10);
        movementController.immortal = (IImmortal)hitPoint;
        float screenWidth = Camera.main.orthographicSize * 2 * 9f / 16f;
        float cellSize = (float)(screenWidth - 0.1 * (6 - 1)) / 6;
        transform.localScale = new Vector3(cellSize, cellSize, 1);

    }
    private void Start()
    {


    }
    public void SetCurrentData(DataHero hero)
    {
        currentHero = hero;
    }
    public void LoadLevel()
    {
        hitPoint.SetHealth(currentHero.hp);
        hitPoint.Heal(currentHero.hp);
        GetComponent<SpriteRenderer>().sprite = currentHero.heroImage;
        movementController.LoadMove();
        SkillManager.instance.LoadSkillPVE();
    }
    private void Update()
    {
        if (!GameManager.instance.isEnd)
        {
            if (hitPoint.GetCurrentHealth() <= 0)
            {
                return;
            }
            else
            {
                movementController.Movement();
            }
        }
        CheckHPAndPlayMusic();

    }
    private void CheckHPAndPlayMusic()
    {
        float currentHealth = hitPoint.GetCurrentHealth();
        float maxHealth = hitPoint.GetMaxHealth();
        float healthPercentage = (currentHealth / maxHealth) * 100;

        string newHPState;

        if (healthPercentage < 30)
        {
            newHPState = "HP < 30%";
        }
        else if (healthPercentage >= 30 && healthPercentage <= 59)
        {
            newHPState = "HP 30-59%";
        }
        else
        {
            newHPState = "HP > 60%";
        }

        if (newHPState != currentHPState)
        {
            currentHPState = newHPState;
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            StartCoroutine(TransitionMusic(currentHPState));
            // SoundManager.instance.PlayMusic(currentHPState);
        }
    }
    private IEnumerator TransitionMusic(string newMusic)
    {
        while (SoundManager.instance.musicSource.volume > 0)
        {
            SoundManager.instance.musicSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        SoundManager.instance.PlayMusic(newMusic);

        while (SoundManager.instance.musicSource.volume < 0.5f)
        {
            SoundManager.instance.musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }


    }
}
