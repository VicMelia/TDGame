using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] InventoryItemDefinition keyDefinition;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(InventoryUI.instance.Contains(keyDefinition))
            {
                InventoryUI.instance.Consume(keyDefinition);
                Destroy(gameObject);
            }
        }
    }
}
