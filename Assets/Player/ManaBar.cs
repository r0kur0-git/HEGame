using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class ManaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxMana(int maxMana)
        {
            slider.maxValue = maxMana;
            slider.value = maxMana;
        }

        public void SetCurrentMana(float currentMana)
        {
            slider.value = currentMana;
        }
    }
}
