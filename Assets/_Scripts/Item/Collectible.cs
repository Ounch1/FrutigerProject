using UnityEngine;

public class Collectible : MonoBehaviour
{
    public ItemData itemData;

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
}
