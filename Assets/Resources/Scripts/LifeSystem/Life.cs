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

    GameObject BloodEffectPrefab;
    GameObject SmokeEffectPrefab;
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
        BloodEffectPrefab = Resources.Load<GameObject>("Prefabs/BloodPS");
        SmokeEffectPrefab = Resources.Load<GameObject>("Prefabs/SmokePS");
    }

    public void OnHitReceived(float damage)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
            Instantiate(BloodEffectPrefab, transform.position, Quaternion.identity);
            onLifeChanged.Invoke(currentLife / startingLife);
            if (currentLife <= 0f)
            {
                currentLife = 0;
                Instantiate(SmokeEffectPrefab, transform.position, Quaternion.identity);
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
