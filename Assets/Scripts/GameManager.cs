using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<Actor> Enemies = new List<Actor>();
    public Actor Player { get; set; }
    public List<Consumable> Items = new List<Consumable>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }
    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (var item in Items)
        {
            if (item != null && item.transform.position == location)
            {
                return item;
            }
        }
        return null;
    }
    public void RemoveItem(Consumable item)
    {
        if (Items.Contains(item))
        {
            Items.Remove(item);
            Destroy(item.gameObject);
            Debug.Log($"{item.name} has been removed.");
        }
        else
        {
            Debug.Log("Item not found in the list.");
        }
    }
    public static GameManager Get { get => instance; }

    public Actor GetActorAtLocation(Vector3 location)
    {
        if (location == Player.transform.position)
        {
            return Player;
        }
        else
        {
            foreach (Actor enemy in Enemies)
            {
                if (location == enemy.transform.position)
                {
                    return enemy;
                }
            }
            return null;
        }
    }
    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        if (name == "Player")
        {
            Player = actor.GetComponent<Actor>();
        }
        else
        {
            AddEnemy(actor.GetComponent<Actor>());
        }
        actor.name = name;
        return actor;
    }
    public void StartEnemyTurn()
    {
        foreach (Actor enemy in Enemies)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.RunAI();
            }
        }
    }
    public void RemoveEnemy(Actor enemy)
    {
        if (Enemies.Contains(enemy))
        {
            Enemies.Remove(enemy);
            Debug.Log($"{enemy.name} has been removed.");
        }
        else
        {
            Debug.Log("Enemy not found in the list.");
        }
    }
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        foreach (Actor enemy in Enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, location);
            if (distance < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        return nearbyEnemies;
    }
}
