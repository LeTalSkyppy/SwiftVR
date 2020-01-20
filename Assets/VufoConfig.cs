using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VufoConfig : DefaultTrackableEventHandler
{
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();

        if(mTrackableBehaviour)
        {
            var rigidbodies = mTrackableBehaviour.GetComponentsInChildren<Rigidbody>(true);

            foreach(Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
            }
        }
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();

        if(mTrackableBehaviour)
        {
            var rigidbodies = mTrackableBehaviour.GetComponentsInChildren<Rigidbody>(true);

            foreach(Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = true;
            }
        }
    }
}
