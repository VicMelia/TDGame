using System.Collections;
using UnityEngine;

public class EnemyCharacter : BaseCharacter
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _stopDistance = 0.2f;
    [SerializeField] private float _attackRange = 0.6f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private GameObject[] _deathDrops;
    private EnemySpawner _spawner;
    private Sight2D _sight2D;
    private float _nextAttackTime;
    private int _currentHealth;
    private bool _isDead;
    private Life _life;

    protected override void Awake()
    {
        base.Awake();
        _currentHealth = _maxHealth;
        _nextAttackTime = 0.5f;
        _sight2D = GetComponent<Sight2D>();
        _player = FindAnyObjectByType<PlayerController>();
        _life = GetComponent<Life>();
    }
    private void Update()
    {
        if (_player == null) return;
        Transform closestTarget = _sight2D.GetClosestTarget();
        //Debug.Log(closestTarget);
        if (closestTarget == null)
        {
            SetAnimMove(0, 0);
            return;
        }
        Vector3 delta = closestTarget.position - transform.position;
        FaceByDirectionX(delta.x); //Flip sprite towards player
        float dist = Vector2.Distance(transform.position, closestTarget.position);
        if (dist <= _attackRange)
        {
            PlayerController player = closestTarget.GetComponent<PlayerController>();
            if(player != null && !player.IsDead() && closestTarget.CompareTag("Player")) TryAttack();
            SetAnimMove(0, 0);
            return;
        }
        if (dist > _stopDistance)
        {
            if(closestTarget == null) SetAnimMove(0, 0);
            else
            {
                MoveTo(closestTarget.position);
                float h = Mathf.Abs(delta.x) >= Mathf.Abs(delta.y) ? 1f : 0f;
                float v = h == 1f ? 0f : 1f;
                SetAnimMove(h, v);
            }
        }
        else
        {
            SetAnimMove(0, 0);
        }
    }
    private void TryAttack()
    {
        if (Time.time < _nextAttackTime) return;
        _nextAttackTime = Time.time + _attackCooldown;
        _anim.SetTrigger("Attack0");
        _player.TakeDamage(_damage);
    }
    public void TakeDamage(int dmg)
    {
        if (_isDead) return;
        if (dmg <= 0) return;
        _currentHealth -= dmg;
        _life.OnHitReceived(dmg);
        Debug.Log("Enemy got hit");
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        base.Die();
        _isDead = true;
        if(Random.Range(0f, 1f) < 0.55f)
        {
            int index = Random.Range(0, _deathDrops.Length);
            Instantiate(_deathDrops[index], transform.position, Quaternion.identity);
        }
        _spawner.OnEnemyDefeated();
        Destroy(gameObject);
    }
    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }
}
