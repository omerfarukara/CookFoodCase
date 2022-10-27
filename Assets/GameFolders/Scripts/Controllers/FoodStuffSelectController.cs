using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStuffSelectController : MonoBehaviour
{
    private EventData _eventData;
    private GameData _gameData;
    private FoodStuff selectedFoodStuff;

    private float doubleTapTime;
    private float currentTime;

    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
        _gameData = Resources.Load("GameData") as GameData;
    }

    private void Start()
    {
        doubleTapTime = _gameData.DoubleTapTime;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void OnEnable()
    {
        _eventData.OnSelectFoodStuff += SetFoodStuff;
    }

    private void OnDisable()
    {
        _eventData.OnSelectFoodStuff -= SetFoodStuff;
    }

    private void SetFoodStuff(FoodStuff foodStuff)
    {
        if (foodStuff == selectedFoodStuff && currentTime <= doubleTapTime)
        {
            _eventData.OnDoubleTapToFoodStuff?.Invoke(selectedFoodStuff);
            selectedFoodStuff.ScaleUp();
        }
        else
        {
            if (selectedFoodStuff)
            {
                selectedFoodStuff.ScaleDown();
            }

            selectedFoodStuff = foodStuff;
            
            _eventData.OnSelectFoodStuffChanged?.Invoke();

            currentTime = 0;
        }
    }
}