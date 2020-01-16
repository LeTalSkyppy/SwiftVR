using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HTC_or_TP : MonoBehaviour
{

    public delegate void HMDed();
    public static event HMDed hmded; 

    public delegate void ThirdPersoned();
    public static event ThirdPersoned thirdPersoned;
    private IEnumerator Start()
    {
        Debug.Log("Coucou");
         while (SteamVR.initializedState == SteamVR.InitializedStates.None || SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
                yield return null;
                
        Debug.Log("TG PD" +SteamVR.instance);
        if(SteamVR.instance != null)
        {
            hmded();
        }
        else
        {
            thirdPersoned();
        }
    }
}
