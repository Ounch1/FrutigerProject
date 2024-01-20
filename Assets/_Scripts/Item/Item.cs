using UnityEngine;
/// <summary>
/// The base class for all items in the game.
/// </summary>
public abstract class Item : MonoBehaviour
{
    /// <summary>
    /// An abstract method that defines how the item is used.
    /// Subclasses must implement this method to specify item-specific behavior.
    /// </summary>
    public abstract void UseItem();

}
