using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.UI;

public class OpeningInterface : MonoBehaviour
{

    SteamVR_Input_Sources inputSource;
    SteamVR_Behaviour_Pose pose;
    private bool isOpen = false;
    private int numChild = -1;
    


    public GameObject menu;

    public GameObject[] childArray = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SteamVR_Actions._default.InterfaceOpening.GetStateDown(inputSource))
        {
            if(isOpen == false)
            {
                isOpen = true;
                menu.SetActive(isOpen);
            } 
            else 
            {
                foreach(GameObject gameobject in childArray)
                {
                    gameobject.GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                }
                isOpen = false;
                menu.SetActive(isOpen);
                numChild = -1;
            }
        }

        if(SteamVR_Actions._default.ValidateAction.GetStateDown(inputSource))
        {
            Debug.Log("VALIDATE");
        }

        if(SteamVR_Actions._default.SelectOptionBot.GetStateDown(inputSource))
        {
            if(numChild == -1)
            {
                numChild = 0;
            }
            else if(numChild == 2)
            {
                childArray[numChild].GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                numChild = 0;
            }
            else
            {
                childArray[numChild].GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                numChild = numChild + 1;
            }
            childArray[numChild].GetComponent<Image>().color = new Color32(140, 140, 140, 175);
        }

        if(SteamVR_Actions._default.SelectOptionTop.GetStateDown(inputSource))
        {
            if(numChild == -1)
            {
                numChild = 2;
            }
            else if(numChild == 0)
            {
                childArray[numChild].GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                numChild = 2; 
            }
            else
            {
                childArray[numChild].GetComponent<Image>().color = new Color32(255, 255, 255, 175);
                numChild = numChild - 1;
            }
            childArray[numChild].GetComponent<Image>().color = new Color32(140, 140, 140, 175);
        }

        Debug.Log(numChild);
    }

    void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource =  pose.inputSource;
        menu.SetActive(isOpen);
    }
}
