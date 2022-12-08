using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Character
{
    string test = "test";
    int fireDamage = 4;

    private void Awake() {
        Combat.onAttack += FireBreath;
    }
    private void OnDestroy()
    {
        Combat.onAttack -= FireBreath;
    }

        private void FireBreath(Character friendly, Character enemy)
    {
        Debug.Log("Firebreathing");
        LevelController levelController = FindObjectOfType<LevelController>();
        if (friendly.gameObject == this.gameObject && levelController.enemyTeam.Count > 1)
        {
            if  (levelController.enemyTeam[1] != null )
            {
                levelController.enemyTeam[1].TakeDamage(fireDamage, levelController.enemyTeam[1]);
            }
            else 
            {
                return;
            }
        }
        Debug.Log("Firebreathing");

        if (enemy.gameObject == this.gameObject && levelController.friendlyTeam.Count > 1)
        {
            if  (levelController.friendlyTeam[1] != null )
            {
                levelController.friendlyTeam[1].TakeDamage(fireDamage, levelController.friendlyTeam[1]);
            }
            else 
            {
                return;
            }
        }
    }

    // private void FireBreath(Character friendly, Character enemy)
    // {
    //     Debug.Log("Firebreathing");
    //     LevelController levelController = FindObjectOfType<LevelController>();
    //     if (friendly.gameObject == this.gameObject)
    //     {
    //         for (int i = 0; i < levelController.enemyTeam.Count; i++)
    //         {
    //             if  (levelController.enemyTeam[i] == enemy)
    //             {
    //                 if (i < levelController.enemyTeam.Count - 1)
    //                 {
    //                     if (levelController.enemyTeam[i + 1] != null)
    //                         {
    //                             levelController.enemyTeam[i+1].TakeDamage(fireDamage, levelController.enemyTeam[i+1]);
    //                             Debug.Log("Trying to deal damage to "+ levelController.enemyTeam[i+1]);
    //                         }
    //                 }
    //                 else 
    //                 {
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    //     if (enemy.gameObject == this.gameObject)
    //     {
    //         for (int i = 0; i < levelController.friendlyTeam.Count; i++)
    //         {
    //             if  (levelController.friendlyTeam[i] == friendly)
    //             {
    //                 if (i < levelController.friendlyTeam.Count - 1)
    //                 {
    //                     if (levelController.friendlyTeam[i + 1] != null)
    //                         {
    //                             levelController.friendlyTeam[i+1].TakeDamage(fireDamage, levelController.friendlyTeam[i+1]);
    //                             Debug.Log("Trying to deal damage to "+ levelController.friendlyTeam[i+1]);
    //                         }
    //                 }
    //                 else 
    //                 {
    //                     return;
    //                 }
    //             }
    //         }
    //     }
    // }
}
