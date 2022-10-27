using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIController : MonoSingleton<UIController>
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] Image material1, material2, material3;
    [SerializeField] TextMeshProUGUI foodName;
    [SerializeField] TextMeshProUGUI levelText;

    [Header("Gold")]
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Transform goldParent;
    [SerializeField] GameObject goldAnimation;
    [SerializeField] float timeBetween;
    [SerializeField] float animationSpeed;

    [Header("NoThanks")]
    [SerializeField] GameObject extraTimeBackgroundPanel;
    [SerializeField] GameObject noThanksPanel;
    [SerializeField] GameObject cryEmoji;


    [SerializeField] Motion levelFailedImageMotion, extraTimeMotion, noThanksMotion;
    [SerializeField] Motion levelWinImageMotion;

    private int gold;

    EventData _eventData;

    public GameObject LosePanel => losePanel;

    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
        Singleton();
    }

    private void OnEnable()
    {
        _eventData.OnWin += Win;
        _eventData.OnLose += Lose;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            mainMenu.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(false);
        }

        gold = GameManager.Instance.Gold;
        goldText.text = gold.ToString();
        UpgradeLevelText(GameManager.Instance.EndlessLevel);
    }

    private void OnDisable()
    {
        _eventData.OnWin -= Win;
        _eventData.OnLose -= Lose;
    }

    private void Win()
    {
        StartCoroutine(WinPanelCoroutine());
    }

    public void UpgradeLevelText(int levelCount)
    {
        levelText.text = $"Level {levelCount}";
    }

    public void SetFoodMaterials(Sprite image1, Sprite image2, Sprite image3, String food)
    {
        material1.sprite = image1;
        material2.sprite = image2;
        material3.sprite = image3;
        foodName.text = food;
    }

    public void UpdateGoldText()
    {
        StartCoroutine(UpdateGoldTextCoroutine());
    }
    
    public void ResumeGame()
    {
        losePanel.SetActive(false);
        Timer.Instance.ChestTime = Timer.Instance.ExtraTime;
        GameManager.Instance.Gold -= 5;
        goldText.text = GameManager.Instance.Gold.ToString();

        StartCoroutine(Timer.Instance.TimeCoroutine());
    }

    public void NoThanks()
    {
        extraTimeBackgroundPanel.SetActive(false);
        noThanksPanel.SetActive(false);
        cryEmoji.transform.localScale = Vector3.zero;
        cryEmoji.SetActive(true);
        cryEmoji.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).OnComplete(() => _eventData.OnRestart?.Invoke());
    }

    public void Lose()
    {
        StartCoroutine(LosePanelCoroutine());
    }

    IEnumerator WinPanelCoroutine()
    {
        winPanel.SetActive(true);
        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            winPanel.GetComponent<Image>().fillAmount = t;
            yield return null;
        }
        levelWinImageMotion.DoMotionIn();
        yield return new WaitForSeconds(0.5f);
        _eventData.OnFinish?.Invoke();
    }

    IEnumerator LosePanelCoroutine()
    {
        losePanel.SetActive(true);
        levelFailedImageMotion.Transform.localScale = Vector3.zero;
        extraTimeMotion.Transform.localScale = Vector3.zero;
        noThanksMotion.Transform.localScale = Vector3.zero;

        float t = 0;
        while (t <= 1)
        {
            t += Time.deltaTime;
            losePanel.GetComponent<Image>().fillAmount = t;
            yield return null;
        }
        levelFailedImageMotion.DoMotionIn();
        yield return new WaitForSeconds(0.5f);
        extraTimeMotion.DoMotionIn();
        yield return new WaitForSeconds(0.5f);
        noThanksMotion.DoMotionIn();
    }

    IEnumerator UpdateGoldTextCoroutine()
    {
        int residual = GameManager.Instance.Gold - gold;
        for (int i = 1; i <= residual; i++)
        {
            GameObject newGold = Instantiate(goldAnimation, goldParent);
            newGold.GetComponent<Animator>().speed = animationSpeed;
            gold++;
            goldText.text = $"{gold}";
            Destroy(newGold, (1 / animationSpeed) + 0.05f);
            yield return new WaitForSeconds(timeBetween);
        }
    }

}
