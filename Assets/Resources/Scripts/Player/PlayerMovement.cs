using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _attackCd = 0.5f;
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private LayerMask _hittableLayer;
    [SerializeField] private LayerMask _stairsLayer;
    [SerializeField] private float _interactionRadius = 1f;
    [SerializeField] private float _hitRadius = 0.35f;
    private float _hitOffset = 1f;
    private Animator _playerAnim;
    private SpriteRenderer _playerRenderer;
    private float _inputH;
    private float _inputV;
    private Vector3 _destinationPoint;
    private Vector3 _interactionPoint;
    private bool _isMoving;
    private bool _isAttacking;
    private int _lastAttack = 0;
    private bool _canAttack = true;
    private int _currentHeight = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerAnim = GetComponent<Animator>();
        _playerRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!_isMoving)
        {
            if (_isAttacking) return;
            else
            {
                if (_canAttack && Input.GetKeyDown(KeyCode.F))
                {
                    StartCoroutine(Attack());
                }
            }

            _inputH = Input.GetAxisRaw("Horizontal");
            _inputV = Input.GetAxisRaw("Vertical");
            if (_inputH != 0) _inputV = 0;
            else if (_inputV != 0) _inputH = 0;

            if (_inputV == 0) //Flip sprite
            {
                if (_inputH < 0) _playerRenderer.flipX = true;
                else if(_inputH > 0) _playerRenderer.flipX = false;
            }

            if (_inputH != 0 || _inputV != 0)
            {
                _destinationPoint = transform.position + new Vector3(_inputH, _inputV, 0f);
                _interactionPoint = _destinationPoint;

                if (!GetCollision()) //Checks for collision (walls)
                {
                    Collider2D stairs = GetStairsCollision(); //Checks for stairs
                    if (stairs != null)
                    {
                        if (_inputV == 0) //Vertical movement collides with stairs
                        {
                            UseStairs(stairs); 
                        }
                        else return;
                    }
                    else
                    {
                        int nextHeight = GetDestinationHeight(_destinationPoint);
                        if (_currentHeight != nextHeight) return;
                    }
                    StartCoroutine(Move());
                }
            }
        }
    }

    private int GetDestinationHeight(Vector3 dest)
    {
        Collider2D c = Physics2D.OverlapCircle(dest, 0.1f);
        switch(c.tag)
        {
            case "Ground3": return 3;
            case "Ground2": return 2;
            default: return 1;
        }
    }

    private void UseStairs(Collider2D stairs)
    {
        if (stairs.CompareTag("RightStairs"))
        {
            if (_inputH > 0)
            {
                _destinationPoint = transform.position + new Vector3(2, -1, 0f);
                _currentHeight--;
            }
            else if (_inputH < 0)
            {
                _destinationPoint = transform.position + new Vector3(-2, 1, 0f);
                _currentHeight++;
            }
        }
        else if (stairs.CompareTag("LeftStairs"))
        {
            if (_inputH > 0)
            {
                _destinationPoint = transform.position + new Vector3(2, 1, 0f);
                _currentHeight++;
            }
            else if (_inputH < 0)
            {
                _destinationPoint = transform.position + new Vector3(-2, -1, 0f);
                _currentHeight--;
            }
        }
        _interactionPoint = _destinationPoint;
    }
    private bool GetCollision() {
        return Physics2D.OverlapCircle(_interactionPoint, _interactionRadius, _collisionLayer);
    }

    private Collider2D GetStairsCollision()
    {
        return Physics2D.OverlapCircle(_interactionPoint, _interactionRadius, _stairsLayer);
    }
    private IEnumerator Move()
    {
        _isMoving = true;
        while (transform.position != _destinationPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destinationPoint, _movementSpeed * Time.deltaTime);
            yield return null;
        }
        _interactionPoint = _destinationPoint;
        _isMoving = false;
    }

    private IEnumerator Attack()
    {
        _isAttacking = true;
        _canAttack = false;
        if(_lastAttack == 0) //Plays attack 1
        {
            _playerAnim.SetTrigger("Attack1");
            _lastAttack = 1;
        }
        else if(_lastAttack == 1) //Plays attack 0
        {
            _playerAnim.SetTrigger("Attack0");
            _lastAttack = 0;
        }
        Vector3 hitPoint = GetHitPoint();
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint, _hitRadius, _hittableLayer);
        foreach (var h in hits)
        {
            //TODO: Activate hittable stuff (destroy for example or damage if enemy)
        }
        yield return new WaitForSeconds(0.5f);
        _isAttacking = false;
        StartCoroutine(ResetAttack());
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(_attackCd);
        _canAttack = true;
    }

    private Vector3 GetHitPoint()
    {
        float dir = _playerRenderer.flipX ? -1f : 1f;
        return transform.position + new Vector3(dir * _hitOffset, 0f, 0f);
    }

    private void LateUpdate()
    {
        _playerAnim.SetFloat("InputH", Mathf.Abs(_inputH));
        _playerAnim.SetFloat("InputV", Mathf.Abs(_inputV));
    }
    private void OnDrawGizmosSelected()
    {
        if (_playerRenderer == null) return;
        Gizmos.DrawWireSphere(GetHitPoint(), _hitRadius);
    }
}
