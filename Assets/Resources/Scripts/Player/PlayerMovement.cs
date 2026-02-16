using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private LayerMask _collisionLayer;
    [SerializeField] private float _interactionRadius = 1f;
    private float _inputH;
    private float _inputV;
    private Vector3 _destinationPoint;
    private Vector3 _interactionPoint;
    private bool _isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_inputV == 0) _inputH = Input.GetAxisRaw("Horizontal");
        if(_inputH == 0) _inputV = Input.GetAxisRaw("Vertical");

        if (!_isMoving && (_inputH != 0 || _inputV != 0)) {
            _destinationPoint = transform.position + new Vector3(_inputH, _inputV, 0f);
            _interactionPoint = _destinationPoint;
            if(!GetInteraction()) StartCoroutine(Move());
        } 
    }
    private bool GetInteraction()
    {
        return Physics2D.OverlapCircle(_interactionPoint, _interactionRadius, _collisionLayer);
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
}
