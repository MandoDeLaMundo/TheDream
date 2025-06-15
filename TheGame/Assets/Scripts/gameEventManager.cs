using UnityEngine;
using System.Collections.Generic;

//public class gameEventManager : MonoBehaviour
//{
//    public static gameEventManager instance;

//    //[SerializeField] List<DialogueStats> dialogueStatsList = new List<DialogueStats>();
//    [SerializeField] List<GameObject> FairyTriggerList = new List<GameObject>();

//    int ListPos = 0;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Awake()
//    {
//        instance = this;
//        if (dialogueStatsList != null && FairyTriggerList != null)
//        {
//            for (; ListPos < dialogueStatsList.Count;)
//            {
//                for (int index = 0; index < dialogueStatsList[ListPos].dialogue.Count; index++)
//                {
//                    FairyTriggerList[ListPos].GetComponent<SelectionSpawner>().dialogue.Add(dialogueStatsList[ListPos].dialogue[index]);
//                }
//                ListPos++;
//            }
//            ListPos = 0;
//        }
//    }

//    public void EventOff(GameObject Spawner)
//    {
//        Spawner.SetActive(false);
//    }
//}
