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

    private Healthbar healthBar;
    private Messages messages;

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

    // Voeg een bericht toe aan het berichten systeem
    public void AddMessage(string message, Color color)
    {
        if (messages != null)
        {
            messages.AddMessage(message, color);
        }
    }
}
