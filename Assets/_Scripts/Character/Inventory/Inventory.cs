using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// The Inventory class manages a simple inventory system with a fixed size.
/// </summary>
public class Inventory : MonoBehaviour
{
    public ItemData[] itemArray = new ItemData[10];
    private int selectItemIndex = -1;
    public int GetSelectedItemIndex() => selectItemIndex;
    public UnityEvent<int> onAction;

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                // Use ternary operator to toggle the selection/deselection of the item.
                selectItemIndex = (selectItemIndex == i) ? -1 : i;
                onAction.Invoke(selectItemIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && selectItemIndex != -1)
        {
            DropItem(selectItemIndex);
            onAction.Invoke(selectItemIndex);
        }
    }
    /// <summary>
    /// Return true and add the item if there is an empty slot, else return false, begin at index 1 and end at index 0.
    /// </summary>
    /// <param name="item">The item to be added to the inventory.</param>
    /// <returns>True if the item was added successfully, false if no empty slots are available.</returns>
    public bool TryAddItem(ItemData itemData)
    {
        for (int i = 1; i < itemArray.Length + 1; i++)
        {
            int index = i % itemArray.Length; // Use modulo to go back to 0 when i is equal to itemArray.Length
            if (itemArray[index] == null)
            {
                itemArray[index] = itemData;
                onAction.Invoke(selectItemIndex);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Drops an item from the specified inventory slot.
    /// </summary>
    /// <param name="slotIndex">The index of the slot from which to drop the item.</param>
    public void DropItem(int slotIndex)
    {
        if (itemArray[slotIndex] == null)
        {
            return;
        }
        Vector3 dropPos = CalculateDropPosition();
        AdjustToAvoidPlayer(ref dropPos, 1.4f);

        Instantiate(itemArray[slotIndex].collectiblePrefab, dropPos, transform.rotation);
        itemArray[slotIndex] = null;
    }

    /// <summary>
    /// Calculates the drop position for an item based on the player's orientation and environment.
    /// </summary>
    /// <returns>The calculated drop position as a Vector3.</returns>
    private Vector3 CalculateDropPosition()
    {
        float maxDistance = 1.6f;
        Vector3 dropPos = transform.position + transform.forward * maxDistance;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            dropPos = hit.point - transform.forward * Mathf.Min(0.5f, Vector3.Distance(hit.point, transform.position));
        }

        return dropPos;
    }

    /// <summary>
    /// Adjusts the drop position to ensure a minimum distance from the player.
    /// </summary>
    /// <param name="dropPos">The current drop position to be adjusted.</param>
    /// <param name="minDistanceToPlayer">The minimum distance to be maintained between the dropped item and the player.</param>
    private void AdjustToAvoidPlayer(ref Vector3 dropPos, float minDistanceToPlayer)
    {
        // Calculate the distance between the drop position and the player
        float distanceToPlayer = Vector3.Distance(dropPos, transform.position);

        if (distanceToPlayer < minDistanceToPlayer)
        {
            // Determine the offset direction based on the player's rotation
            Vector3 playerForward = transform.forward;
            Vector3 playerRight = Vector3.Cross(playerForward, Vector3.up);
            Vector3 offsetDirection = Vector3.Dot(dropPos - transform.position, playerRight) > 0 ? playerRight : -playerRight;

            // Adjust the drop position by moving it along the offset direction
            dropPos += offsetDirection * (minDistanceToPlayer - distanceToPlayer);
        }
    }

}
