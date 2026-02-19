using UnityEngine;
public abstract class BaseCharacter : MonoBehaviour, IVisible2D
{
    [SerializeField] protected float _moveSpeed = 3f;
    protected SpriteRenderer _sr;
    protected Animator _anim;
    [SerializeField] protected int _priority = 0;
    [SerializeField] protected IVisible2D.Side _side;

    protected virtual void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    protected void FaceByDirectionX(float dirX)
    {
        if (_sr == null) return;
        if (dirX < 0) _sr.flipX = true;
        else if (dirX > 0) _sr.flipX = false;
    }
    protected float MoveTo(Vector3 targetPosition)
    {
        Vector3 before = transform.position;
        transform.position = Vector3.MoveTowards(before, targetPosition, _moveSpeed * Time.deltaTime);
        return Vector3.Distance(transform.position, targetPosition);
    }

    protected void SetAnimMove(float inputH, float inputV)
    {
        if (_anim == null) return;
        _anim.SetFloat("InputH", Mathf.Abs(inputH));
        _anim.SetFloat("InputV", Mathf.Abs(inputV));
    }

    int IVisible2D.GetPriority()
    {
        return _priority;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return _side;
    }
}
