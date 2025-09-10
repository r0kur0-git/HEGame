using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PRJCTA.HOLLOWECHOES
{
    public class PlayerAttack : MonoBehaviour
    {
        AnimatorHandler animatorHandler;        
        PlayerInputManager inputManager;
        WeaponHolderSlot weaponHolderSlot;
        PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;
        PlayerStats playerStats;

        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            inputManager = GetComponentInChildren<PlayerInputManager>();
            weaponHolderSlot = GetComponentInParent<WeaponHolderSlot>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
        }

        public void HandleWeaponCombo(WeaponItem weapon, WeaponType weaponType)
        {
            if (inputManager._combo)
            {
                animatorHandler.animator.SetBool("canDoCombo", false);

                switch (weaponType)
                {
                    case WeaponType.Wand:
                        if (playerManager.isGrounded)
                        {
                            if (lastAttack == weapon.AttackSeq1)
                            {
                                animatorHandler.PlayTargetAnimation(weapon.AttackSeq2, true);
                                inputManager._actionTriggered = true;
                                lastAttack = weapon.AttackSeq2;
                                Debug.Log("Ground Attack 2");
                            }
                            else if (lastAttack == weapon.AttackSeq2)
                            {
                                animatorHandler.PlayTargetAnimation(weapon.AttackSeq3, true);
                                inputManager._actionTriggered = true;
                                lastAttack = weapon.AttackSeq3;
                                Debug.Log("Ground Attack 3");
                            }
                        }
                        else // not grounded
                        {
                            if (lastAttack == weapon.A_AttackSeq1)
                            {
                                animatorHandler.PlayTargetAnimation(weapon.A_AttackSeq2, true);
                                inputManager._actionTriggered = true;
                                lastAttack = weapon.A_AttackSeq2;
                                Debug.Log("Air Attack 2");
                            }
                            else if (lastAttack == weapon.A_AttackSeq2)
                            {
                                animatorHandler.PlayTargetAnimation(weapon.A_AttackSeq3, true);
                                inputManager._actionTriggered = true;
                                lastAttack = weapon.A_AttackSeq3;
                                Debug.Log("Air Attack 3");
                            }
                        }
                            break;
                        case WeaponType.Sword:
                        break;
                }
            }
        }

        public void HandleBasicAttack(WeaponItem weapon, WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Wand:
                    if (playerManager.isGrounded)
                    {
                        // Ground attack
                        animatorHandler.PlayTargetAnimation(weapon.AttackSeq1, true);
                        inputManager._actionTriggered = true;
                        lastAttack = weapon.AttackSeq1;
                        Debug.Log("Ground Wand Attack");
                    }
                    else
                    {
                        // Aerial attack
                        animatorHandler.PlayTargetAnimation(weapon.A_AttackSeq1, true);
                        inputManager._actionTriggered = true;
                        lastAttack = weapon.A_AttackSeq1;
                        Debug.Log("Aerial Wand Attack");
                    }
                    break;

                case WeaponType.Sword:
                    animatorHandler.PlayTargetAnimation(weapon.AttackSeq1, true);
                    inputManager._actionTriggered = true;
                    lastAttack = weapon.AttackSeq1;
                    Debug.Log("Attack 1");
                    break;
            }
        }

        /*public void HandleCharged(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.ChargedAttack, true);
            Debug.Log("CA");
        }*/

        public void HandleSpellOneAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.SpellOne, true);
            Debug.Log("Spell 1");
        }

        public void HandleSpellTwoAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.SpellTwo, true);
            Debug.Log("Spell 2");
        }
        
        public void HandleSpellThreeAttack(WeaponItem weapon)
        {
            animatorHandler.PlayTargetAnimation(weapon.SpellThree, true);
            Debug.Log("Spell 3");
        }
    }
}