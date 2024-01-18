using System;
using UnityEngine;
/// <summary>
/// The Inventory class manages a simple inventory system with a fixed size.
/// </summary>
public class Inventory : MonoBehaviour
{
    public ItemData[] itemArray = new ItemData[10];
    private int selectItemIndex = -1;
    public int GetSelectedItemIndex() => selectItemIndex;
    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                // Use ternary operator to toggle the selection/deselection of the item.
                selectItemIndex = (selectItemIndex == i) ? -1 : i;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && selectItemIndex != -1)
        {
            DropItem(selectItemIndex);
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
        Vector3 dropPos = transform.position + transform.forward * 2;
        Instantiate(itemArray[slotIndex].itemPrefab, dropPos, Quaternion.identity);
        itemArray[slotIndex] = null;
    }
}
