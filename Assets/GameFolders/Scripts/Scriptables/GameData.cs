using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private float doubleTapTime;

    public float DoubleTapTime => doubleTapTime;
}
