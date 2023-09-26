using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventCommunicator : MonoBehaviour
{
    public UnityEvent callback;

    public void AnimEventFire()
    {
        callback?.Invoke();
    }
}
