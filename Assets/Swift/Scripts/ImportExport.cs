using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ImportExport : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public static ImportExport instance;

    public List<PhotonView> machineOwned = new List<PhotonView>();
    public Exportable[] exportables;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        exportables = GameObject.FindObjectsOfType<Exportable>();
        BetterStreamingAssets.Initialize();
    }

    public override void OnEnable ()
    {
        base.OnEnable();

        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable ()
    {
        base.OnDisable();

        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void Import()
    {
        GetAuthority();
    }

    public void Export()
    {
        Configuration.Export();
    }

    public void GetAuthority()
    {
        machineOwned.Clear();
        foreach(Exportable exportable in exportables)
        {
            PhotonView exportablePhotonView =  exportable.GetComponent<PhotonView>();
            if(exportablePhotonView.Owner != null && exportablePhotonView.Owner.IsLocal)
            {
                machineOwned.Add(exportablePhotonView);
            }
            else
            {
                exportablePhotonView.RequestOwnership();
            }
            
        }
        if(machineOwned.Count == exportables.Length)
            {
                Configuration.Import();
            }
    }

    public void GetAuthorityFromPC()
    {
        machineOwned.Clear();
        foreach(Exportable exportable in exportables)
        {
            PhotonView exportablePhotonView =  exportable.GetComponent<PhotonView>();
            if(exportablePhotonView.Owner != null && exportablePhotonView.Owner.IsLocal)
            {
                machineOwned.Add(exportablePhotonView);
            }
            else
            {
                exportablePhotonView.RequestOwnership();
            }
            
        }
        if(machineOwned.Count == exportables.Length)
        {
            Configuration.ImportFromPC();
        }
    }

    void IPunOwnershipCallbacks.OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        //
    }

    void IPunOwnershipCallbacks.OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if(targetView.Owner.IsLocal)
        {
            machineOwned.Add(targetView);
            
            if(machineOwned.Count == exportables.Length)
            {
                Configuration.Import();
            }
        }
    }
}
