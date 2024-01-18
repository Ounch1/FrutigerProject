using UnityEngine;
using UnityEngine.UI;

// TODO: Make the items add at first slot and not at 0
public class InventoryUI : MonoBehaviour
{
    public GameObject slotHighlight;
    public GameObject[] slots;
    public Image[] itemImages;
    public Inventory inventory;

    private void Start()
    {
        // Get images from the slots
        for (int i = 0; i < slots.Length; i++)
        {
            itemImages[i] = slots[i].transform.GetChild(0).GetComponent<Image>();
        }
    }
    private void Update()
    {
        // Move the slot hightlight cursor to the current item index
        HandleHighlightCursor();

        // Update the item images
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (inventory.itemArray[i] == null)
            {
                itemImages[i].gameObject.SetActive(false);
            }
            else
            {
                itemImages[i].gameObject.SetActive(true);
                itemImages[i].sprite = inventory.itemArray[i].itemIcon;
            }
        }
    }

    private void HandleHighlightCursor()
    {
        if (inventory.GetSelectedItemIndex() == -1)
        {
            slotHighlight.SetActive(false);
            return;
        }

        slotHighlight.SetActive(true);
        slotHighlight.transform.position = slots[inventory.GetSelectedItemIndex()].transform.position;
    }
}
