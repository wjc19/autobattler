using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamhainShop : MonoBehaviour
{
    [SerializeField] public GameObject[] shopCharacters = new GameObject[5];
    [SerializeField] public GameObject[] shopItems = new GameObject[3];

    [SerializeField] Transform[] characterShopPositions = new Transform[5];
    [SerializeField] Transform[]  itemShopPositions = new Transform[3];

    // List<GameObject> classList = new List<GameObject>();
    LevelController levelController;
    [SerializeField] TeamBuilder teamBuilder;
    public static event Action<Item> updateShopCostForItem;
    public static event Action<Character> updateShopCostForCharacter;

    private void Start() {
        // classList = teamBuilder.ReturnClassPrefabList();
        levelController = FindObjectOfType<LevelController>();
        DragAndDrop.onCharacterPurchased += HandlePurchasedCharacter;
        DragAndDrop.onItemAssigned += HandlePurchasedItem;

        // Debug.Log(teamBuilder.classList.Count);
        HandleSpawning(shopCharacters, characterShopPositions, teamBuilder.classList);
        HandleItemSpawning(shopItems, itemShopPositions, teamBuilder.itemList);
    }

    private void OnDisable() {
        DragAndDrop.onCharacterPurchased -= HandlePurchasedCharacter;
        DragAndDrop.onItemAssigned -= HandlePurchasedItem;
    }

    public void HandleSpawning(GameObject[] teamEnumValues, Transform[] spawnTransforms, List<GameObject> list)
    {
        
        Debug.Log("handle character spawning");
        for (int i = 0; i < teamEnumValues.Length; i++)
        {
            // var candidateMember = (CharacterClasses)Enum.Parse(typeof(CharacterClasses), Enum.GetName(typeof(CharacterClasses), UnityEngine.Random.Range(0, Enum.GetValues(typeof(CharacterClasses)).Length -1)));
            var values = UnityEngine.Random.Range(0, Enum.GetValues(typeof(CharacterClasses)).Length);
            Debug.Log(values);
            Debug.Log(Enum.GetValues(typeof(CharacterClasses)).Length);
            var characterMember = Instantiate(list[values], spawnTransforms[i].position, Quaternion.identity, spawnTransforms[i].transform);
            shopCharacters[i] = characterMember;
            spawnTransforms[i].GetComponent<ShopPosition>().costText.text = "Cost: " + characterMember.GetComponent<Character>().cost.ToString();
            
        }
    }
    public void HandleItemSpawning(GameObject[] itemEnumValues, Transform[] spawnTransforms, List<GameObject> list)
    {

        Debug.Log("handle item spawning");
        for (int i = 0; i < itemEnumValues.Length; i++)
        {
            var enumCount = Enum.GetValues(typeof(Items)).Length - 1;
            var values = UnityEngine.Random.Range(0, enumCount);
            var itemMember = Instantiate(list[values], spawnTransforms[i].position, Quaternion.identity, spawnTransforms[i].transform);
            shopItems[i] = itemMember;
            spawnTransforms[i].GetComponent<ShopPosition>().costText.text = "Cost: " + itemMember.GetComponent<Item>().cost.ToString();
        }
    }

    public void Reroll()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (shopItems[i] == null || shopItems[i].GetComponentInParent<Character>())
            {
                continue;
            }
            Destroy(shopItems[i].gameObject);
            shopItems[i] = null;

        }
        for (int i = 0; i < shopCharacters.Length; i++)
        {
            if (shopCharacters[i] == null)
            {
                continue;
            }
            else if (shopCharacters[i].GetComponent<Character>().isTeamMember)
            {
                shopCharacters[i] = null;
                continue;
            }
            else
            {
                Destroy(shopCharacters[i].gameObject);
                shopCharacters[i] = null;
            }
        }
        HandleSpawning(shopCharacters, characterShopPositions, teamBuilder.classList);
        HandleItemSpawning(shopItems, itemShopPositions, teamBuilder.itemList);
    }

    private void HandlePurchasedCharacter(Character character)
    {
        for (int i = 0; i < shopCharacters.Length; i++)
        {
            if (shopCharacters[i] == null)
            {
                continue;
            }
            if (shopCharacters[i].gameObject == character.gameObject)
            {
                shopCharacters[i] = null;
                return;
            }
        }
    }
    
    private void HandlePurchasedItem(Character character, Item purchaseditem) 
    {
        for (int i = 0; i < shopCharacters.Length; i++)
        {
            if (shopItems[i] == null)
            {
                continue;
            }
            if (shopItems[i].gameObject == purchaseditem.gameObject)
            {
                shopItems[i] = null;
                return;
            }
        }
    }
}
