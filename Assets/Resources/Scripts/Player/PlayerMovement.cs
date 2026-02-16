using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private LayerMask _stairsLayer;
    [SerializeField] private float _interactionRadius = 1f;
    private Animator _playerAnim;
    private SpriteRenderer _playerRenderer;
    private float _inputH;
    private float _inputV;
    private Vector3 _destinationPoint;
    private Vector3 _interactionPoint;
    private bool _isMoving;

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
            _inputH = Input.GetAxisRaw("Horizontal");
            _inputV = Input.GetAxisRaw("Vertical");

            if (_inputH != 0) _inputV = 0;
            else if (_inputV != 0) _inputH = 0;

            if (_inputH != 0 || _inputV != 0)
            {
                _destinationPoint = transform.position + new Vector3(_inputH, _inputV, 0f);
                _interactionPoint = _destinationPoint;

                if (!GetCollision())
                {
                    _playerRenderer.flipX = (_inputH < 0);
                    Collider2D stairs = GetStairsCollision();
                    if (stairs != null)
                    {
                        if(_inputV == 0) //vertical movement collides
                        {
                            if (stairs.CompareTag("RightStairs"))
                            {
                                if (_inputH > 0)
                                {
                                    _destinationPoint = transform.position + new Vector3(1, -1, 0f);
                                }
                                else if (_inputH < 0)
                                {
                                    _destinationPoint = transform.position + new Vector3(-1, 1, 0f);
                                }
                            }
                            else if (stairs.CompareTag("LeftStairs"))
                            {
                                if (_inputH > 0)
                                {
                                    _destinationPoint = transform.position + new Vector3(-1, 1, 0f);
                                }
                                else if (_inputH < 0)
                                {
                                    _destinationPoint = transform.position + new Vector3(1, -1, 0f);
                                }
                            }
                            _interactionPoint = _destinationPoint;
                        }
                        
                    }
                    StartCoroutine(Move());
                }
            }
        }
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

    private void LateUpdate()
    {
        _playerAnim.SetFloat("InputH", Mathf.Abs(_inputH));
        _playerAnim.SetFloat("InputV", Mathf.Abs(_inputV));
    }
}
