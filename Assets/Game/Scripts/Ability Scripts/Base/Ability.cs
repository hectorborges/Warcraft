﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;
    public Sprite abilityIcon;
    public Sprite abilityIconGreyscale;
    public int abilityResourceRequirement;
    public float abilityRange;
    public int abilityMinDamage;
    public int abilityMaxDamage;
    public float abilityCooldown;
    [TextArea]
    public string abilityDescription;
    public bool abilityEnchantable;
    public float statusLength;
    public bool requiresTarget;
    public bool abilityUnlocked;

    public Animator anim;

    public enum Enchant
    {
        None,
        Dot,
        Slow,
        Weighted
    };

    public enum ResourceType
    {
        None,
        Arrows,
        Energy
    };

    public ResourceType resourceType;


    public Enchant enchant;
    public Enchant currentEnchant;

    bool onCooldown;
    [HideInInspector]
    public bool globalCooldown;

    private void Start()
    {
        if (abilityUnlocked)
            PlayerPrefsX.SetBool(abilityName, true);
    }

    public virtual void ActivateAbility()
    {

    }

    public virtual void ReActivateAbility()
    {

    }

    public void RemoveEnchant()
    {
        currentEnchant = Enchant.None;
    }

    public void TriggerCooldown()
    {
        onCooldown = true;
        StartCoroutine(Cooldown());
    }

    public bool OnCooldown()
    {
        return onCooldown;
    }

    public bool OnGlobalCooldown()
    {
        return globalCooldown;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(abilityCooldown);
        onCooldown = false;
    }

    public IEnumerator GlobalCooldown()
    {
        globalCooldown = true;
        yield return new WaitForSeconds(1);
        globalCooldown = false;
    }

    public bool AbilityUnlocked()
    {
        if (abilityUnlocked)
            return true;
        else
        return PlayerPrefsX.GetBool(abilityName);
    }

    public void UnlockAbility()
    {
        PlayerPrefsX.SetBool(abilityName, true);
    }

    public void LockAbility()
    {
        PlayerPrefsX.SetBool(abilityName, false);
    }
}
