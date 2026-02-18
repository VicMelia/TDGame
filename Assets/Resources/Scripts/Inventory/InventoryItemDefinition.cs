using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemDefinition", menuName = "ScriptableObjects/InventoryItemDefinition", order = 1)]
public class InventoryItemDefinition : ScriptableObject
{
    public Sprite icon;
    public string uniqueItemName;
    public string description;
    public int numUses = 1;
}