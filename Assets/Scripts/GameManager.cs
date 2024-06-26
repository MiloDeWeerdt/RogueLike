using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public List<Actor> Enemies = new List<Actor>();
    public Actor Player { get; set; }
    public List<Consumable> Items = new List<Consumable>();
    private List<Tombstone> tombstones = new List<Tombstone>();
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
    public void AddLadder(Ladder ladder)
    {
        ladders.Add(ladder);
    }

    public Ladder GetLadderAtLocation(Vector3 location)
    {
        foreach (Ladder ladder in ladders)
        {
            if (ladder.transform.position == location)
            {
                return ladder;
            }
        }
        return null;
    }

    public void AddTombStone(TombStone stone)
    {
        tombstones.Add(stone);
    }
    public void ClearFloor()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }
        items.Clear();

        foreach (var ladder in ladders)
        {
            Destroy(ladder.gameObject);
        }
        ladders.Clear();

        foreach (var stone in tombstones)
        {
            Destroy(stone.gameObject);
        }
        tombstones.Clear();
    }
    public void SavePlayerData()
    {
        if (Player != null)
        {
            var player = Player.GetComponent<Player>();
            PlayerPrefs.SetInt("MaxHitPoints", player.MaxHitPoints);
            PlayerPrefs.SetInt("HitPoints", player.HitPoints);
            PlayerPrefs.SetInt("Defense", player.Defense);
            PlayerPrefs.SetInt("Power", player.Power);
            PlayerPrefs.SetInt("Level", player.Level);
            PlayerPrefs.SetInt("XP", player.XP);
            PlayerPrefs.SetInt("XpToNextLevel", player.XpToNextLevel);
            PlayerPrefs.Save();
        }
    }

    public void LoadPlayerData()
    {
        if (Player != null)
        {
            var player = Player.GetComponent<Player>();
            player.MaxHitPoints = PlayerPrefs.GetInt("MaxHitPoints", 100); 
            player.HitPoints = PlayerPrefs.GetInt("HitPoints", 100);
            player.Defense = PlayerPrefs.GetInt("Defense", 10);
            player.Power = PlayerPrefs.GetInt("Power", 10);
            player.Level = PlayerPrefs.GetInt("Level", 1);
            player.XP = PlayerPrefs.GetInt("XP", 0);
            player.XpToNextLevel = PlayerPrefs.GetInt("XpToNextLevel", 100);
        }
    }

    public void DeletePlayerSave()
    {
        PlayerPrefs.DeleteKey("MaxHitPoints");
        PlayerPrefs.DeleteKey("HitPoints");
        PlayerPrefs.DeleteKey("Defense");
        PlayerPrefs.DeleteKey("Power");
        PlayerPrefs.DeleteKey("Level");
        PlayerPrefs.DeleteKey("XP");
        PlayerPrefs.DeleteKey("XpToNextLevel");
    }

    public void PlayerDied()
    {
        DeletePlayerSave();
    }

}
