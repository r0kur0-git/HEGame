using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class AnimationEventSystem : MonoBehaviour
    {
        public static event Action<GameObject> OnAnimationEventTriggered;

        public void TriggerAnimationEvent(GameObject target)
        {
            OnAnimationEventTriggered?.Invoke(target);
        }
    }
}