using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private int width, height;
    private int maxRoomSize, minRoomSize;
    private int maxRooms;
    List<Room> rooms = new List<Room>();
    public int maxEnemies;
    private int maxItems;
    private int currentFloor = 0;
    private List<string> enemyNames = new List<string>() { "ant", "bat", "cat", "chicken", "devil", "dog", "dwarf", "hog", "under,worm"; }
    public void SetSize(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetRoomSize(int min, int max)
    {
        minRoomSize = min;
        maxRoomSize = max;
    }

    public void SetMaxRooms(int max)
    {
        maxRooms = max;
    }
    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }
    public void SetMaxItems(int maxItems)
    {
        this.maxItems = maxItems;
    }
    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
    }
    public void Generate()
    {
        rooms.Clear();

        for (int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);

            int roomX = Random.Range(0, width - roomWidth - 1);
            int roomY = Random.Range(0, height - roomHeight - 1);

            var room = new Room(roomX, roomY, roomWidth, roomHeight);

            // if the room overlaps with another room, discard it
            if (room.Overlaps(rooms))
            {
                continue;
            }

            // add tiles make the room visible on the tilemap
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    if (x == roomX
                        || x == roomX + roomWidth - 1
                        || y == roomY
                        || y == roomY + roomHeight - 1)
                    {
                        if (!TrySetWallTile(new Vector3Int(x, y)))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        SetFloorTile(new Vector3Int(x, y, 0));
                    }

                }
            }

            // create a coridor between rooms
            if (rooms.Count != 0)
            {
                TunnelBetween(rooms[rooms.Count - 1], room);
            }
            
                PlaceEnemies(room, maxEnemies);
            PlaceItems(room, maxItems);
            rooms.Add(room);
        }
        var player = GameManager.Get.CreateActor("Player", rooms[0].Center());
        if (currentFloor > 0)
        {
            PlaceLadderDown(ladderPos);
        }
        if (currentFloor > 0)
        {
            var firstRoom = rooms[0];
            Vector2Int ladderUpPos = new Vector2Int(firstRoom.Center().x, firstRoom.Center().y);
            PlaceLadderUp(ladderUpPos);
        }
    }
    private void PlaceItems(Room room, int maxItems)
    {
        int numItems = Random.Range(0, maxItems + 1);

        for (int i = 0; i < numItems; i++)
        {
            int x = Random.Range(room.X + 1, room.X + room.Width - 1);
            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);

            GameObject itemPrefab = null;

            // Kies willekeurig welk item te plaatsen
            float randomValue = Random.value;
            if (randomValue < 0.33f)
            {
                itemPrefab = Resources.Load<GameObject>("Prefabs/HealthPotion");
            }
            else if (randomValue < 0.66f)
            {
                itemPrefab = Resources.Load<GameObject>("Prefabs/Fireball");
            }
            else
            {
                itemPrefab = Resources.Load<GameObject>("Prefabs/ScrollOfConfusion");
            }

            Instantiate(itemPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }
    }

    private bool TrySetWallTile(Vector3Int pos)
    {
        // if this is a floor, it should not be a wall
        if (MapManager.Get.FloorMap.GetTile(pos))
        {
            return false;
        }
        else
        {
            // if not, it can be a wall
            MapManager.Get.ObstacleMap.SetTile(pos, MapManager.Get.WallTile);
            return true;
        }
    }

    private void SetFloorTile(Vector3Int pos)
    {
        // this tile should be walkable, so remove every obstacle
        if (MapManager.Get.ObstacleMap.GetTile(pos))
        {
            MapManager.Get.ObstacleMap.SetTile(pos, null);
        }
        // set the floor tile
        MapManager.Get.FloorMap.SetTile(pos, MapManager.Get.FloorTile);
    }

    private void TunnelBetween(Room oldRoom, Room newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCorner;

        if (Random.value < 0.5f)
        {
            // move horizontally, then vertically
            tunnelCorner = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        else
        {
            // move vertically, then horizontally
            tunnelCorner = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }

        // Generate the coordinates for this tunnel
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BresenhamLine.Compute(oldRoomCenter, tunnelCorner, tunnelCoords);
        BresenhamLine.Compute(tunnelCorner, newRoomCenter, tunnelCoords);

        // Set the tiles for this tunnel
        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            SetFloorTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y));

            for (int x = tunnelCoords[i].x - 1; x <= tunnelCoords[i].x + 1; x++)
            {
                for (int y = tunnelCoords[i].y - 1; y <= tunnelCoords[i].y + 1; y++)
                {
                    if (!TrySetWallTile(new Vector3Int(x, y, 0)))
                    {
                        continue;
                    }
                }
            }
        }
    }
    private void PlaceEnemies(Room room, int maxEnemies)

    {

        // the number of enemies we want 

        int num = Random.Range(0, maxEnemies + 1);



        for (int counter = 0; counter < num; counter++)

        {

            // The borders of the room are walls, so add and substract by 1 

            int x = Random.Range(room.X + 1, room.X + room.Width - 1);

            int y = Random.Range(room.Y + 1, room.Y + room.Height - 1);



            // create different enemies 

            int maxIndex = Mathf.Clamp(currentFloor, 0, enemyNames.Count - 1);
            int enemyIndex = Random.Range(0, maxIndex + 1);

            string enemyName = enemyNames[enemyIndex];
            GameManager.Get.CreateActor(enemyName, new Vector2(x, y));

        }

    }
    private void PlaceLadderDown(Vector2Int position)
    {
        GameObject ladderPrefab = Resources.Load<GameObject>("Prefabs/LadderDown");
        Instantiate(ladderPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
    }

    private void PlaceLadderUp(Vector2Int position)
    {
        GameObject ladderPrefab = Resources.Load<GameObject>("Prefabs/LadderUp");
        Instantiate(ladderPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
    }
}
