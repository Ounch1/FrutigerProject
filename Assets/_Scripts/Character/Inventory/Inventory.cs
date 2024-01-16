using UnityEngine;
/// <summary>
/// The Inventory class manages a simple inventory system with a fixed size.
/// </summary>
public class Inventory : MonoBehaviour
{
    [SerializeField] private string[] itemArray = new string[5]; // SerializeField for testing

    /// <summary>
    /// Return true and add the item if there is an empty slot, else return false.
    /// </summary>
    /// <param name="item">The item to be added to the inventory.</param>
    public bool TryAddItem(Item item)
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (string.IsNullOrEmpty(itemArray[i]))
            {
                itemArray[i] = item.itemName;
                return true;
            }
        }
        return false;
    }
}
