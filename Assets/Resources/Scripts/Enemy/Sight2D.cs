using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _checkFrequency = 5f;
    private float _lastCheckTime;
    private Collider2D[] _colliders;
    private Transform _closestPlayer;
    private float _distanceToClosestPlayer;
    private float _priorityOfClosestPlayer;
    [Space]
    [SerializeField] private IVisible2D.Side[] _perceivedSides;


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
            _priorityOfClosestPlayer = -1f;
            for(int i = 0; i < _colliders.Length; i++)
            {
                IVisible2D visible = _colliders[i].GetComponent<IVisible2D>();
                if ((visible != null) && (CanSee(visible)))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, _colliders[i].transform.position);
                    if((visible.GetPriority() > _priorityOfClosestPlayer) || 
                        (visible.GetPriority() == _priorityOfClosestPlayer) && (distanceToPlayer < _distanceToClosestPlayer))
                    {
                        _closestPlayer = _colliders[i].transform;
                        _distanceToClosestPlayer = distanceToPlayer;
                        _priorityOfClosestPlayer = visible.GetPriority();
                    }
                }
            }
        }
    }

    public Transform GetClosestTarget()
    {
        return _closestPlayer;
    }

    private bool CanSee(IVisible2D visible)
    {
        bool canSee = false;
        for(int i = 0; i < _perceivedSides.Length; i++)
        {
            canSee = visible.GetSide() == _perceivedSides[i];
        }
        return canSee;
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
