using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseCharacter
{
    [SerializeField] private float _stepDistance = 1f;
    [SerializeField] private LayerMask _solidLayer;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _stairsLayer;
    [SerializeField] private float _interactionRadius = 0.1f;
    [SerializeField] private int _maxHealth = 5;
    [SerializeField] private float _hurtInvulTime = 0.2f;
    [SerializeField] private float _attackCd = 0.5f;
    [SerializeField] private LayerMask _hittableLayer;
    [SerializeField] private float _hitRadius = 0.35f;
    [SerializeField] private float _hitOffset = 1f;
    [SerializeField] private Life _life;
    private bool _isInventoryOpen;
    private bool _isDead;
    private bool _isAttacking;
    private bool _canAttack = true;
    private int _lastAttack = 0;
    private int _currentHealth;
    private bool _invulnerable;
    private float _inputH;
    private float _inputV;
    private Vector3 _destinationPoint;
    private bool _isMoving;
    private int _currentHeight = 2;
    private bool _canMove = true;

    protected override void Awake()
    {
        base.Awake();
        _currentHealth = _maxHealth;
        _destinationPoint = transform.position;
    }

    private void Update()
    {
        if (_isMoving || _isDead || _isAttacking) return;
        if (!_canMove) return;
        if (_canAttack && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(Attack());
            return;
        }
        if (Input.GetKeyDown(KeyCode.E)) ShowInventory();
        _inputH = Input.GetAxisRaw("Horizontal");
        _inputV = Input.GetAxisRaw("Vertical");
        if (_inputH != 0) _inputV = 0;
        else if (_inputV != 0) _inputH = 0;
        if (_inputV == 0) FaceByDirectionX(_inputH); //Flip sprite
        if (_inputH == 0 && _inputV == 0) return;
        Vector3 step = new Vector3(_inputH, _inputV, 0f) * _stepDistance;
        Vector3 target = transform.position + step;
        if (IsSolidAt(target))
        {
            _inputH = 0;
            _inputV = 0;
            return;
        }
        Collider2D stairs = GetStairsAt(target);
        if (stairs != null)
        {
            if (_inputV == 0)
            {
                target = UseStairs(stairs); //stairs if walking horizontal
            }
            else
            {
                _inputH = 0;
                _inputV = 0;
                return;
            }
        }
        else
        {
            int nextHeight = GetDestinationHeight(target);
            if (_currentHeight != nextHeight)
            {
                _inputH = 0;
                _inputV = 0;
                return;
            }
        }
        _destinationPoint = target;
        StartCoroutine(MoveStep());
    }
    private void ShowInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;
        InventoryUI.instance.OpenInventory(_isInventoryOpen);
    }

    public void SetMovement(bool canMove)
    {
        _canMove = canMove;
    }
    private IEnumerator MoveStep() //Grid movement
    {
        _isMoving = true;
        while (Vector3.Distance(transform.position, _destinationPoint) > 0.001f)
        {
            MoveTo(_destinationPoint);
            yield return null;
        }
        transform.position = _destinationPoint;
        _isMoving = false;
        _inputH = 0;
        _inputV = 0;
    }
    private bool IsSolidAt(Vector3 point)
    {
        return Physics2D.OverlapCircle(point, _interactionRadius, _solidLayer) != null;
    }
    private Collider2D GetStairsAt(Vector3 point)
    {
        return Physics2D.OverlapCircle(point, _interactionRadius, _stairsLayer);
    }
    private int GetDestinationHeight(Vector3 dest)
    {
        Collider2D c = Physics2D.OverlapCircle(dest, 0.1f, _groundLayer);
        if (c == null) return _currentHeight;

        switch (c.tag)
        {
            case "Ground3": return 3;
            case "Ground2": return 2;
            default: return 1;
        }
    }
    private Vector3 UseStairs(Collider2D stairs)
    {
        Vector3 target = transform.position;

        if (stairs.CompareTag("RightStairs"))
        {
            if (_inputH > 0)
            {
                target += new Vector3(2f, -1f, 0f);
                _currentHeight--;
            }
            else if (_inputH < 0)
            {
                target += new Vector3(-2f, 1f, 0f);
                _currentHeight++;
            }
        }
        else if (stairs.CompareTag("LeftStairs"))
        {
            if (_inputH > 0)
            {
                target += new Vector3(2f, 1f, 0f);
                _currentHeight++;
            }
            else if (_inputH < 0)
            {
                target += new Vector3(-2f, -1f, 0f);
                _currentHeight--;
            }
        }

        return target;
    }
    private void LateUpdate()
    {
        SetAnimMove(_inputH, _inputV);
    }
    private IEnumerator Attack()
    {
        _isAttacking = true;
        _canAttack = false;
        if (_lastAttack == 0)
        {
            _anim.SetTrigger("Attack1");
            _lastAttack = 1;
        }
        else
        {
            _anim.SetTrigger("Attack0");
            _lastAttack = 0;
        }
        Vector3 hitPoint = GetHitPoint();
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint, _hitRadius, _hittableLayer);
        foreach (var h in hits)
        {
            if (h == null) continue;
            h.TryGetComponent<EnemyCharacter>(out EnemyCharacter enemy);
            if (enemy != null)
            {
                enemy.TakeDamage(1);
                Debug.Log("Le hago daño");
            }
        }
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        yield return new WaitForSeconds(_attackCd);
        _canAttack = true;
    }
    private Vector3 GetHitPoint()
    {
        float dir = _sr.flipX ? -1f : 1f;
        return transform.position + new Vector3(dir * _hitOffset, 0f, 0f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.right, _hitRadius);
    }
    
    public void TakeDamage(int dmg)
    {
        if (_isDead || _invulnerable) return;
        if (dmg <= 0) return;
        _life.OnHitReceived(dmg);
        _currentHealth -= dmg;
        Debug.Log("Me hace 1 de damage");
        if (_currentHealth < 0) _currentHealth = 0;
        if (_currentHealth == 0)
        {
            Die();
            return;
        }
        if (_hurtInvulTime > 0f)
            StartCoroutine(HurtInvulnerability());
    }
    private IEnumerator HurtInvulnerability()
    {
        _invulnerable = true;
        yield return new WaitForSeconds(_hurtInvulTime);
        _invulnerable = false;
    }

    protected override void Die()
    {
        base.Die();
        _isDead = true;
        StartCoroutine(ReturnToMenu());
    }

    IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }

    public bool IsDead()
    {
        return _isDead;
    }

    public void NotifyInventoryItemUsed(InventoryItemDefinition itemDefinition)
    {
        if (itemDefinition.uniqueItemName.Contains("Meat"))
        {
            _currentHealth++;
            _life.RecoverLife(1f);
        }
    }
}
