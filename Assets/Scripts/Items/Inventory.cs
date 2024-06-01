using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public List<Consumable> Items = new List<Consumable>();
    public int MaxItems = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool AddItem(Consumable item)
    {
        if (Items.Count < MaxItems)
        {
            Items.Add(item);
            Debug.Log($"{item.name} has been added to the inventory.");
            return true;
        }
        else
        {
            Debug.Log("Inventory is full. Cannot add item.");
            return false;
        }
    }
    public void DropItem(Consumable item)
    {
        Items.Remove(item);
    }
}
