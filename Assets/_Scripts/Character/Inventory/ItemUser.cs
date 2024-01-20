using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ItemUser : MonoBehaviour
{
    [SerializeField] private GameObject handPos;
    private Inventory inventory;


    private void Start()
    {
        inventory = GetComponent<Inventory>();
        inventory.onAction.AddListener(ToggleItemHold);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ActivateItem();
        }
    }

    /// <summary>
    /// If the item index is valid and the corresponding item is not null, 
    /// it instantiates the item's prefab at the designated hand position.
    /// If the item index is -1 or the corresponding item is null, 
    /// it destroys the currently held item if there is one.
    /// </summary>
    /// <param name="itemIndex">The index of the item to be held or released.</param>
    private void ToggleItemHold(int itemIndex)
    {
        if (itemIndex != -1 && inventory.itemArray[itemIndex] != null)
        {
            GameObject instantiatedObject = Instantiate(inventory.itemArray[itemIndex].itemPrefab, handPos.transform);
            instantiatedObject.transform.localPosition = Vector3.zero;
        }
        else if (handPos.transform.childCount > 0)
        {
            Destroy(handPos.transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Call the UseItem method from currently held item if there is one.
    /// </summary>
    private void ActivateItem()
    {
        if (handPos.transform.childCount > 0)
        {
            Item item = handPos.transform.GetChild(0).GetComponent<Item>();
            if (item != null)
            {
                item.UseItem();
            }
        }
    }
}
