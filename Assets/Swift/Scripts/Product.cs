using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Product : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView target;
    public int targetId;
    public string type;
    public List<Text> textList = new List<Text>();
    Vector3 diff = Vector3.zero;
    Vector3 targetPosition = Vector3.zero;

    Vector3 initialPosition = Vector3.zero;

    float diffSpeed;

    public float speed = 50f;

    public float speedoRotato = 500f;

    private bool observabled = false;
    void Start()
    {
        initialPosition = transform.position;
    }
    void Update()
    {
        if(target != null)
        {
            if(targetPosition == Vector3.zero || diff == Vector3.zero)
            {
                targetPosition = target.transform.position;
                diff = targetPosition - initialPosition;
                diffSpeed = diff.magnitude;
            }

            if(targetPosition != target.transform.position)
            {
                targetPosition = target.transform.position;
                //diff = targetPosition - initialPosition;
            }

            Vector3 diffInstantT = targetPosition - transform.position;            
            Vector3 diffCalcul = diffInstantT *(diffSpeed/diffInstantT.magnitude);
            transform.position += diffCalcul/100f * speed * Time.deltaTime;
            transform.Rotate(new Vector3(0,speedoRotato * Time.deltaTime,0));
            if(diffInstantT.magnitude < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTypeProduct(string type)
    {
        foreach(Text text in textList)
        {
            text.text = type;
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(!observabled)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(targetId);
                stream.SendNext(type);
            }
            else
            {
                targetId = (int) stream.ReceiveNext();
                type  = (string) stream.ReceiveNext();

                target = PhotonNetwork.GetPhotonView(targetId);
                SetTypeProduct(type);
            }
            observabled = true;
        }
       
    }
}
