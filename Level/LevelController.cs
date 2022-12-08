using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using UnityEngine.EventSystems;

public class LevelController : MonoBehaviour
{
    
    [SerializeField] SpawnLocations spawnLocations;
    [SerializeField] GameObject teamObject;
    [SerializeField] public GameObject enemyTeamObject;
    [SerializeField] GameObject holder;
 
    public List<Character> friendlyTeam = new List<Character>();
    public List<Character> enemyTeam = new List<Character>();
    private float combatDelay = 1f; 


    [SerializeField] public Character[] friendlyTeamStatic = new Character[5];
    public static event Action<Character> onRoundEnd;

    bool combatStarted;
    string player = "Player";
    string enemy = "Enemy";
    public int round = 1;
    Character activeFriendlyFighter;
    Character activeEnemyFighter;
    Combat combat;
    bool crRunning;

    private void Awake()
    {
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("LevelController");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    private void Start()
    {
        combat = GetComponent<Combat>();
        Character.onDeath += RemoveDeadTeamMember;
    }

    public void TryContinue()
    {
        if (!crRunning)
        {
            StartCoroutine(ContinueFighting());
        }
    }

    private IEnumerator ContinueFighting()
    {
        crRunning = true;
        yield return new WaitForSeconds(.5f);
        FindActiveFighter();
        crRunning = false;
    }

    private void RemoveDeadTeamMember(Character obj, Character attacker)
    {
        foreach (var character in friendlyTeam.ToList())
        {
            if (character == null)
            {
                continue;
            }
            else if (character.taggedForDeath == true)
            {
                friendlyTeam.Remove(character);
                Destroy(obj.gameObject);
            }
        }
        foreach (var character in enemyTeam.ToList())
        {
            if (character == null)
            {
                continue;
            }
            else if(character.taggedForDeath == true)
            {
                enemyTeam.Remove(character);
                Destroy(obj.gameObject);
            }
        }
        TryContinue();
    }

    public void FindActiveFighter()
    {
        activeFriendlyFighter = GetActiveFighter(friendlyTeam);
        activeEnemyFighter = GetActiveFighter(enemyTeam);
        if (activeEnemyFighter == null && activeFriendlyFighter != null)
        {
            StartCoroutine(ShopSceneLoad("Player wins round!"));
            round++; 
            Debug.Log("Beginning round " + round);
        }
        else if (activeEnemyFighter != null && activeFriendlyFighter == null)
        {
            StartCoroutine(ShopSceneLoad("Enemy wins round. :("));
             round++; 
             Debug.Log("Beginning round " + round);
        }
        else if (activeEnemyFighter == null & activeFriendlyFighter == null)
        {
            StartCoroutine(ShopSceneLoad("Round is a tie."));
            round++; 
            Debug.Log("Beginning round " + round);
        }
        else
        {
            StartCoroutine(HandleFightPosition());
        }     
    }

    private IEnumerator HandleFightPosition()
    {
        // for (int i = 0; i < friendlyTeam.Count; i++)
        // {
        //     if (friendlyTeam[i] == null)
        //     friendlyTeam.Remove(friendlyteam[i]);
        // }
        friendlyTeam.RemoveAll(s => s == null);
        for (int i = 0; i < friendlyTeam.Count; i++)
        {
            friendlyTeam[i].transform.position = spawnLocations.friendlySpawnLocations[i].transform.position;
        }

        // for (int i = 0; i < enemyTeam.Count; i++)
        // {
        //     if (enemyTeam[i] == null)
        //     enemyTeam.Remove(enemyTeam[i]);
        // }
        enemyTeam.RemoveAll(s => s == null);
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            enemyTeam[i].transform.position = spawnLocations.enemySpawnLocations[i].transform.position;
        }
        yield return new WaitForSeconds(combatDelay);
        combat.Fight(GetActiveFighter(friendlyTeam), GetActiveFighter(enemyTeam));
        Debug.Log("Fighting called");
    }

    private Character GetActiveFighter(List<Character> members)
    {
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i] != null)
            {
                return members[i];
            }
            continue;
        }       
        return null;
    }

    public void SetTeam(Character[] team, Character[] eTeam)
    {
        friendlyTeam = team.ToList();
        enemyTeam = eTeam.ToList();
        friendlyTeamStatic = team;

        foreach (var character in friendlyTeam)
        {
            if (character != null)
            {
                character.transform.SetParent(teamObject.transform);
            }
        }
        StartCoroutine(BattleSceneLoad());
    }    

    private IEnumerator BattleSceneLoad()
    {
        SceneControl.NextScene("Battle Scene");
        yield return null;
        PositionTeamOffScreen();
    }

    private void PositionTeamOffScreen()
    {
        spawnLocations = FindObjectOfType<BattleCanvas>().GetComponentInChildren<SpawnLocations>();
        for (int i = 0; i < friendlyTeam.Count; i++)
        {
            if (friendlyTeam[i] != null)
            {
                friendlyTeam[i].transform.position = holder.transform.position;
            }
        }
        PositionEnemyTeam();
        InstantiateFriendlyTeamForBattle();
    }


    private void PositionEnemyTeam()
    {
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            if (enemyTeam[i] == null)
            {
                continue;
            }
            enemyTeam[i].transform.position = spawnLocations.enemySpawnLocations[i].transform.position;
            enemyTeam[i].spriteRenderer.flipX = true;
            enemyTeam[i].transform.SetParent(FindObjectOfType<BattlePositions>().transform);
        }
    }
// friendly  team is copied and assigned to friendlyTeam list
    private void InstantiateFriendlyTeamForBattle()
    {
        for (int i = 0; i < friendlyTeam.Count; i++)
        {
            if (friendlyTeam[i] == null)
            {
                continue;
            }
            Character friendly = Instantiate(friendlyTeam[i], spawnLocations.friendlySpawnLocations[i].transform.position, Quaternion.identity, FindObjectOfType<BattlePositions>().transform);
            friendlyTeam[i] = friendly;
            friendlyTeam[i].transform.SetParent(FindObjectOfType<BattlePositions>().transform);

        }
    }

    private IEnumerator ShopSceneLoad(string message)
    {
        var battleCanvas = FindObjectOfType<BattleCanvas>();
        battleCanvas.victoryText.text = message;
        battleCanvas.victoryText.enabled = true;
        yield return new WaitForSeconds(5f);
        SceneControl.NextScene("Shop Scene");
    }
}
