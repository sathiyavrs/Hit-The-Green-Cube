using UnityEngine;

public class PositionHelper : MonoBehaviour
{
    public Resources.CornersBox Positions { get; private set; }
    public float AngleOfRotation { get; private set; }
    public Resources.CornersBox ZeroAnglePositions { get; private set; }
    public Resources.CornersBox RelativePositionsToCenter { get; private set; }

    private BoxCollider2D _boxCollider;
    private Collider2D _collider;

    public void Start()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogError("No Collider found!");
            return;
        }
        _boxCollider = (BoxCollider2D)_collider;

        Positions = new Resources.CornersBox();
        ZeroAnglePositions = new Resources.CornersBox();
        RelativePositionsToCenter = new Resources.CornersBox();
    }

    public void Update()
    {
        AngleOfRotation = transform.rotation.eulerAngles.z;
        ComputeNormalPositions(Positions);
        ZeroAnglePositions.EquateWith(Positions);
        RelativePositionsToCenter.EquateWith(Positions);

        ComputeRotationOffset(Positions);

        PositionRelativeToWorld(Positions);
        PositionRelativeToWorld(ZeroAnglePositions);
    }

    private void PositionRelativeToWorld(Resources.CornersBox corners)
    {
        corners._bottomLeft += new Vector2(transform.position.x, transform.position.y);
        corners._bottomRight += new Vector2(transform.position.x, transform.position.y);
        corners._topRight += new Vector2(transform.position.x, transform.position.y);
        corners._topLeft += new Vector2(transform.position.x, transform.position.y);
    }

    private void ComputeNormalPositions(Resources.CornersBox corners)
    {
        corners._bottomLeft.x = corners._topLeft.x
           = (_boxCollider.offset.x - _boxCollider.size.x / 2) * transform.localScale.x;

        corners._topLeft.y = corners._topRight.y
            = (_boxCollider.offset.y + _boxCollider.size.y / 2) * transform.localScale.y;

        corners._topRight.x = corners._bottomRight.x
            = (_boxCollider.offset.x + _boxCollider.size.x / 2) * transform.localScale.x;

        corners._bottomLeft.y = corners._bottomRight.y
            = (_boxCollider.offset.y - _boxCollider.size.y / 2) * transform.localScale.y;

    }

    private void ComputeRotationOffset(Resources.CornersBox corners)
    {
        var angle = transform.rotation.eulerAngles.z;
        Resources.ReverseRotatePositionVector(ref corners._bottomLeft, angle);
        Resources.ReverseRotatePositionVector(ref corners._bottomRight, angle);
        Resources.ReverseRotatePositionVector(ref corners._topLeft, angle);
        Resources.ReverseRotatePositionVector(ref corners._topRight, angle);
    }
}
