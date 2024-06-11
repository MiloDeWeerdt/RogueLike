using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Healthbar : MonoBehaviour
{
    private VisualElement root;

    private VisualElement healthBar;

    private Label healthLabel;
    private Label levelLabel;
    private Label xpLabel;
    // Start is called before the first frame update
    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        healthBar = root.Q<VisualElement>("Healthbar");
        healthLabel = root.Q<Label>("HealthLabel");
        levelLabel = root.Q<Label>("LevelLabel");
        xpLabel = root.Q<Label>("XPLabel");
        if (healthBar == null)
        {
            Debug.LogError("HealthBar element is not found in the UXML!");
        }
        if (healthLabel == null)
        {
            Debug.LogError("HealthLabel element is not found in the UXML!");
        }
        if (levelLabel == null)
        {
            Debug.LogError("LevelLabel element is not found in the UXML!");
        }
        if (xpLabel == null)
        {
            Debug.LogError("XPLabel element is not found in the UXML!");
        }
        SetValues(30, 30);
    }
    public void SetValues(int currentHitPoints, int maxHitPoints)
    {
        float percent = (float)currentHitPoints / maxHitPoints * 100f;

        healthBar.style.width = new Length(percent, LengthUnit.Percent);

        healthLabel.text = $"{currentHitPoints}/{maxHitPoints} HP";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetLevel(int level)
    {
        levelLabel.text = "Level: " + level;
    }

    public void SetXP(int xp)
    {
        xpLabel.text = "XP: " + xp;
    }
}
