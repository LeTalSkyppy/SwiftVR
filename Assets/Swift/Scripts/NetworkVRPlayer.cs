using UnityEngine;
using Photon.Pun;
using Valve.VR.InteractionSystem;
using Valve.VR;
using System.Collections;

public class NetworkVRPlayer : MonoBehaviourPunCallbacks
{
    public GameObject steamVRPrefab;
    public GameObject observed;
    public PhotonTransformViewFrom leftHandView;
    public PhotonTransformViewFrom rightHandView;
    public PhotonTransformViewFrom lookAtView;
    protected Animator animator;

    private void Awake ()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.enabled = false;
            }

            observed = Instantiate(steamVRPrefab, transform.position, transform.rotation);
            Player player = observed.GetComponent<Player>();

            // set the transform to observe
            GetComponent<PhotonTransformViewFrom>().m_Other = player.transform;
            Transform lookAt = observed.transform.Find("SteamVRObjects").Find("VRCamera").Find("LookAt");
            lookAtView.m_Other = lookAt;

            StartCoroutine(WaitForHand(leftHandView,  player.leftHand));
            StartCoroutine(WaitForHand(rightHandView, player.rightHand));
        }
    }

    private IEnumerator WaitForHand (PhotonTransformViewFrom view, Hand hand)
    {
        while (!hand.mainRenderModel)
            yield return null;

        view.m_Other = hand.mainRenderModel.transform;
    }

    private void OnAnimatorIK (int layerIndex)
    {
        animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, .5f);
        animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, .5f);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, .7f);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, .7f);

        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, .5f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, .5f);

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandView.transform.position);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandView.transform.position);

        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandView.transform.rotation);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandView.transform.rotation);

        animator.SetLookAtWeight(.8f);
        animator.SetLookAtPosition(lookAtView.transform.position);
    }
}
