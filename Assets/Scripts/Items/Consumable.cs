using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Get.AddItem(this);
    }
    public enum ItemType
    {
        HealthPotion,
        Fireball,
        ScrollOfConfusion
    }
    [SerializeField]
    private ItemType type;

    public ItemType Type
    {
        get { return type; }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
