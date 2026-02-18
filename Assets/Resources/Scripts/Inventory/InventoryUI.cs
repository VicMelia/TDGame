using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] InventoryItemUI itemUIPrefab;
    [SerializeField] PlayerController owner;
    public static InventoryUI instance;
    GridLayoutGroup gridLayout;
    void Awake()
    {
        if (instance != null)
        {
            throw new System.Exception("There should be only one InventoryUI instance");
        }
        instance = this;
        gridLayout = GetComponentInChildren<GridLayoutGroup>();
    }

    public void NotifyItemPicked(InventoryItemDefinition itemDefinition)
    {
        InventoryItemUI itemUI = Instantiate(itemUIPrefab, gridLayout.transform);
        itemUI.Init(itemDefinition);
    }

    public void NotifyInventoryItemUsed(InventoryItemDefinition itemDefinition)
    {
        Debug.LogWarning("Create the logic to use items in player controller");
        //owner.NotifyInventoryItemUsed(itemDefinition);
    }

    internal bool Contains(InventoryItemDefinition keyDefinition)
    {
        InventoryItemUI[] items = GetComponentsInChildren<InventoryItemUI>();
        return Array.Find(items, item => item.definition.uniqueItemName == keyDefinition.uniqueItemName) != null;
    }

    internal void Consume(InventoryItemDefinition keyDefinition)
    {
        InventoryItemUI[] items = GetComponentsInChildren<InventoryItemUI>();
        InventoryItemUI itemToConsume = Array.Find(items, item => item.definition.uniqueItemName == keyDefinition.uniqueItemName);
        if (itemToConsume != null)
        {
            itemToConsume.definition.numUses--;
            if (itemToConsume.definition.numUses <= 0)
            {
                Destroy(itemToConsume.gameObject);
            }
        }
        else
        {
            Debug.LogError("Trying to consume an item that is not in the inventory: " + keyDefinition.uniqueItemName);
        }
    }
}
