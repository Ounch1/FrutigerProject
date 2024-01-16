using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject itemPrefab;
    [TextArea]
    public string description;
}
