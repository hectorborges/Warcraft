﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    public int explosiveMinDamage;
    public int explosiveMaxDamage;
    public float explosiveRange = 10f;
    public LayerMask targetable;

    public override void SetProjectileValues(Transform _target, int _minimumDamage, int _maximumDamage, int _criticalStrikeChance, float _criticalStrikeDamage, float _statusLength, NamePlate _enemyNamePlate, Ability.Enchant _enchant)
    {
        target = _target;
        minimumDamage = _minimumDamage;
        maximumDamage = _maximumDamage;
        criticalStrikeChance = _criticalStrikeChance;
        criticalStrikeDamage = _criticalStrikeDamage;
        statusLength = _statusLength;
        enemyNamePlate = _enemyNamePlate;
        currentEnchant = _enchant;
    }

    private void Update()
    {
        if (target)
            transform.LookAt(target);

        smoothTime += speed * Time.deltaTime;

        if (target)
            transform.position = Vector3.Lerp(transform.position, target.transform.position, smoothTime);
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            int randomDamage = Random.Range(minimumDamage, maximumDamage);
            other.GetComponent<Health>().TookDamage(randomDamage, criticalStrikeChance, criticalStrikeDamage, enemyNamePlate);

            RaycastHit[] hit = Physics.SphereCastAll(target.transform.position, explosiveRange, transform.position, 0, targetable);

            for (int i = 0; i < hit.Length; i++)
            {
                PushBack(hit[i].transform.gameObject);

                int randomExplosiveDamage = Random.Range(explosiveMinDamage, explosiveMaxDamage);
                hit[i].transform.GetComponent<Health>().TookDamage(randomExplosiveDamage, criticalStrikeChance, criticalStrikeDamage, enemyNamePlate);
            }

            switch (currentEnchant)
            {
                case Ability.Enchant.None:
                    break;
                case Ability.Enchant.Dot:
                    other.GetComponent<EnchantEffects>().Dot(minimumDamage, maximumDamage, criticalStrikeChance, criticalStrikeDamage, enemyNamePlate, statusLength);
                    break;
                case Ability.Enchant.Slow:
                    other.GetComponent<EnchantEffects>().Slow(statusLength);
                    break;
                case Ability.Enchant.Weighted:
                        PushBack(other.gameObject);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
