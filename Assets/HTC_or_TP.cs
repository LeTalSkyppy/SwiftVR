using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using System;

public class HTC_or_TP : MonoBehaviour
{
    [Serializable]
    public class Ready : UnityEvent<bool> { };
    public Ready ready;

    private void OnEnable ()
    {
        Debug.Log("Initialize Steam VR");

        SteamVR.Initialize();
    }

    private IEnumerator Start()
    {

         while (SteamVR.initializedState == SteamVR.InitializedStates.None || SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
             yield return null;

         ready.Invoke(SteamVR.instance != null);
    }
}
