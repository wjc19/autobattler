using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TeamBuilder : MonoBehaviour
{
    [Header("")]
    [SerializeField] GameObject team;
    [SerializeField] public Transform[] teamTransformArray = new Transform[5];
    [SerializeField] public Character[] teamArray = new Character[5];
    [SerializeField] public Character[] enemyTeamArray = new Character[5];
    [SerializeField] public List<GameObject> classList = new List<GameObject>();
    [SerializeField] public List<GameObject> itemList = new List<GameObject>();
    [SerializeField] Button button;
    public static event Action<Character, Item> onItemAssigned;
    public static event Action<TeamBuilder> onTeamBuilderStart;
    public static bool teamIsFull;
    
    LevelController levelController;
    // Dictionary<Character, Dictionary<string, int>> teamTable = new Dictionary<Character, Dictionary<string, int>>();

    private void Start()
    {
        levelController = FindObjectOfType<LevelController>();
        // DragAndDrop.onDrag += CallPositionTeam;
        DragAndDrop.onCharacterDrop += CallPositionTeam;
        onTeamBuilderStart?.Invoke(this);
        // DragAndDrop.onItemDrop += Something
        GetTeamFromLevelController();
    }

    private void OnDisable()
    {
        // DragAndDrop.onDrag -= CallPositionTeam;
        // DragAndDrop.onItemDrop += Something
        DragAndDrop.onCharacterDrop -= CallPositionTeam;
    }

    public void HandleRemove(Character character, int destination)
    {
        teamArray[destination] = null;
        Debug.Log("Remove handled from slot " + destination);
    }

    public void HandleInsert(Character character, int destinationIndex)
    {
        Debug.Log(destinationIndex);

        if (teamArray[destinationIndex] == null)
        {
            teamArray[destinationIndex] = character;
            Debug.Log("team addition handled for slot " + destinationIndex);
            PositionTeam();
            return;
        }

        var emptyIndex = FindNearestEmptySpace(destinationIndex);
        HandleCharacterRotation(character, destinationIndex, emptyIndex);
    }

    private void HandleCharacterRotation(Character character, int destinationIndex, int emptyIndex)
    {
        if (teamArray[destinationIndex] != null && teamArray[emptyIndex] == null)
        {
            if (Mathf.Abs(destinationIndex - emptyIndex) == 1)
            {
                teamArray[emptyIndex] = teamArray[destinationIndex];
                teamArray[destinationIndex] = character;

            }

            else if (destinationIndex > emptyIndex)
            {
                for (int i = emptyIndex; i < destinationIndex; i++)
                {
                    teamArray[i] = teamArray[i + 1];
                }
                teamArray[destinationIndex] = character;
            }
            else 
            {
                for (int i = emptyIndex; i > destinationIndex; i--)
                {
                    teamArray[i] = teamArray[i - 1];
                }
                teamArray[destinationIndex] = character;
            }
            PositionTeam();
            Debug.Log("team addition handled for slot " + destinationIndex);
            return;
        }
    }

    private void PositionTeam()
    {
        for (int i = 0; i < teamArray.Length; i++)
        {
            if (teamArray[i] == null)
            {
                continue;
            }
            teamArray[i].transform.SetParent(team.transform);
            teamArray[i].transform.position = teamTransformArray[i].transform.position;
        }
    }

    public int FindNearestEmptySpace(int destination)
    {
        int shortestDistance;
        int closestIndex = 5;
        List<int> emptySpaces = new List<int>();
        for (int i = 0; i < teamArray.Length; i++)
        {
            if (teamArray[i] == null)
            {
                emptySpaces.Add(i);
            }
        }
        if (emptySpaces.Count == 0)
        {
            teamIsFull = true;
        }
        else
        {
            teamIsFull = false;
        }
         
        shortestDistance = Mathf.Abs(destination - emptySpaces[0]);
        for (int i = 0; i < emptySpaces.Count; i++)
        {
            Mathf.Abs(destination - emptySpaces[i]);
            if (Mathf.Abs(destination - emptySpaces[i]) <= shortestDistance)
            {
                shortestDistance = Mathf.Abs(destination - emptySpaces[i]);
                closestIndex = emptySpaces[i];
            } 
        }
        return closestIndex;
    }

    private void CallPositionTeam(Character obj)
    {
        PositionTeam();
    }

    public void BuildTeam()
    {
        StartCoroutine(BuildTeamCoroutine());
    }

    private IEnumerator BuildTeamCoroutine()
    {
    
        levelController.GetComponent<FirebaseManager>().SaveTeamData(teamArray);
        levelController.GetComponent<FirebaseManager>().StartLoadUserData();
        button.interactable = false; 
        yield return new WaitForSeconds(1f);
        levelController.SetTeam(teamArray, enemyTeamArray);

    }

    public void InstantiateEnemyTeamForBattle(int teamPosition, string characterName, int damage, int health, string itemName = null)
    {
        int classInt = (int) Enum.Parse(typeof(CharacterClasses), characterName);
        
        var enemy = Instantiate(classList[classInt - 1], levelController.enemyTeamObject.transform.position, Quaternion.identity, levelController.enemyTeamObject.transform);
        
        enemyTeamArray[teamPosition] = enemy.GetComponent<Character>(); 
        enemyTeamArray[teamPosition].damage = damage;
        enemyTeamArray[teamPosition].health = health;
        if (!string.IsNullOrEmpty(itemName))
        {
            Debug.Log("spawning items");
            int itemInt = (int)Enum.Parse(typeof(Items),itemName);
            Debug.Log(itemInt);
            var item = Instantiate(itemList[itemInt - 1], enemyTeamArray[teamPosition].itemPosition.transform.position, Quaternion.identity, enemyTeamArray[teamPosition].transform);
            item.GetComponent<Oscillator>().enabled = false;
            enemyTeamArray[teamPosition].GetComponent<Character>().equippedItem = item.GetComponent<Item>();   
            onItemAssigned?.Invoke(enemyTeamArray[teamPosition].GetComponent<Character>(), item.GetComponent<Item>());        
        }       
    }

    private void GetTeamFromLevelController()
    {
        for (int i = 0; i < teamArray.Length; i++)
        {
            if (levelController.friendlyTeamStatic[i] == null)
            {
                continue;
            }
            teamArray[i] = levelController.friendlyTeamStatic[i];
            levelController.friendlyTeamStatic[i].transform.SetParent(teamTransformArray[i]);
            PositionTeam();
        }
    }
}
