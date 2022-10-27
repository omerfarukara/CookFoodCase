using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Data/Event Data")]
public class EventData : ScriptableObject
{
    public Action OnPlay;
    public Action OnFinish;
    public Action OnWin;
    public Action OnLose;
    public Action OnRestart;

    public Action<Vector3> OnSetBorderPosition;
    public Action<FoodStuff> OnSelectFoodStuff;
    public Action<FoodStuff> OnDoubleTapToFoodStuff;
    public Action OnSelectFoodStuffChanged;
}