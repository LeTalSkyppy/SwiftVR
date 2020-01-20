// ----------------------------------------------------------------------------
// <copyright file="PhotonTransformView.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
//   Component to synchronize Transforms via PUN PhotonView.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    using UnityEngine;

    [AddComponentMenu("Photon Networking/Photon Transform View From")]
    [HelpURL("https://doc.photonengine.com/en-us/pun/v2/gameplay/synchronization-and-state")]
    [RequireComponent(typeof(PhotonView))]
    public class PhotonTransformViewFrom : MonoBehaviour, IPunObservable
    {
        public Transform m_Other;

        private float m_Distance;
        private float m_Angle;

        private PhotonView m_PhotonView;

        private Vector3 m_Direction;
        private Vector3 m_NetworkPosition;
        private Vector3 m_StoredPosition;

        private Quaternion m_NetworkRotation;

        public bool m_SynchronizePosition = true;
        public bool m_SynchronizeRotation = true;
        public bool m_SynchronizeScale = false;

        bool m_firstSync = true;
        bool m_firstTake = false;

        public void Awake()
        {
            m_PhotonView = GetComponent<PhotonView>();

            m_NetworkPosition = Vector3.zero;

            m_NetworkRotation = Quaternion.identity;
        }

        void OnEnable()
        {
            m_firstTake = true;
        }

        public void Update()
        {
            if (!this.m_PhotonView.IsMine)
            {
                transform.position = Vector3.MoveTowards(transform.position, this.m_NetworkPosition, this.m_Distance * (1.0f / PhotonNetwork.SerializationRate));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, this.m_NetworkRotation, this.m_Angle * (1.0f / PhotonNetwork.SerializationRate));
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (m_Other && m_firstSync)
                {
                    m_StoredPosition = m_Other.transform.position;
                    m_firstSync = !m_firstSync;
                }
                if (this.m_SynchronizePosition)
                {
                    if (m_Other)
                    {
                        this.m_Direction = m_Other.transform.position - this.m_StoredPosition;
                        this.m_StoredPosition = m_Other.transform.position;

                        stream.SendNext(m_Other.transform.position);
                        stream.SendNext(this.m_Direction);
                    }
                    else
                    {
                        stream.SendNext(Vector3.zero);
                        stream.SendNext(Vector3.forward);
                    }
                }

                if (this.m_SynchronizeRotation)
                {
                    if (m_Other)
                    {
                        stream.SendNext(m_Other.transform.rotation);
                    }
                    else
                    {
                        stream.SendNext(Quaternion.identity);
                    }
                }

                if (this.m_SynchronizeScale)
                {
                    if (m_Other)
                    {
                        stream.SendNext(m_Other.transform.localScale);
                    }
                    else
                    {
                        stream.SendNext(Vector3.one);
                    }
                }
            }
            else
            {
                if (this.m_SynchronizePosition)
                {
                    this.m_NetworkPosition = (Vector3)stream.ReceiveNext();
                    this.m_Direction = (Vector3)stream.ReceiveNext();

                    if (m_firstTake)
                    {
                        transform.position = this.m_NetworkPosition;
                        this.m_Distance = 0f;
                    }
                    else
                    {
                        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                        this.m_NetworkPosition += this.m_Direction * lag;
                        this.m_Distance = Vector3.Distance(transform.position, this.m_NetworkPosition);
                    }                   
                }

                if (this.m_SynchronizeRotation)
                {
                    this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();

                    if (m_firstTake)
                    {
                        this.m_Angle = 0f;
                        transform.rotation = this.m_NetworkRotation;
                    }
                    else
                    {
                        this.m_Angle = Quaternion.Angle(transform.rotation, this.m_NetworkRotation);
                    }
                }

                if (this.m_SynchronizeScale)
                {
                    transform.localScale = (Vector3)stream.ReceiveNext();
                }

                if (m_firstTake)
                {
                    m_firstTake = false;
                }
            }
        }
    }
}