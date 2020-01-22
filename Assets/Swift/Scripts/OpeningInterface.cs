using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class OpeningInterface : MonoBehaviour
{

    SteamVR_Input_Sources inputSource;
    SteamVR_Behaviour_Pose pose;
    private bool isOpen = false;


    public GameObject menu;


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
                isOpen = false;
                menu.SetActive(isOpen);
            }
        }
    }

    void Awake()
    {
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource =  pose.inputSource;
        menu.SetActive(isOpen);
    }
}
