using UnityEngine;
using UnityEngine.UI;

public class DisplayHotBar : MonoBehaviour
{
    public static DisplayHotBar instance;

    [Header("CurrentSpell")]
    [SerializeField] GameObject CurrentHotBar;
    public Image MainSpell;
    [SerializeField] GameObject OneSlots;

    [Header("TwoSlots")]
    [SerializeField] GameObject TwoSlots;
    public Image TwoSpellOne;
    public Image TwoSpellTwo;

    [Header("ThreeSlots")]
    [SerializeField] GameObject ThreeSlots;
    public Image ThreeSpellOne;
    public Image ThreeSpellTwo;
    public Image ThreeSpellThree;

    [Header("FourSlots")]
    [SerializeField] GameObject FourSlots;
    public Image FourSpellOne;
    public Image FourSpellTwo;
    public Image FourSpellThree;
    public Image FourSpellFour;

    [Header("FiveSlots")]
    [SerializeField] GameObject FiveSlots;
    public Image FiveSpellOne;
    public Image FiveSpellTwo;
    public Image FiveSpellThree;
    public Image FiveSpellFour;
    public Image FiveSpellFive;


    [SerializeField] ListsTracker listsTracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(gameObject);
        //if (listsTracker.spellList != null)
        //    switch (listsTracker.spellList.Count)
        //    {
        //        case 1:
        //            if (MainSpell.sprite != null)
        //            {
        //                OneSlots.SetActive(true);
        //                MainSpell.sprite = listsTracker.spellList[0].sprite;
        //            }
        //            break;
        //        case 2:
        //            TwoSpellOne.sprite = listsTracker.spellList[0].sprite;
        //            TwoSpellTwo.sprite = listsTracker.spellList[1].sprite;
        //            OneSlots.SetActive(true);
        //            CurrentHotBar = TwoSlots;
        //            CurrentHotBar.SetActive(true);
        //            break;
        //        case 3:
        //            ThreeSpellOne.sprite = listsTracker.spellList[0].sprite;
        //            ThreeSpellTwo.sprite = listsTracker.spellList[1].sprite;
        //            ThreeSpellThree.sprite = listsTracker.spellList[2].sprite;
        //            OneSlots.SetActive(true);
        //            CurrentHotBar = ThreeSlots;
        //            CurrentHotBar.SetActive(true);
        //            break;
        //        case 4:
        //            FourSpellOne.sprite = listsTracker.spellList[0].sprite;
        //            FourSpellTwo.sprite = listsTracker.spellList[1].sprite;
        //            FourSpellThree.sprite = listsTracker.spellList[2].sprite;
        //            FourSpellFour.sprite = listsTracker.spellList[3].sprite;
        //            OneSlots.SetActive(true);
        //            CurrentHotBar = FourSlots;
        //            CurrentHotBar.SetActive(true);
        //            break;
        //        case 5:
        //            FiveSpellOne.sprite = listsTracker.spellList[0].sprite;
        //            FiveSpellTwo.sprite = listsTracker.spellList[1].sprite;
        //            FiveSpellThree.sprite = listsTracker.spellList[2].sprite;
        //            FiveSpellFour.sprite = listsTracker.spellList[3].sprite;
        //            FiveSpellFive.sprite = listsTracker.spellList[4].sprite;
        //            OneSlots.SetActive(true);
        //            CurrentHotBar = FiveSlots;
        //            CurrentHotBar.SetActive(true);
        //            break;
        //        default:
        //            break;
        //    }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HotBar(int spell)
    {
        if (spell < listsTracker.spellList.Count)
        {
            switch (listsTracker.spellList.Count - 1)
            {
                case 0:
                    if (OneSlots != null)
                        OneSlots.SetActive(true);
                    MainSpell.sprite = listsTracker.spellList[spell].sprite;
                    TwoSpellOne.sprite = listsTracker.spellList[spell].sprite;
                    ThreeSpellOne.sprite = listsTracker.spellList[spell].sprite;
                    FourSpellOne.sprite = listsTracker.spellList[spell].sprite;
                    FiveSpellOne.sprite = listsTracker.spellList[spell].sprite;
                    break;
                case 1:
                    if (CurrentHotBar != TwoSlots)
                    {
                        //CurrentHotBar.SetActive(false);
                        CurrentHotBar = TwoSlots;
                        CurrentHotBar.SetActive(true);
                    }
                    switch (spell)
                    {
                        case 0:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 1:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 2:
                            listsTracker.spellListPos -= 1;
                            break;
                    }
                    break;
                case 2:
                    if (CurrentHotBar == TwoSlots)
                    {
                        CurrentHotBar.SetActive(false);
                        CurrentHotBar = ThreeSlots;
                        CurrentHotBar.SetActive(true);
                    }
                    switch (spell)
                    {
                        case 0:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 1:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 2:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 3:
                            listsTracker.spellListPos -= 1;
                            break;
                    }
                    break;
                case 3:
                    if (CurrentHotBar == ThreeSlots)
                    {
                        CurrentHotBar.SetActive(false);
                        CurrentHotBar = FourSlots;
                        CurrentHotBar.SetActive(true);
                    }
                    switch (spell)
                    {
                        case 0:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 1:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 2:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 3:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellFour.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellFour.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 4:
                            listsTracker.spellListPos -= 1;
                            break;
                    }
                    break;
                case 4:
                    if (CurrentHotBar == FourSlots)
                    {
                        CurrentHotBar.SetActive(false);
                        CurrentHotBar = FiveSlots;
                        CurrentHotBar.SetActive(true);
                    }
                    switch (spell)
                    {
                        case 0:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellOne.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 1:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            TwoSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellTwo.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 2:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            ThreeSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellThree.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 3:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            FourSpellFour.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellFour.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 4:
                            MainSpell.sprite = listsTracker.spellList[spell].sprite;
                            FiveSpellFive.sprite = listsTracker.spellList[spell].sprite;
                            break;
                        case 5:
                            listsTracker.spellListPos -= 1;
                            break;

                    }
                    break;
            }
        }
    }
}
