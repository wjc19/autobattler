using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TeamSetDisplay : MonoBehaviour
{
    [SerializeField] List<Transform> positions = new List<Transform>();

    [SerializeField] TeamSetText accursedText;
    [SerializeField] TeamSetText hellText;
    [SerializeField] TeamSetText necromancyText;
    [SerializeField] TeamSetText faeText;

    [SerializeField] int accursedCount;
    [SerializeField] int hellCount;
    [SerializeField] int necromancyCount;
    [SerializeField] int faeCount;

    [SerializeField] TeamBuilder teamBuilder;

    // Start is called before the first frame update
    void Start()
    {
        DragAndDrop.onCharacterPurchased += HandleTeamSetIncrease;
        DragAndDrop.onCharacterRefunded += HandleTeamSetDecrease;
    }
    
    void OnDisable()
    {
        DragAndDrop.onCharacterPurchased -= HandleTeamSetIncrease;
        DragAndDrop.onCharacterRefunded -= HandleTeamSetDecrease;
    }

    private void HandleTeamSetIncrease(Character character)
    {
        teamBuilder = FindObjectOfType<TeamBuilder>();
        for (int i = 0; i < teamBuilder.teamArray.Length; i++)
        {
            if (teamBuilder.teamArray[i] == null || teamBuilder.teamArray[i].gameObject == character.gameObject)
            {
                continue;
            }
            if (teamBuilder.teamArray[i].characterClass == character.characterClass)
            {
                return;
            }
        }
        if (character.GetComponent<CharacterSet>() != null)
        {
            if (character.classSetName == "Accursed")
            {
                accursedText.TeamSetCount += 1;
            }
            if (character.classSetName == "Hell")
            {
                hellText.TeamSetCount += 1;
            }
            if (character.classSetName == "Necromancy")
            {
                necromancyText.TeamSetCount += 1;
            }
            if (character.classSetName == "Fae")
            {
                faeText.TeamSetCount += 1;
            }
            UpdateCharacterSetText();
        }
    }

    private void HandleTeamSetDecrease(GameObject character)
    {
        teamBuilder = FindObjectOfType<TeamBuilder>();
        for (int i = 0; i < teamBuilder.teamArray.Length; i++)
        {
            if (teamBuilder.teamArray[i] == null)
            {
                continue;
            }
            if (teamBuilder.teamArray[i].characterClass == character.GetComponent<Character>().characterClass)
            {
                return;
            }
        }
        if (character.GetComponent<CharacterSet>() != null)
        {
            if (character.GetComponent<Character>().classSetName == "Accursed")
            {
                accursedText.TeamSetCount -= 1;
            }
            if (character.GetComponent<Character>().classSetName == "Hell")
            {
                hellText.TeamSetCount -= 1;
            }
            if (character.GetComponent<Character>().classSetName == "Necromancy")
            {
                necromancyText.TeamSetCount  -= 1;
            }
            if (character.GetComponent<Character>().classSetName == "Fae")
            {
                faeText.TeamSetCount -= 1;
            }
            UpdateCharacterSetText();
        }
    }

    private void UpdateCharacterSetText()
    {
        List<TeamSetText> teamSetCounts = new List<TeamSetText>();

        teamSetCounts.Add(accursedText);
        teamSetCounts.Add(faeText);
        teamSetCounts.Add(hellText);
        teamSetCounts.Add(necromancyText);

        var sortedTeamSets = teamSetCounts.OrderByDescending(x => x.teamSetCount).ToList();     

        for (int i = 0; i < sortedTeamSets.Count; i++)
        {
            sortedTeamSets[i].transform.position = positions[i].transform.position;
            if (sortedTeamSets[i].teamSetCount > 0)
            {
                sortedTeamSets[i].setTextGroup.alpha = 1;
            }
            else
            {
                sortedTeamSets[i].setTextGroup.alpha = 0;
            }
        }
    }

}
