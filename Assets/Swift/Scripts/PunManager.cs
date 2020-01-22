using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.UI;

public class PunManager : MonoBehaviourPunCallbacks
{
    public enum DeviceType
    {
        NA,
        VR,
        AR,
        PC
    }
    public Image blackScreen;
    public GameObject vrPlayerPrefab;
    public GameObject thirdPersonPlayerPrefab;
    public DeviceType editorDeviceType;
    [ReadOnly]
    public DeviceType deviceType;
    public string ipAddress;

    public float lobbyTime = 10f;
    protected float timer = 0;

    public GameObject connectingString;
    private  void Awake ()
    {
        //PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(this);

        PhotonNetwork.AuthValues =  new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = Guid.NewGuid().ToString();
    }
    

    public override void OnConnectedToMaster ()
    {
        Debug.Log("Connected to Server");

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected : " + cause.ToString());

        PhotonNetwork.ConnectToMaster(ipAddress, 5055, "appId");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");

        StartCoroutine(LoadLobby());
    }

    private IEnumerator LoadLobby ()
    {
        for (float i = 0; i <= 1; i += 0.02f)
        {
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    public void JoinRoom ()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 10;

        PhotonNetwork.JoinOrCreateRoom("The Game", opt, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room : " + PhotonNetwork.CurrentRoom.Name);
        
        StartCoroutine(LoadShopLayout());
    }
    
    private IEnumerator LoadShopLayout ()
    {
        for (float i = 0; i <= 1; i += 0.02f)
        {
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (Application.isEditor && editorDeviceType != DeviceType.NA)
            deviceType = editorDeviceType;
        
        switch (deviceType)
        {
            case DeviceType.NA:
                throw new Exception("No device");
            case DeviceType.VR:
                Debug.Log("Start VR mode");
                PhotonNetwork.Instantiate(vrPlayerPrefab.name, SpawnPoint.point.position, SpawnPoint.point.rotation, 0);
            break;
            case DeviceType.PC:
                Debug.Log("Start PC mode");
                PhotonNetwork.Instantiate(thirdPersonPlayerPrefab.name, SpawnPoint.point.position, SpawnPoint.point.rotation, 0);
            break;
        }
        
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            blackScreen.color = new Color(0, 0, 0, i);
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to join room : " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Failed to create room : " + message);
    }

    public void SteamReady (bool isVR)
    {
        //stay vide
    }

    private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("EXPORT CONFIG");
            Configuration.Export();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("IMPORT CONFIG");
            Configuration.Import();
        }
        if (PhotonNetwork.InLobby)
        {
            if (timer >= lobbyTime)
            {
                JoinRoom();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    public void OnEndEdit(string ip)
    {
        connectingString.SetActive(true);
        deviceType = DeviceType.AR;        
        PhotonNetwork.ConnectToMaster(ip, 5055, "appId");
    }
    
}
