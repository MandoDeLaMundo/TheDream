using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static UnityEditor.Progress;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    public List<itemStats> inventoryStats = new List<itemStats>();

    public ItemCount ingredents;

    public GameObject Inventory;

    public GameObject RowOne;
    public Image R1SlotOne;
    public Image R1SlotTwo;
    public Image R1SlotThree;
    public Image R1SlotFour;
    public Image R1SlotFive;

    public TMP_Text R1SlotOnetext;
    public TMP_Text R1SlotTwotext;
    public TMP_Text R1SlotThreetext;
    public TMP_Text R1SlotFourtext;
    public TMP_Text R1SlotFivetext;

    public GameObject RowTwo;
    public Image R2SlotOne;
    public Image R2SlotTwo;
    public Image R2SlotThree;
    public Image R2SlotFour;
    public Image R2SlotFive;

    public TMP_Text R2SlotOnetext;
    public TMP_Text R2SlotTwotext;
    public TMP_Text R2SlotThreetext;
    public TMP_Text R2SlotFourtext;
    public TMP_Text R2SlotFivetext;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // or log a warning

        RowOne.SetActive(false);
        RowTwo.SetActive(false);
        Inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateInventoryCount()
    {
        for (int index = 0; index < inventoryStats.Count; index++)
        {
            switch (index)
            {
                //Row1
                case 0:
                    R1SlotOnetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 1:
                    R1SlotTwotext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 2:
                    R1SlotThreetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 3:
                    R1SlotFourtext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 4:
                    R1SlotFivetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                //Row2
                case 5:
                    RowTwo.SetActive(true);

                    R2SlotOnetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 6:
                    R2SlotTwotext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 7:
                    R2SlotThreetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 8:
                    R2SlotFourtext.text = inventoryStats[index].Count.ToString("F0");
                    break;
                case 9:
                    R2SlotFivetext.text = inventoryStats[index].Count.ToString("F0");
                    break;
            }
        }
    }

    public void StoredInventory(int item)
    {
        switch (item)
        {
            //Row1
            case 0:
                RowOne.SetActive(true);
                R1SlotOne.sprite = inventoryStats[item].sprite;
                break;
            case 1:
                R1SlotTwo.sprite = inventoryStats[item].sprite;
                break;
            case 2:
                R1SlotThree.sprite = inventoryStats[item].sprite;
                break;
            case 3:
                R1SlotFour.sprite = inventoryStats[item].sprite;
                break;
            case 4:
                R1SlotFive.sprite = inventoryStats[item].sprite;
                break;
            //Row2
            case 5:
                RowTwo.SetActive(true);

                R2SlotOne.sprite = inventoryStats[item].sprite;
                break;
            case 6:
                R2SlotTwo.sprite = inventoryStats[item].sprite;
                break;
            case 7:
                R2SlotThree.sprite = inventoryStats[item].sprite;
                break;
            case 8:
                R2SlotFour.sprite = inventoryStats[item].sprite;
                break;
            case 9:
                R2SlotFive.sprite = inventoryStats[item].sprite;
                break;

        }
    }

    public void VerifyCount()
    {
        for (int i = 0; i < inventoryStats.Count; i++)
        {
            if (inventoryStats[i].itemName == "Bee Wax")
            {
                inventoryStats[i].Count = ingredents.beewaxCount;
            }
            else if (inventoryStats[i].itemName == "Boar Meat")
            {
                inventoryStats[i].Count = ingredents.baconCount;
            }
            else if (inventoryStats[i].itemName == "Mushroom")
            {
                inventoryStats[i].Count = ingredents.mushroomCount;
            }
            else if (inventoryStats[i].itemName == "Venom Gland")
            {
                inventoryStats[i].Count = ingredents.venomGlandCount;
            }
            else if (inventoryStats[i].itemName == "Cinnamon")
            {
                inventoryStats[i].Count = ingredents.cinnamonCount;
            }
            UpdateInventoryCount();
        }
    }
}
