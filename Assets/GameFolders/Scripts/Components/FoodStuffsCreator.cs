using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStuffsCreator : MonoSingleton<FoodStuffsCreator>
{
    [SerializeField] private List<FoodStuff> foodStuffs;
    [SerializeField] private int extraFoodAmount;
    [SerializeField] private float instantiateTimeBetween;
    [SerializeField] private Vector3 maxVelocity;

    private FoodData _foodData;
    private int foodAmount;

    private void Awake()
    {
        _foodData = Resources.Load("FoodData") as FoodData;
    }

    void Start()
    {
        Singleton();
        StartCoroutine(InstantiateFoods());
    }
    
    private FoodStuff GetFoodStuff(FoodStuffType foodStuffType)
    {
        foreach (FoodStuff foodStuff in foodStuffs)
        {
            if (foodStuff.FoodStuffType == foodStuffType)
            {
                return foodStuff;
            }
        }

        return null;
    }

    IEnumerator InstantiateFoods()
    {
        List<FoodStuff> createdFoodStuffs = new List<FoodStuff>();
        
        foreach (Food food in _foodData.GetFoods())
        {
            foreach (FoodStuffMaterial foodStuffMaterial in food.FoodStuffsMaterials)
            {
                FoodStuff tempFood = GetFoodStuff(foodStuffMaterial.FoodStuffType);
                FoodStuff newFoodStuff = Instantiate(tempFood, transform);
                createdFoodStuffs.Add(newFoodStuff);
                float rndX, rndY, rndZ;
                
                rndX = Random.Range(-maxVelocity.x, maxVelocity.x);
                rndY = Random.Range(0, maxVelocity.y);
                rndZ = Random.Range(-maxVelocity.z, maxVelocity.z);
                
                newFoodStuff.FallDown(new Vector3(rndX, rndY, rndZ));
                yield return new WaitForSeconds(instantiateTimeBetween);
            }

            yield return null;
        }
        
        for (int i = 0; i < extraFoodAmount; i++)
        {
            int randomIndex = Random.Range(0, foodStuffs.Count);
            FoodStuff newFoodStuff = Instantiate(foodStuffs[randomIndex], transform);
            createdFoodStuffs.Add(newFoodStuff);
            float rndX, rndY, rndZ;
                
            rndX = Random.Range(-maxVelocity.x, maxVelocity.x);
            rndY = Random.Range(0, maxVelocity.y);
            rndZ = Random.Range(-maxVelocity.z, maxVelocity.z);
                
            newFoodStuff.FallDown(new Vector3(rndX, rndY, rndZ));
            yield return new WaitForSeconds(instantiateTimeBetween);
        }

        yield return new WaitForSeconds(1);
        
        foreach (FoodStuff foodStuff in createdFoodStuffs)
        {
            foodStuff.FreezeConstraints(RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY);
        }
    }
}