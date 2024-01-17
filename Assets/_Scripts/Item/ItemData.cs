using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    public Sprite itemIcon;
    [TextArea]
    public string description;
}
