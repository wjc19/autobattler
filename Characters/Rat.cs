using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Character
{

    private void Awake() 
    {
        Character.onDeath += StinkyCloud;
    }
    private void OnDestroy()
    {
        Character.onDeath -= StinkyCloud;
    }

    private void StinkyCloud(Character friendly, Character attacker)
    {
        LevelController levelController = FindObjectOfType<LevelController>();
        if (friendly.gameObject == this.gameObject)
        {
            if (friendly.isTeamMember)
            {
                for (int i = 0; i < levelController.enemyTeam.Count; i++)
                {
                    levelController.enemyTeam[i].isStinky = true;
                }
            }
            else
            {
                for (int i = 0; i < levelController.friendlyTeam.Count; i++)
                {
                    levelController.friendlyTeam[i].isStinky = true;
                }
            }
        }
    }
}
