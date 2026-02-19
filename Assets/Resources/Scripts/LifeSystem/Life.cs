using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] float startingLife = 10f;
    [SerializeField] public UnityEvent<float> onLifeChanged;
    [SerializeField] public UnityEvent onDeath;
    
    [Header("Debug")]
    [SerializeField] float debugHitDamage = 0.1f;
    [SerializeField] bool debugReceiveHit;
    float currentLife;

    private void OnValidate()
    {
        if (debugReceiveHit)
        {
            debugReceiveHit = false;
            OnHitReceived(debugHitDamage);
        }
    }

    void Awake()
    {
        currentLife = startingLife;
    }

    public void OnHitReceived(float damage)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
            onLifeChanged.Invoke(currentLife / startingLife);
            if (currentLife <= 0f)
            {
                currentLife = 0;
                onDeath.Invoke();
            }
        }
    }
    public void RecoverLife(float recovery)
    {
        currentLife += recovery;
        onLifeChanged.Invoke(currentLife / startingLife);
        if (currentLife > startingLife)
        {
            currentLife = startingLife;
        }
    }
}
