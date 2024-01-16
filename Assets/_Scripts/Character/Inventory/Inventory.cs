using System;
using UnityEngine;
/// <summary>
/// The Inventory class manages a simple inventory system with a fixed size.
/// </summary>
public class Inventory : MonoBehaviour
{
    public ItemData[] itemArray = new ItemData[5];

    /// <summary>
    /// Return true and add the item if there is an empty slot, else return false.
    /// </summary>
    /// <param name="item">The item to be added to the inventory.</param>
    public bool TryAddItem(ItemData itemData)
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null)
            {
                itemArray[i] = itemData;
                return true;
            }
        }
        return false;
    }

    public void DropItem(int slotIndex)
    {
        throw new NotImplementedException("This method is not implemented.");
    }
}
