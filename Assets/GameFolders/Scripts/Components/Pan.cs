using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Pan : MonoSingleton<Pan>
{
    [SerializeField] Transform foodBackPosition;

    private EventData _eventData;
    private FoodStuff activeFoodStuff;
    private FoodData _foodData;

    private List<FoodStuff> foodStuffs = new List<FoodStuff>();
    private List<FoodStuffMaterial> activeFoodStuffMaterials = new List<FoodStuffMaterial>();

    private int activeFoodIndex;
    private int maxFoodAmount;

    bool _clickPlayability;

    public bool ClickPlayability
    {
        get { return _clickPlayability; }
        set { _clickPlayability = value; }
    }

    private void Awake()
    {
        _eventData = Resources.Load("EventData") as EventData;
        _foodData = Resources.Load("FoodData") as FoodData;
    }

    private void Start()
    {
        ClickPlayability = true;

        Singleton();
        UpdateFoodMaterials();
        maxFoodAmount = _foodData.GetFoods().Count;
        SetNextFoodStuffMaterials();
    }

    private void OnEnable()
    {
        _eventData.OnDoubleTapToFoodStuff += SetFoodStuff;
        _eventData.OnSelectFoodStuffChanged += RemoveFoodStuff;
    }

    private void OnMouseDown()
    {
        if (ClickPlayability)
        {
            AddFoodStuff();
        }
    }

    private void OnDisable()
    {
        _eventData.OnDoubleTapToFoodStuff -= SetFoodStuff;
        _eventData.OnSelectFoodStuffChanged -= RemoveFoodStuff;
    }

    private void SetFoodStuff(FoodStuff foodStuff)
    {
        activeFoodStuff = foodStuff;
    }

    private void RemoveFoodStuff()
    {
        activeFoodStuff = null;
    }

    private void AddFoodStuff()
    {
        if (activeFoodStuff)
        {
            FoodStuffMaterial tempMaterial = null;

            foreach (FoodStuffMaterial foodStuffMaterial in activeFoodStuffMaterials)
            {
                if (activeFoodStuff.FoodStuffType == foodStuffMaterial.FoodStuffType)
                {
                    tempMaterial = foodStuffMaterial;

                }
            }

            if (tempMaterial != null)
            {
                activeFoodStuff.transform.DOScale(0.4f, 1f);
                ClickPlayability = false;
                activeFoodStuff.transform.DOMove(new Vector3(transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), 4, transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f)), 1f).OnComplete(() =>
                {
                    activeFoodStuff.FreezeConstraints(RigidbodyConstraints.FreezeAll);
                    Invoke("ResetActiveFoodStuff", 1f);
                    MatchProcess(tempMaterial);
                });
            }
            else 
            {
                activeFoodStuff.transform.DOScale(0.4f, 1f);
                ClickPlayability = false;
                activeFoodStuff.transform.DOMove(new Vector3(transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), 4, transform.position.z + UnityEngine.Random.Range(-0.5f, 0.5f)), 1f).OnComplete(() =>
                {
                    activeFoodStuff.MoveBackFoodArea(FoodStuffsCreator.Instance.transform);
                    activeFoodStuff.FreezeConstraints(RigidbodyConstraints.None);
                });
            }
        }
    }

    private void MatchProcess(FoodStuffMaterial tempMaterial)
    {
        activeFoodStuffMaterials.Remove(tempMaterial);
        foodStuffs.Add(activeFoodStuff);

        if (activeFoodStuffMaterials.Count == 0)
        {
            GameManager.Instance.Gold += 10;
            activeFoodIndex++;

            AddFoodStuff();

            if (activeFoodIndex == maxFoodAmount)
            {
                _eventData.OnWin?.Invoke();
            }
            else
            {
                SetNextFoodStuffMaterials();
                UpdateFoodMaterials();
            }
            ClearFoods();
        }
    }

    private void UpdateFoodMaterials()
    {
        UIController.Instance.SetFoodMaterials(
            _foodData.LevelFoods[_foodData.GetLevel()].Foods[activeFoodIndex].FoodStuffsMaterials[0].Sprite,
            _foodData.LevelFoods[_foodData.GetLevel()].Foods[activeFoodIndex].FoodStuffsMaterials[1].Sprite,
            _foodData.LevelFoods[_foodData.GetLevel()].Foods[activeFoodIndex].FoodStuffsMaterials[2].Sprite,
            _foodData.LevelFoods[_foodData.GetLevel()].Foods[activeFoodIndex].Name);
    }

    private void ResetActiveFoodStuff()
    {
        activeFoodStuff = null;
        ClickPlayability = true;
    }

    private void SetNextFoodStuffMaterials()
    {
        activeFoodStuffMaterials.Clear();
        activeFoodStuffMaterials.AddRange(_foodData.GetFoods()[activeFoodIndex].FoodStuffsMaterials);
    }

    private void ClearFoods()
    {
        foreach (FoodStuff foodStuff in foodStuffs)
        {
            foodStuff.Destroy();
        }

        foodStuffs.Clear();
    }
}
