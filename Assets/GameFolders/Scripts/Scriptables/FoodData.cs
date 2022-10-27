using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "FoodData", menuName = "Data/FoodData")]
public class FoodData : ScriptableObject
{
    [SerializeField] private List<LevelFoods> levelFoods;

    public List<LevelFoods> LevelFoods => levelFoods;

    public List<Food> GetFoods()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;
        return levelFoods[levelIndex - 1].Foods;
    }
    public int GetLevel()
    {
        return SceneManager.GetActiveScene().buildIndex - 1;
    }
}

[System.Serializable]
public class LevelFoods
{
    [GUIColor(.5f, 1f, 0.2f)]
    [SerializeField] private int level;
    [GUIColor(0.1f, 1f, 1f)]
    [SerializeField] private List<Food> foods;

    public List<Food> Foods => foods;
    public int Level => level;
}

[System.Serializable]
public class Food
{
    [GUIColor(0.7f, 0.8f, 1.2f)]
    [SerializeField] private string name;
    [GUIColor(.7f, 0.8f, 1.2f)]
    [SerializeField] private Sprite sprite;
    [GUIColor(1f, 0.7f, 0.7f)]
    [SerializeField] private List<FoodStuffMaterial> foodStuffsMaterials;

    public string Name => name;
    public Sprite Sprite => sprite;
    public List<FoodStuffMaterial> FoodStuffsMaterials => foodStuffsMaterials;
    public int Count => foodStuffsMaterials.Count;
}

[System.Serializable]
public class FoodStuffMaterial
{
    [GUIColor(1f, .5f, 1f)]
    [SerializeField] private Sprite sprite;
    [GUIColor(.5f, 1f, 1.5f)]
    [SerializeField] private FoodStuffType foodStuffType;

    public Sprite Sprite => sprite;
    public FoodStuffType FoodStuffType => foodStuffType;
}