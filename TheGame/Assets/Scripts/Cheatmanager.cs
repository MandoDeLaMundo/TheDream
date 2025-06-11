using UnityEngine;

public class Cheatmanager : MonoBehaviour
{

    public static Cheatmanager instance;

    bool invulnerable = false;

    private KeyCode[] cheatCode =
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow
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
            if (Input.GetKeyDown(cheatCode[curIndex]))
            {
                Debug.Log(cheatCode[curIndex]);
                curIndex++;
                if (curIndex >= cheatCode.Length)
                {
                Debug.Log("invulnerable");
                    invulnerable = true;
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
        Debug.Log(" bool invulnerable");
        return invulnerable;
    }
}
