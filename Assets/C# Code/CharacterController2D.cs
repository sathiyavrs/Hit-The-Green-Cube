using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public bool DebugInformation;
    public Vector2 Velocity
    {
        get
        {
            return _velocity;
        }
    }

    private Vector2 _velocity;
    private CircleCollider2D _collider;
    private BoxCollider2D _other;

    public void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        if (_collider == null)
            Debug.LogError("Circle Collider is not available");
    }

    public void Update()
    {

    }
}
