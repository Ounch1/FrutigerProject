using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [Space]
    public GameObject itemPrefab;
    public GameObject collectiblePrefab;

    [TextArea]
    public string description;
}
