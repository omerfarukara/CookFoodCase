using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI levelText;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject settingsPanel, storePanel, mainMenuPanel;

    [SerializeField] GameObject soundOnButton, soundOffButton;
    [SerializeField] GameObject musicOnSettings, musicOffSettings;
    [SerializeField] GameObject hapticOnSettings, hapticOffSettings;

    void Start()
    {
        if (GameManager.Instance.EndlessLevel % 3 == 0)
        {
            ChestOpen();
        }
        SetLevelText();
    }

    private void SetLevelText()
    {
        levelText.text = GameManager.Instance.EndlessLevel.ToString();
    }

    public void ChestOpen()
    {
        chest.transform.localScale = Vector3.zero;
        chest.SetActive(true);
        chest.transform.DOScale(Vector3.one, 1).SetEase(Ease.OutElastic);
    }

    public void ChestReward()
    {
        chest.transform.DOScale(Vector3.zero, 1).SetEase(Ease.OutElastic).OnComplete(() => chest.SetActive(true));
    }

    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
        storePanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void StorePanel()
    {
        storePanel.SetActive(true);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
    }

    public void BackButton()
    {
        mainMenuPanel.SetActive(true);
        storePanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void MusicOnSettings()
    {
        musicOnSettings.SetActive(false);
        musicOffSettings.SetActive(true);
    }
    public void MusicOffSettings()
    {
        musicOnSettings.SetActive(true);
        musicOffSettings.SetActive(false);
    }
    public void SoundOnSettings()
    {
        soundOnButton.SetActive(false);
        soundOffButton.SetActive(true);
    }
    public void SoundOffSettings()
    {
        soundOnButton.SetActive(true);
        soundOffButton.SetActive(false);
    }
    public void HapticOnSettings()
    {
        Handheld.Vibrate();
        hapticOffSettings.SetActive(true);
        hapticOnSettings.SetActive(false);
    }
    public void HapticOffSettings()
    {
        hapticOnSettings.SetActive(true);
        hapticOffSettings.SetActive(false);
    }
}
