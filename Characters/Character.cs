using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using System;

public class Character : MonoBehaviour, IDraggable
{
    

    public static event Action<Character, Character> onDeath;
    public SnapController snapController { get; set; }
    public bool taggedForDeath = false;
    public bool isTeamMember = false;
    public bool isStinky = false;
    
    [SerializeField] public string classSetName;
    [SerializeField] public string characterClass;
    [SerializeField] public Transform itemPosition;
    [SerializeField] public SpriteRenderer spriteRenderer;
    
    public Action<int> onDamageChanged;

    public int Damage
    {
        get { return damage; } 
        set
        {
            damage = value;
            onDamageChanged.Invoke(damage);
            // Debug.Log(this.name);
            // Debug.Log(damage);
        }
    }

    public Action<int> onHealthChanged;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            onHealthChanged.Invoke(health);
        }
    }
    

    [Header("Combat Stats")]
    [SerializeField] public int damage; 
    [SerializeField] public int health;
    [SerializeField] public int armor;
    [Header("Experience")]
    [SerializeField] int level;
    [SerializeField] int experience;
    [SerializeField] const int maxExperience = 6;
    [Header("Cost")]
    [SerializeField] public int cost;
    [SerializeField] int salePrice; 
    [Header("Equipped Item")]
    [SerializeField] public Item equippedItem;

    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI healthText;
    LevelController levelController;    


    private void Start() 
    {
        levelController = FindObjectOfType<LevelController>();

        UpdateHealthText(health);
        UpdateDamageText(damage);

        this.onHealthChanged += UpdateHealthText;
        this.onDamageChanged += UpdateDamageText;
        DragAndDrop.onItemAssigned += HandleItemAssignment;
        DragAndDrop.onCharacterRefunded += HandleCharacterRefund;
        // TeamBuilder.onItemAssigned += HandleItemAssignment;

        DragAndDrop.onCharacterSelect += StopAnimators;
        // DragAndDrop.onCharacterDeselect += StartAnimators;
    }
    
    public void StartAnimators(Character character)
    {
        // GetComponent<Animator>().enabled = true;
    }

    public void StopAnimators(Character character)
    {
        // GetComponent<Animator>().enabled = false;
    }

    private void HandleItemAssignment(Character character, Item item)
    {
    //     if (equippedItem != null)
    //     {
    //         equippedItem.HandleItem(this, equippedItem);
    }

    private void OnDisable() {
        this.onHealthChanged -= UpdateHealthText;
        this.onDamageChanged -= UpdateDamageText;
        DragAndDrop.onCharacterSelect -= StopAnimators ;
        DragAndDrop.onCharacterDeselect -= StartAnimators;
        DragAndDrop.onItemAssigned -= HandleItemAssignment;     
        TeamBuilder.onItemAssigned -= HandleItemAssignment;
        DragAndDrop.onCharacterRefunded -= HandleCharacterRefund;
    }

    private void HandleCharacterRefund(GameObject obj)
    {
        Debug.Log(this.gameObject.name);
        if (obj == gameObject)
        {
             Destroy(this.gameObject);
        }
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = health.ToString();
    }

    public void UpdateDamageText(int damage)
    {
        damageText.text = damage.ToString();
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetDamage(int newDamage)
    {
        damage += newDamage;
    }

    public void SetHealth(int newHealth)
    {
        health += newHealth;
    }

    IEnumerator Checkfordeath(Character attacker)
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(.5f);
        if (health <= 0)
        {
            Debug.Log("C OnDeath");
            taggedForDeath = true;
            Material material = spriteRenderer.material;
            onDeath?.Invoke((gameObject.GetComponent<Character>()), attacker);
        }
        else
        {
            levelController.TryContinue();
        }
    }

    public void TakeDamage(int damage, Character attacker)
    {
        Debug.Log(gameObject.name + " taking damage");
        health -= (damage - armor);
        UpdateHealthText(health);
        StartCoroutine(Checkfordeath(attacker));
    }

    public void DragOnCursur()
    {
        // if scene is Shop && clickedon
    }
}
