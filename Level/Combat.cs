using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour , IDealDamage
{

    public static event Action<Character, Character> onAttack;

    public void Fight(Character friendlyFighter, Character enemyFighter)
    {
        // StartCoroutine(OnAttack());
        // onAttack?.Invoke(friendlyFighter, enemyFighter);
        // onAttack?.Invoke(enemyFighter, friendlyFighter);
        DealDamage(friendlyFighter, enemyFighter);
        Debug.Log(friendlyFighter + " " + enemyFighter);
        friendlyFighter.GetComponentInChildren<Animator>().SetTrigger("CombatAnimationTrigger");
        enemyFighter.GetComponentInChildren<Animator>().SetTrigger("EnemyAnimationTrigger");
    }

    public void DealDamage(Character friendly, Character enemy)
    {
        onAttack?.Invoke(friendly, enemy);
        friendly.TakeDamage(enemy.GetDamage(), enemy);
        enemy.TakeDamage(friendly.GetDamage(), enemy);
        friendly.GetComponentInChildren<Animator>().SetTrigger("IdleAnimationTrigger");
        enemy.GetComponentInChildren<Animator>().SetTrigger("IdleAnimationTrigger");
        Debug.Log("Dealing Damage");
    }
}