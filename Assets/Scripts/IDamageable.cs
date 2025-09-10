using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public interface IDamageable
    {
        void TakeDamage(int damage, ElementType elementalType);
    }

}
