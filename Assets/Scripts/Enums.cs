using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public enum WeaponType
    {
        All,
        Wand,
        Sword,
    }

    public enum ObjectType
    {
        None,
        Player,
        Enemy,
    }

    public enum ElementType
    {
        None,
        Fire,
        Ice,
        Electric,
    }

    public enum BoonType
    {
        weaponUpgrade,
        abilityUpgrade,
        healthUpgrade,
        speedUpgrade,
        utility,
    }

    public enum AbilityType
    {
        None,
        SwitchSword,
        FireDoT,
        IceDoT,
        LightningDoT,
    }

    public enum UtilityType
    {
        doubleJump,
        doubleDash,
        HealingOverTime,
    }
}
