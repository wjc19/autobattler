using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Registration : MonoBehaviour
{
    [SerializeField] TeamBuilder teamBuilder;
    public string idName;

// TODO - update coroutine to include imported ID
    public void CallRegister(string playerID)
    {
        StartCoroutine(Register(idName));
    }

    IEnumerator Register(string playerID)
    {
        
        Debug.Log("Storing team in database");
        for (int i = 0; i < teamBuilder.teamArray.Length; i++)
        {
            if (teamBuilder.teamArray[i] == null)
            {
                continue;
            }
            WWWForm form = new WWWForm();
            form.AddField("PlayerID", idName);
            form.AddField("TeamPosition", i);
            form.AddField("CharacterClass", teamBuilder.teamArray[i].characterClass);
            form.AddField("Health", teamBuilder.teamArray[i].Health);
            form.AddField("Damage", teamBuilder.teamArray[i].Damage);
            if (teamBuilder.teamArray[i].equippedItem != null)
            {
                form.AddField("EquippedItem", teamBuilder.teamArray[i].equippedItem.itemClass);
            }
            else 
            {
                form.AddField("EquippedItem", "");
            }
            UnityWebRequest www = UnityWebRequest.Post("http://localhost/sqlconnect/register.php", form);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("team data failed to save");

            }
            else
            {
                Debug.Log("Received: " + www.downloadHandler.text);
            }
            www.Dispose();
        }
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
