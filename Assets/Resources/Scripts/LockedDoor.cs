using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] InventoryItemDefinition keyDefinition;
    [SerializeField] private EnemySpawner _spawner;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(InventoryUI.instance.Contains(keyDefinition))
            {
                InventoryUI.instance.Consume(keyDefinition);
                Destroy(gameObject);
                if(_spawner != null) _spawner.StartSpawning();
            }
        }
    }
}
