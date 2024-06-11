using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private bool inventoryIsOpen = false;
    private bool droppingItem = false;
    private bool usingItem = false;

    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();
    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (context.performed)
            {
                if (inventoryIsOpen)
                {
                    Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
                    if (direction.y > 0)
                    {
                        inventoryUI.SelectPreviousItem();
                    }
                    else if (direction.y < 0)
                    {
                        inventoryUI.SelectNextItem();
                    }
                }
                else
                {
                    Move();
                }
            }
        }
    }

    public void OnExit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (inventoryIsOpen)
            {
                inventoryUI.Hide();
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }
    public void OnGrab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 playerPosition = transform.position;
            Consumable item = GameManager.Get.GetItemAtLocation(playerPosition);

            if (item == null)
            {
                UIManager.Instance.AddMessage("Er is geen item om op te rapen.", Color.yellow);
                return;
            }

            if (inventory.IsFull())
            {
                UIManager.Instance.AddMessage("Je inventaris is vol. Je kunt dit item niet oppakken.", Color.red);
                return;
            }
            inventory.AddItem(item);
            item.gameObject.SetActive(false);
            GameManager.Get.RemoveItem(item);
            Debug.Log($"je hebt het item opgerapen en toegevoegd aan je inventaris");

        }
    }
    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                inventoryUI.Show(inventory.Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                inventoryUI.Show(inventory.Items);
                inventoryIsOpen = true;
                usingItem = true;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed && inventoryIsOpen)
        {
            Consumable selectedItem = inventory.Items[inventoryUI.Selected];

            if (droppingItem)
            {
                inventory.DropItem(selectedItem);
                selectedItem.transform.position = transform.position;
                GameManager.Get.AddItem(selectedItem);
                selectedItem.gameObject.SetActive(true);
            }
            else if (usingItem)
            {
                UseItem(selectedItem);
                Destroy(item.gameObject);
            }

            inventoryUI.Hide();
            inventoryIsOpen = false;
            droppingItem = false;
            usingItem = false;
        }
    }
    private void UseItem(Consumable item)
    {
        if (inventoryIsOpen)
        {
            if (item is HealthPotion)
            {
                HealthPotion healthPotion = (HealthPotion)item;
                int amountHealed = healthPotion.HealAmount;
                GetComponent<Actor>().Heal(amountHealed);
                UIManager.Instance.AddMessage($"Used {item.name} and healed {amountHealed} health points.", Color.green);
            }
            else if (item is Fireball)
            {
                List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                foreach (Actor enemy in nearbyEnemies)
                {
                    int damage = 10;
                    enemy.DoDamage(damage);
                }
                UIManager.Instance.AddMessage($"Used {item.name} and dealt damage to nearby enemies.", Color.red);
            }
            else if (item is ScrollOfConfusion)
            {
                List<Actor> nearbyEnemies = GameManager.Get.GetNearbyEnemies(transform.position);
                foreach (Actor enemy in nearbyEnemies)
                {
                    Enemy enemyComponent = enemy.GetComponent<Enemy>();
                    if (enemyComponent != null)
                    {
                        enemyComponent.Confuse();
                    }
                }
                UIManager.Instance.AddMessage($"Used {item.name} and confused nearby enemies.", Color.blue);
            }
            inventoryUI.Hide();
            inventoryIsOpen = false;
            droppingItem = false;
            usingItem = false;
        }
    }
    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.MoveOrHit(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector3 playerPosition = transform.position;
            Ladder ladder = GameManager.Get.GetLadderAtLocation(playerPosition);

            if (ladder != null)
            {
                if (ladder.Up)
                {
                    MapManager.Get.MoveUp();
                }
                else
                {
                    MapManager.Get.MoveDown();
                }
            }
        }
    }
}
