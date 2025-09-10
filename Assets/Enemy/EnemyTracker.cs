using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class EnemyTracker : MonoBehaviour
    {
        void OnEnable()
        {
            Wand.RegisterEnemy(gameObject);
        }

        void OnDisable()
        {
            Wand.UnregisterEnemy(gameObject);
        }
    }
}
