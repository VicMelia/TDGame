using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _checkFrequency = 5f;
    private float _lastCheckTime;
    private Collider2D[] _colliders;
    private Transform _closestPlayer;
    private float _distanceToClosestPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if((Time.time - _lastCheckTime) > (1f / _checkFrequency))
        {
            _lastCheckTime = Time.time;
            _colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            _closestPlayer = null;
            _distanceToClosestPlayer = Mathf.Infinity;
            for(int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i].CompareTag("Player"))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, _colliders[i].transform.position);
                    if(distanceToPlayer < _distanceToClosestPlayer)
                    {
                        _closestPlayer = _colliders[i].transform;
                        _distanceToClosestPlayer = distanceToPlayer;
                    }
                }
            }
        }
    }

    public Transform GetClosestTarget()
    {
        return _closestPlayer;
    }

    public bool IsPlayerInSight()
    {
        bool isPlayerInSight = false;
        for(int i = 0; !isPlayerInSight && (i < _colliders.Length); i++)
        {
            if (_colliders[i].CompareTag("Player"))
            {
                isPlayerInSight = true;
            }
        }
        return isPlayerInSight;
    }
}
