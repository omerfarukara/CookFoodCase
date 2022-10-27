using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Timer : MonoSingleton<Timer>
{
    [SerializeField] float time;
    [SerializeField] float extraTime;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject timer;

    float currentTime;
    
    EventData _eventData;


    public float ChestTime
    {
        get { return time; }
        set { time = value; }
    }

    public float ExtraTime
    {
        get { return extraTime; }
        set { extraTime = value; }
    }

    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
        Singleton();
    }

    void Start()
    {
        StartCoroutine(TimeCoroutine());
    }

    public void ChestReward()
    {
        timer.SetActive(false);
        timerText.transform.parent.gameObject.SetActive(true);
        StartCoroutine(TimeCoroutine());
    }

    public IEnumerator TimeCoroutine()
    {
        currentTime = time;
        timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60));
        while (currentTime > 0)
        {
            currentTime -= 1;
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60));
            yield return new WaitForSeconds(1);
            if (currentTime == 0)
            {
                _eventData.OnLose?.Invoke();
            }
        }
        yield return null;
    }
}
