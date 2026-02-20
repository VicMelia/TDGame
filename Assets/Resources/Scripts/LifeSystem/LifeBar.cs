using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] Life life;
    [SerializeField] Image imageFill;

    void OnEnable()
    {
        if(life == null)
        {
            Debug.LogError("Please assign a Life component to the LifeBar.");
            return;
        }
        life.onLifeChanged.AddListener(OnLifeChanged);
        life.onDeath.AddListener(OnDeath);
    }
    void OnDisable()
    {
        life.onLifeChanged.RemoveListener(OnLifeChanged);
        life.onDeath.RemoveListener(OnDeath);
    }

    void OnLifeChanged(float percentage)
    {
        imageFill.fillAmount = percentage;
    }
    void OnDeath()
    {
        Destroy(gameObject);
    }
}
