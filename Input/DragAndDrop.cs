using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DragAndDrop : MonoBehaviour, ITryBuy
{
    [SerializeField] TeamBuilder teamBuilder;

    public static event Action<Character> onCharacterSelect;
    public static event Action<Character> onDrag;
    public static event Action<Character> onCharacterDrop;
    public static event Action<Character> onCharacterDeselect;
    public static event Action<GameObject> onCharacterRefunded;
    public static event Action<Item> onItemSelect;
    public static event Action<Item> onItemDrag;
    public static event Action<Item> onItemDrop;
    public static event Action<Item> onItemDeselect;
    public static event Action<Character, Item> onItemAssigned;
    public static event Action<Character> onCharacterPurchased;



    public static bool crRunning;
    public static Character selectedCharacter;
    public static Item selectedItem;
    public static bool isDragging;
    private Vector3 originalItemPosition;
    private InputControls inputControls;    
    Vector2 currentCursorLocation;
    Vector3 originalCharacterPosition;
    private Player player;
    private bool isTeamMember;

    private void Awake() {
        inputControls = new InputControls();
        player = FindObjectOfType<Player>();

    }

    private void OnEnable() {
        inputControls.Enable();
        inputControls.Shop.Click.performed += Select;
        inputControls.Shop.Drop.performed += Drop;
        
    }

    private void OnDisable() {
        inputControls.Disable();
        inputControls.Shop.Click.performed -= Select;
        inputControls.Shop.Drop.performed -= Drop;  
    }

    private void Start() {
        inputControls.Shop.Click.performed += Select;
        inputControls.Shop.Drop.performed += Drop;
    }

    private void Update()
    {
        Dragging();
    }

    private void Dragging()
    {
        if (isDragging && selectedCharacter != null && selectedItem == null)
        {
            currentCursorLocation = Camera.main.ScreenToWorldPoint(inputControls.Shop.Position.ReadValue<Vector2>());
            selectedCharacter.transform.position = currentCursorLocation;
        }
        if (isDragging && selectedCharacter == null && selectedItem != null)
        {
            currentCursorLocation = Camera.main.ScreenToWorldPoint(inputControls.Shop.Position.ReadValue<Vector2>());
            selectedItem.transform.position = currentCursorLocation;
        }
    }

    private void Select(InputAction.CallbackContext obj)
    {
        var ray = Camera.main.ScreenPointToRay(inputControls.Shop.Position.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (hit.collider.GetComponent<Character>())
            {
                selectedCharacter = hit.collider.GetComponent<Character>();
                Debug.Log("Selected character is " +selectedCharacter.name);
                isDragging = true;

                originalCharacterPosition = selectedCharacter.transform.position;
                CheckCharacterSelection(selectedCharacter);

            }
            else if (hit.collider.GetComponent<Item>())
            {
                if (hit.collider.GetComponent<Item>().isPurchased)
                {
                    return;
                }
                selectedItem = hit.collider.GetComponent<Item>();
                isDragging = true;
                originalItemPosition = selectedItem.transform.position;
                CheckItemSelection(selectedItem);
                Debug.Log(" item selected");
            }
        }
        else 
        {
            Debug.Log("deselecting Character and item");
            // if (selectedCharacter != null)
            // {
                onCharacterDeselect?.Invoke(selectedCharacter);
                selectedCharacter = null;
            // }
            selectedItem = null;
        }
    }

    private void CheckCharacterSelection(Character character)
    {

        if (character.GetComponentInParent<ShopPosition>())
        {
            if (TryBuyCharacter(character, player))
            {
                Debug.Log("Is Shop character");
                onCharacterSelect?.Invoke(selectedCharacter);
                isDragging = true;
            }
            else
            {
                isDragging = false;
                selectedCharacter = null;
                Debug.Log("Too poor to buy this");
                // onCharacterDeselect?.Invoke(selectedCharacter);
            }

        }
        else if (character.GetComponentInParent<Team>())
        {
            Debug.Log("Is TeamMember");
            onCharacterSelect?.Invoke(selectedCharacter);
            isDragging = true;
        }
    }

    private void CheckItemSelection(Item item)
    {
        if (item.GetComponentInParent<ShopPosition>())
        {
            if (TryBuyItem(item, player))
            {
                Debug.Log("Is shop item");
                isDragging = true;
                
            }
            else 
            {
                isDragging = false;
                selectedItem = null;
                Debug.Log("too poor to buy item");
            }
        }
        else if (item.GetComponentInParent<TeamPosition>())
        {
            return;
            Debug.Log("Is on a assigned to team member");
            
        }
    }

    private void Drop(InputAction.CallbackContext obj)
    {
        if (selectedCharacter != null && selectedItem == null)
        {
            HandleCharacterDrop();
        }
        else if (selectedCharacter == null && selectedItem != null)
        {
            HandleItemDrop();
        }
        else 
        {
            return;
        }
    }

    private void HandleItemDrop()
    {
        isDragging = false;
        if (selectedItem == null)
        {
            return;
        }
        else if (!TeamPosition.isHoveringOverTeamPosition)
        {
            selectedItem.transform.position = originalItemPosition;
        }
        else if (TeamPosition.isHoveringOverTeamPosition && teamBuilder.teamArray[TeamPosition.hoveredPosition] != null)
        {
            player.Souls -= selectedItem.cost;
            AssignItemToCharacter();
        }
        selectedItem = null;
    }

    private void AssignItemToCharacter()
    {
        selectedItem.GetComponent<Oscillator>().enabled = false;
        selectedItem.transform.position = teamBuilder.teamArray[TeamPosition.hoveredPosition].itemPosition.transform.position;
        selectedItem.transform.SetParent(teamBuilder.teamArray[TeamPosition.hoveredPosition].transform);
        onItemAssigned?.Invoke(teamBuilder.teamArray[TeamPosition.hoveredPosition], selectedItem);
    }

    private void HandleCharacterDrop()
    {
        onCharacterDrop?.Invoke(selectedCharacter);
        isDragging = false;
        if (selectedCharacter == null)
        {
            return;
        }

        if (!selectedCharacter.isTeamMember && !TeamPosition.isHoveringOverTeamPosition)
        {
            for (int i = 0; i < teamBuilder.teamArray.Length; i++)
            {
                if (teamBuilder.teamArray[i] != null && selectedCharacter.gameObject == teamBuilder.teamArray[i].gameObject)
                {
                    teamBuilder.HandleRemove(selectedCharacter, i);
                    break;
                }
            }
            selectedCharacter.transform.position = originalCharacterPosition;
            Debug.Log(" condition 1 was called");
        }
        else if (selectedCharacter.isTeamMember && RefundPosition.isHoveringOverRefundPosition)
        {
       
           StartCoroutine(HandleCharacterRefund(selectedCharacter));

        }
        else if (TeamBuilder.teamIsFull && !selectedCharacter.isTeamMember)
        {
            selectedCharacter.transform.position = originalCharacterPosition;
        }
        else if (!selectedCharacter.isTeamMember && TeamPosition.isHoveringOverTeamPosition)
        {
            // teamBuilder.HandleInsert(selectedCharacter, TeamPosition.lastTeamPosition);
            StartCoroutine(HandlePurchasedCharacter(selectedCharacter));
        }
        else if (selectedCharacter.isTeamMember && teamBuilder.teamArray[TeamPosition.lastTeamPosition] == null)
        {
            teamBuilder.HandleInsert(selectedCharacter, TeamPosition.lastTeamPosition);
            Debug.Log("Last Team Position = " + TeamPosition.lastTeamPosition);
            Debug.Log(" condition 3 was called");
        }
        else if (selectedCharacter.isTeamMember && !TeamPosition.isHoveringOverTeamPosition)
        {
            Debug.Log(" condition 4 was called");
            teamBuilder.HandleInsert(selectedCharacter, teamBuilder.FindNearestEmptySpace(3));
        }
        
        Debug.Log("Deselecting character");
        onCharacterDeselect?.Invoke(selectedCharacter);
        selectedCharacter = null;
    }  


    private IEnumerator HandleCharacterRefund(Character character)
    {
            
             Debug.Log("Selling team member");
             yield return player.Souls += character.cost;
             onCharacterRefunded?.Invoke(character.gameObject);
    }

    private IEnumerator HandlePurchasedCharacter(Character character)
    {
            yield return character.isTeamMember = true;
            yield return player.Souls -= character.cost;
            onCharacterPurchased?.Invoke(character);
    }

    public bool TryBuyCharacter(Character character, Player player)
    {
        if (player.souls - character.cost >= 0)
        {
            return true;
        } 
        else
        {

            return false;
        }
    }

    public bool TryBuyItem(Item item, Player player)
    {
        if (player.souls - item.cost >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
    


