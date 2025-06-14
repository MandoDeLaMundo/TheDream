using UnityEngine;

public class craftingSystem : MonoBehaviour
{
    public static craftingSystem instance;

    [SerializeField] GameObject craftActive;
    [SerializeField] GameObject craftHeal;
    [SerializeField] GameObject craftMana;

    [SerializeField] GameObject potionActive;
    [SerializeField] GameObject potionHeal;
    [SerializeField] GameObject potionMana;

    bool IsHealPotion;
    bool IsManaPotion;

    void Awake()
    {
        instance = this;

        craftActive = craftHeal;
        craftActive.SetActive(true);

        potionActive = potionHeal;
        potionActive.SetActive(true);

        IsHealPotion = true;
        IsManaPotion = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            if (IsHealPotion)
            {
                craftActive.SetActive(false);
                craftActive = craftMana;
                craftActive.SetActive(true);
                IsHealPotion = false;
                IsManaPotion = true;
                if (potionActive != null)
                {
                    potionActive.SetActive(false);
                    potionActive = potionMana;
                    potionActive.SetActive(true);
                }
            }
            else if (IsManaPotion)
            {
                craftActive.SetActive(false);
                craftActive = craftHeal;
                craftActive.SetActive(true);
                IsHealPotion = true;
                IsManaPotion = false;
                if (potionActive != null)
                {
                    potionActive.SetActive(false);
                    potionActive = potionHeal;
                    potionActive.SetActive(true);
                }
            }
        }
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
