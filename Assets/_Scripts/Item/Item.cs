using UnityEngine;
/// <summary>
/// The base class for all items in the game.
/// </summary>
public abstract class Item : MonoBehaviour
{
    public ItemData itemData;

    protected string itemName;
    public string GetItemName() => itemName; 

    private void Start()
    {
        itemName = itemData.name;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Inventory inventory))
        {
            // Destroy the item object if it is successfully added to the inventory.
            if (inventory.TryAddItem(itemData))
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// An abstract method that defines how the item is used.
    /// Subclasses must implement this method to specify item-specific behavior.
    /// </summary>
    public abstract void UseItem();

}
