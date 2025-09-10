using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class AttributesManager : MonoBehaviour
    {
        public int health;
        public int attack;
        public bool isDead;

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health <= 0)
            {
                health = 0;
                isDead = true;
                Destroy(gameObject);
            }
        }

        public void DealDamage(GameObject target)
        {
            var atm = target.GetComponent<AttributesManager>();
            if (atm != null)
            {
                atm.TakeDamage(attack);
            }
        }
    }
}