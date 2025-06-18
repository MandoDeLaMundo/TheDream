using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftingSystem : MonoBehaviour
{
    public static craftingSystem instance;

    [Header("Crafting Display")]
    [SerializeField] GameObject craftActive;
    [SerializeField] GameObject craftHeal;
    [SerializeField] GameObject craftMana;

    [Header("Potion Display")]
    [SerializeField] GameObject potionActive;
    [SerializeField] GameObject potionHeal;
    [SerializeField] GameObject potionMana;

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
    [SerializeField] float healingCooldown;
    float healTimer;

    void Awake()
    {
        instance = this;
        craftActive = craftHeal;
        craftActive.SetActive(true);

        potionActive = potionHeal;
        potionActive.SetActive(true);

        IsHealPotion = true;
        IsManaPotion = false;

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
        healTimer += Time.deltaTime;
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
            if (Input.GetKey("c"))
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


                    break;
                case 1:
                    IsHealPotion = false;
                    IsManaPotion = true;

                    break;
                case 2:
                    IsHealPotion = false;
                    IsManaPotion = false;

                    break;
            }
        }
    }

    void CraftPotion()
    {
        if (IsHPPotion() && ingredents.beewaxCount > 0 && ingredents.mushroomCount > 0 && healTimer > healingCooldown)
        {
            ingredents.HealthPotion++;
            ingredents.beewaxCount--;
            ingredents.mushroomCount--;

            healTimer = 0;
        }
        else if (IsMPPotion() && ingredents.beewaxCount > 0 && ingredents.baconCount > 0)
        {
            ingredents.ManaPotion++;
            ingredents.beewaxCount--;
            ingredents.baconCount--;
        }
        InventorySystem.instance.VerifyCount();
    }

    public bool IsHPPotion()
    {
        return IsHealPotion;
    }

    public bool IsMPPotion()
    {
        return IsManaPotion;
    }
}
