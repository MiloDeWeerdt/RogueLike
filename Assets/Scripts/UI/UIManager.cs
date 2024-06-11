using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // GameObjects voor HealthBar en Messages
    [Header("Documents")]
    public GameObject healthBarObject;
    public GameObject messagesObject;
    public GameObject floorInfoObject;

    private Healthbar healthBar;
    private Messages messages;
    public GameObject inventory;
    public FloorInfo floorInfo;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public static UIManager Get { get => instance; }
    public InventoryUI InventoryUI
    {
        get => Inventory.GetComponent<InventoryUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<Healthbar>();
        }

        if (messagesObject != null)
        {
            messages = messagesObject.GetComponent<Messages>();
        }
        if (floorInfoObject != null)
        {
            floorInfo = floorInfoObject.GetComponent<FloorInfo>(); // Voeg dit toe
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealth(int current, int max)
    {
        if (healthBar != null)
        {
            healthBar.SetValues(current, max);
        }
    }
    public void UpdateLevel(int level)
    {
        if (healthBar != null)
        {
            healthBar.SetLevel(level);
        }
    }

    public void UpdateXP(int xp)
    {
        if (healthBar != null)
        {
            healthBar.SetXP(xp);
        }
    }
    // Voeg een bericht toe aan het berichten systeem
    public void AddMessage(string message, Color color)
    {
        if (messages != null)
        {
            messages.AddMessage(message, color);
        }
    }
    public void UpdateFloor(int floor)
    {
        if (floorInfo != null)
        {
            floorInfo.UpdateFloor(floor);
        }
    }

    public void UpdateEnemies(int enemyCount)
    {
        if (floorInfo != null)
        {
            floorInfo.UpdateEnemies(enemyCount);
        }
    }
}
