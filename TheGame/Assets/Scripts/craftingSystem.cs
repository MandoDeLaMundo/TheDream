using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftingSystem : MonoBehaviour
{
    public static craftingSystem instance;

    [Header("Crafting Display")]
    [SerializeField] GameObject craftActive;

    [Header("Ingredents Display")]
    [SerializeField] Image ingredentOne;
    [SerializeField] Image ingredentTwo;
    [SerializeField] Image result;

    [Header("Recipes")]
    [SerializeField] List<Recipes> recipes = new List<Recipes>();
    [SerializeField] ItemCount ingredents;


    int recipePos;
    bool IsHealPotion;
    bool IsManaPotion;
    bool IsHealPotionPlus;
    [SerializeField] float PotionCooldown;
    float makePotionsTimer;

    void Awake()
    {
        instance = this;
        craftActive.SetActive(true);

        IsHealPotion = true;
        IsManaPotion = false;
        IsHealPotionPlus = false;

        if (recipes != null)
        {
            ingredentOne.GetComponent<Image>().sprite = recipes[recipePos].ingredentsOne;
            ingredentTwo.GetComponent<Image>().sprite = recipes[recipePos].ingredentsTwo;
            result.GetComponent<Image>().sprite = recipes[recipePos].result;
            recipePos = 1;
        }
    }

    void Update()
    {
        makePotionsTimer += Time.deltaTime;
        if (playerController.instance.IsInventory)
        {
            if (Input.GetKeyDown("r"))
            {
                if (recipePos < recipes.Count)
                {
                    SetCraft();
                    recipePos++;
                }
                if (recipePos >= recipes.Count)
                {
                    recipePos = 0;
                }
            }
            if (Input.GetKeyDown("c") && makePotionsTimer > PotionCooldown)
            {
                CraftPotion();
            }
        }
    }

    void SetCraft()
    {
        if (recipes[recipePos] != null)
        {
            ingredentOne.GetComponent<Image>().sprite = recipes[recipePos].ingredentsOne;
            ingredentTwo.GetComponent<Image>().sprite = recipes[recipePos].ingredentsTwo;
            result.GetComponent<Image>().sprite = recipes[recipePos].result;
            switch (recipePos)
            {
                case 0:
                    IsHealPotion = true;
                    IsManaPotion = false;
                    IsHealPotionPlus = false;

                    break;
                case 1:
                    IsHealPotion = false;
                    IsManaPotion = true;
                    IsHealPotionPlus = false;

                    break;
                case 2:
                    IsHealPotion = false;
                    IsManaPotion = false;
                    IsHealPotionPlus = true;


                    break;
                case 3:
                    IsHealPotion = false;
                    IsManaPotion = false;
                    IsHealPotionPlus = false;

                    break;//venom and leaf for Mana+
            }
        }
    }

    void CraftPotion()
    {
        if (IsHPPotion() && ingredents.beewaxCount > 0 && ingredents.mushroomCount > 0)
        {
            ingredents.HealthPotion++;
            ingredents.beewaxCount--;
            ingredents.mushroomCount--;

            makePotionsTimer = 0;
        }
        else if (IsMPPotion() && ingredents.beewaxCount > 0 && ingredents.venomGlandCount > 0)
        {
            ingredents.ManaPotion++;
            ingredents.beewaxCount--;
            ingredents.venomGlandCount--;
        }
        else if (IsHPPotionPlus() && ingredents.beewaxCount > 0 && ingredents.leafCount > 0)
        {
            ingredents.ManaPotion++;
            ingredents.beewaxCount--;
            ingredents.leafCount--;
        }
        else if (IsMPPotion() && ingredents.venomGlandCount > 0 && ingredents.leafCount > 0)
        {
            ingredents.ManaPotion++;
            ingredents.venomGlandCount--;
            ingredents.leafCount--;
        }
        InventorySystem.instance.VerifyCount();
        gameManager.instance.UpdatePotionCount();
    }

    public bool IsHPPotion()
    {
        return IsHealPotion;
    }

    public bool IsMPPotion()
    {
        return IsManaPotion;
    }

    public bool IsHPPotionPlus()
    {
        return IsHealPotionPlus;
    }
}
