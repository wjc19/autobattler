using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CallTeamData : MonoBehaviour
{
    [SerializeField] LevelController levelController;
    [SerializeField] TeamBuilder teamBuidler;
    string teamQueryResults;
    Dictionary<int, string[]> teamData = new Dictionary<int,string[]>();
    public string idName;

    public void CallGetTeam(string playerID)
    {
        StartCoroutine(GetTeam(playerID));
    }

    IEnumerator GetTeam(string playerID)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("PlayerID", idName);
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/getteam.php", form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("team data failed to save");

        }
        else
        {
            
            teamQueryResults = www.downloadHandler.text;
            Debug.Log(teamQueryResults);
        }
        

        string[] teamMemberStrings = teamQueryResults.Split('\n');
        teamMemberStrings = teamMemberStrings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        Debug.Log("Team member strings length = " + teamMemberStrings.Length);

        foreach (var member  in teamMemberStrings)
        {
            Debug.Log(member);
            var memberStatStrings = member.Split(',');
            teamBuidler.InstantiateEnemyTeamForBattle(Int32.Parse(memberStatStrings[0]), memberStatStrings[1], Int32.Parse(memberStatStrings[2]), Int32.Parse(memberStatStrings[3]), memberStatStrings[4]);
        }
        www.Dispose();
    }

    public void VerifyInputs()
    {
        // submit button logic
    }

        public void SetID(string name) 
    {
        idName = name;
    }
}
