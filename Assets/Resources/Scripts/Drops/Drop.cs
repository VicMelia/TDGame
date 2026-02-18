using UnityEngine;

public class Drop : MonoBehaviour, IVisible2D
{
    [SerializeField] private DropDefinition dropDefinition;
    [SerializeField] private int priority = 0;
    [SerializeField] private IVisible2D.Side side = IVisible2D.Side.Neutrals;

    public void OnPickup()
    {
        Destroy(gameObject);
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