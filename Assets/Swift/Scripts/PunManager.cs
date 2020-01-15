using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.UI;

public class PunManager : MonoBehaviourPunCallbacks
{
    public Image blackScreen;
    public GameObject thirdPersonPlayerPrefab;
    public string ipAddress;

    public float lobbyTime = 10f;
    protected float timer = 0;

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

    [Obsolete("Wait in Lobby 'til we know if a HMD is connected")]
    public void JoinRoom ()
    {
        RoomOptions opt = new RoomOptions();
        opt.MaxPlayers = 7;

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
        
        PhotonNetwork.Instantiate(thirdPersonPlayerPrefab.name, SpawnPoint.point.position, SpawnPoint.point.rotation, 0);
        
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

    private void Update ()
    {
        if (Time.time > 3f)
        {
            PhotonNetwork.ConnectToMaster(ipAddress, 5055, "appId");
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
}
