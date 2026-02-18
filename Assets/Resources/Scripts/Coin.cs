using UnityEngine;

public class Coin : MonoBehaviour, IVisible2D
{
    [SerializeField] private int priority = 0;
    [SerializeField] private IVisible2D.Side side = IVisible2D.Side.Neutrals;
    public int GetPriority()
    {
        return priority;
    }

    public IVisible2D.Side GetSide()
    {
        return side;
    }
}