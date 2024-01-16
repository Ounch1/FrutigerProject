using UnityEngine;
/// <summary>
/// The base class for all items in the game.
/// </summary>
public abstract class Item : MonoBehaviour
{
    public string itemName;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Inventory inventory))
        {
            // Destroy the item object if it is successfully added to the inventory.
            if (inventory.TryAddItem(this))
            {
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// An abstract method that defines how the item is used.
    /// Subclasses must implement this method to specify item-specific behavior.
    /// </summary>
    public abstract void UseItem();

}
