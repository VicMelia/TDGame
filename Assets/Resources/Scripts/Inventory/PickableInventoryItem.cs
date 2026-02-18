using UnityEngine;

public class PickableInventoryItem : MonoBehaviour, IVisible2D
{
    [SerializeField] InventoryItemDefinition itemDefinition;
    [SerializeField] int priority = 0;
    [SerializeField] IVisible2D.Side side = IVisible2D.Side.Neutrals;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryUI.instance.NotifyItemPicked(itemDefinition);
            Destroy(gameObject);
        }
    }

    public int GetPriority()
    {
        return priority;
    }

    public IVisible2D.Side GetSide()
    {
        return side;
    }
}