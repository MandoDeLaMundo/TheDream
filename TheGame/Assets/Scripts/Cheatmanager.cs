using System.Collections.Generic;
using UnityEngine;

public class Cheatmanager : MonoBehaviour
{

    public static Cheatmanager instance;

    bool invulnerable = false;

    public List<spellStats> ListAllSpells = new List<spellStats>();
    public bool spellCheat;

    public bool DescriptionCheat;
    public bool SpeedCheat;

    private KeyCode[] invulnerablecheatCode =
    {
        KeyCode.I,
        KeyCode.N,
        KeyCode.V,
        KeyCode.U,
        KeyCode.N
    };
    private KeyCode[] allSpellCheatCode =
    {
        KeyCode.M,
        KeyCode.A,
        KeyCode.G,
        KeyCode.I,
        KeyCode.C
    };
    private KeyCode[] DescriptionBoxCheatCode =
    {
        KeyCode.D,
        KeyCode.E,
        KeyCode.S,
        KeyCode.C,
        KeyCode.T,
    };
    private KeyCode[] SuperSpeedCheatCode =
    {
        KeyCode.S,
        KeyCode.U,
        KeyCode.P,
        KeyCode.E,
        KeyCode.R,
    };

    private int curIndex = 0;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(DescriptionBoxCheatCode[curIndex]))
            {
                curIndex++;
                if (curIndex >= DescriptionBoxCheatCode.Length)
                {
                    if (DescriptionCheat)
                    {
                        DescriptionCheat = false;
                    }
                    else
                    {
                        DescriptionCheat = true;
                    }
                    curIndex = 0;
                }
            }
            else if (Input.GetKeyDown(allSpellCheatCode[curIndex]))
            {
                curIndex++;
                if (curIndex >= allSpellCheatCode.Length)
                {
                    if (spellCheat)
                    {

                        gameManager.instance.MainSpell.sprite = null;
                        gameManager.instance.SpellOne.sprite = null;
                        gameManager.instance.SpellTwo.sprite = null;
                        gameManager.instance.SpellThree.sprite = null;
                        gameManager.instance.SpellFour.sprite = null;
                        gameManager.instance.SpellFive.sprite = null;

                        playerController.instance.spellList.Clear();
                        playerController.instance.spellListPos = 0;
                        spellCheat = false;
                    }
                    else
                    {
                        for (int index = 0; index < ListAllSpells.Count; index++)
                        {
                            spellCheat = true;
                            ListAllSpells[index].spellCheck = false;
                            playerController.instance.GetSpellStats(ListAllSpells[index]);
                        }
                    }
                    curIndex = 0;
                }
            }
            else if (Input.GetKeyDown(invulnerablecheatCode[curIndex]))
            {
                curIndex++;
                if (curIndex >= invulnerablecheatCode.Length)
                {
                    if (invulnerable)
                    {
                        invulnerable = false;
                        gameManager.instance.GodMode.SetActive(false);
                        gameManager.instance.NormalMode.SetActive(true);
                    }
                    else
                    {
                        invulnerable = true;
                        gameManager.instance.GodMode.SetActive(true);
                        gameManager.instance.NormalMode.SetActive(false);
                    }
                    curIndex = 0;
                }
            }
            else if (Input.GetKeyDown(SuperSpeedCheatCode[curIndex]))
            {
                curIndex++;
                if (curIndex >= SuperSpeedCheatCode.Length)
                {
                    if (SpeedCheat)
                    {
                        SpeedCheat = false;
                        playerController.instance.speed = playerController.instance.origSpeed;
                    }
                    else
                    {
                        SpeedCheat = true;
                    }
                    curIndex = 0;
                }
            }
            else if (Input.anyKeyDown)
            {
                curIndex = 0;
            }
        }
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }
}
