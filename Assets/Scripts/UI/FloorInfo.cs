using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorInfo : MonoBehaviour
{
    public Label floorLabel;
    public Label enemiesLabel;
    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        floorLabel = root.Q<Label>("Floor");
        enemiesLabel = root.Q<Label>("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateFloorInfo(string floorName, int enemiesLeft)
    {
        floorLabel.text = floorName;
        enemiesLabel.text = enemiesLeft + " enemies left";
    }
    
}
