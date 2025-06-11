using UnityEngine;

public class Cheatmanager : MonoBehaviour
{

    public static Cheatmanager instance;

    bool invulnerable = false;

    private KeyCode[] invulnerablecheatCode =
    {
        KeyCode.V,
        KeyCode.U,
        KeyCode.N
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
            if (Input.GetKeyDown(invulnerablecheatCode[curIndex]))
            {
                curIndex++;
                if (curIndex >= invulnerablecheatCode.Length)
                {
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
