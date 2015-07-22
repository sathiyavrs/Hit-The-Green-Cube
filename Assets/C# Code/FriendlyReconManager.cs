using UnityEngine;
using System.Collections.Generic;

public class FriendlyReconManager : MonoBehaviour
{
    public Resources.FriendlyReconType Type;
    public float Radius;
    public const float SkinWidth = 0.04f;
    public const float SkinRadius = 0.5f; // Degrees
    public const float SkinAngle = 0.05f; // Degrees
    public LayerMask EnemyMask;
    public float RateOfAngleIncrease = 2;
    public int NumberOfRays = 3;
    public bool DebugInformation = false;

    private float _minAngle; // Degrees
    private List<BoxCollider2D> _detectedEnemies;
    private bool _sonarDetection;
    private float _circularDetectionAngle;
    private float _angleBetweenRays;
    private FriendlyBackgroundManager _backgroundManager;
    private CircleCollider2D _sonarCollider;
    private int _debugNumberOfRays = 0;
    private int _lastTimeValue = 0;

    public void Start()
    {
        // 30 rays at minimum
        var alpha = Mathf.Acos(Resources.MinSizeOfEnemy / 2 * Radius) * Mathf.Rad2Deg;
        _minAngle = 180 - 2 * alpha;
        _minAngle -= SkinRadius;
        _detectedEnemies = new List<BoxCollider2D>();

        _sonarCollider = GetComponent<CircleCollider2D>();
        if (_sonarCollider == null)
            Debug.LogError("Sonar Collider component has not been assigned");
        if (Type == Resources.FriendlyReconType.Sonar)
        {
            _sonarDetection = true;
            _sonarCollider.radius = Radius;
            _sonarCollider.isTrigger = true;
        }

        _circularDetectionAngle = 0f;
        _angleBetweenRays = 360f / NumberOfRays;

        _backgroundManager = GetComponent<FriendlyBackgroundManager>();
        if (_backgroundManager == null)
            Debug.LogError("Background Mananger is null");
    }

    public void Update()
    {
        InitializeEnemiesForDetection();
        _backgroundManager.InitializeVectors();
        _backgroundManager.Initialize(new Vector2(transform.position.x, transform.position.y), Radius, Type);
        switch (Type)
        {
            case Resources.FriendlyReconType.Occlusion:
                OcclusionDetection();
                break;

            case Resources.FriendlyReconType.Circular:
                for (var i = 0; i < NumberOfRays; i++)
                {
                    CircularDetection(_circularDetectionAngle + _angleBetweenRays * i);
                }
                _circularDetectionAngle += RateOfAngleIncrease;
                break;

            case Resources.FriendlyReconType.Sonar:
                SonarDetection();
                break;
        }
    }

    private void CircularDetection(float angle)
    {
        var rayOrigin = new Vector2(transform.position.x, transform.position.y);
        var rayDistance = Radius;

        var rayDirection = new Vector2(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));
        var raycastHits = Physics2D.RaycastAll(rayOrigin, rayDirection, rayDistance, EnemyMask);
        if (DebugInformation)
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance);

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            var collider = (BoxCollider2D)raycastHit.collider;
            if (collider == null)
            {
                Debug.LogError("Collider has to be a box Collider for enemies!");
                continue;
            }

            var manager = collider.GetComponent<EnemyReconManager>();
            if (manager == null)
            {
                Debug.LogError("Enemies need a ReconManager");
                continue;
            }

            if (manager.IsDetected)
                continue;

            manager.IsDetected = true;
            _detectedEnemies.Add(collider);
        }

    }

    private void SonarDetection()
    {
        // Nothing here yet.
    }

    private void OcclusionDetection()
    {
        var rayOrigin = new Vector2(transform.position.x, transform.position.y);
        var rayDistance = Radius + SkinRadius;
        var angle = 0f;
        _debugNumberOfRays = 0;

        while (angle < 360f)
        {
            var rayDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(Mathf.Deg2Rad * angle));
            var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, EnemyMask);
            _debugNumberOfRays++;

            if (!raycastHit)
            {
                angle += _minAngle;
                continue;
            }

            var collider = (BoxCollider2D)raycastHit.collider;
            if (collider == null)
            {
                Debug.LogError("Enemies need only BoxColliders man!");
                angle += _minAngle;
                continue;
            }

            var script = collider.GetComponent<EnemyReconManager>();
            if (script == null)
            {
                Debug.LogError("Enemy does not have a ReconManager!");
                angle += _minAngle;
                continue;
            }

            if (collider.GetComponent<EnemyReconManager>().IsDetected)
            {
                break; // TODO:: Prove correctness of Experimental Break

                angle += _minAngle;
                continue;
            }

            if (DebugInformation)
                Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.white);
            script.IsDetected = true;
            _detectedEnemies.Add(collider);

            var position1 = new Vector2(0, 0);
            var position2 = new Vector2(0, 0);
            PerformColliderCalculations(collider, ref position1, ref position2);

            var angle1 = Resources.AngleWithUpwardsClockwise(position1 - new Vector2(transform.position.x, transform.position.y));
            var angle2 = Resources.AngleWithUpwardsClockwise(position2 - new Vector2(transform.position.x, transform.position.y));

            if (Mathf.Min(angle1, angle2) < 90f && Mathf.Max(angle2, angle1) > 180f)
            {
                if (Mathf.Max(angle1, angle2) > 270f)
                {
                    UpwardsOcclusionCheck(Mathf.Max(angle1, angle2));
                    angle = Mathf.Min(angle1, angle2);
                    DownwardsOcclusionCheck(ref angle);
                    angle += _minAngle;
                    continue;
                }
            }

            if (Mathf.Max(angle2, angle1) - Mathf.Min(angle1, angle2) > 180f)
            {
                UpwardsOcclusionCheck(Mathf.Max(angle1, angle2));
                angle = Mathf.Min(angle1, angle2);
                DownwardsOcclusionCheck(ref angle);
                angle += _minAngle;
                continue;
            }

            UpwardsOcclusionCheck(Mathf.Min(angle1, angle2));
            angle = Mathf.Max(angle1, angle2);
            DownwardsOcclusionCheck(ref angle);
            angle += _minAngle;
        }

        if (DebugInformation && _debugNumberOfRays != _lastTimeValue)
        {
            _lastTimeValue = _debugNumberOfRays;
            Debug.Log("Number of Raycasts for Detection: " + _debugNumberOfRays);
        }
        _debugNumberOfRays = 0;
    }

    private void UpwardsOcclusionCheck(float angle)
    {
        angle -= SkinAngle;
        var rayOrigin = new Vector2(transform.position.x, transform.position.y);
        var rayDistance = Radius + SkinRadius;
        var rayDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

        var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, EnemyMask);
        _debugNumberOfRays++;
        if (!raycastHit)
            return;

        var script = raycastHit.collider.GetComponent<EnemyReconManager>();
        if (script == null)
        {
            Debug.LogError("ReconManager not found");
            return;
        }

        if (script.IsDetected)
            return;

        var collider = (BoxCollider2D)raycastHit.collider;
        if (collider == null)
        {
            Debug.LogError("Box Collider is necessary for enemies");
            return;
        }

        if (DebugInformation)
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        script.IsDetected = true;
        _detectedEnemies.Add(collider);

        var position1 = new Vector2(0, 0);
        var position2 = new Vector2(0, 0);
        PerformColliderCalculations(collider, ref position1, ref position2);

        var angle1 = Resources.AngleWithUpwardsClockwise(position1 - new Vector2(transform.position.x, transform.position.y));
        var angle2 = Resources.AngleWithUpwardsClockwise(position2 - new Vector2(transform.position.x, transform.position.y));
        if (Mathf.Min(angle1, angle2) < 90f && Mathf.Max(angle2, angle1) > 180f)
        {
            if (Mathf.Max(angle1, angle2) > 270f)
            {
                UpwardsOcclusionCheck(Mathf.Max(angle1, angle2));
                return;
            }
        }

        if (Mathf.Max(angle2, angle1) - Mathf.Min(angle1, angle2) > 180f)
        {
            UpwardsOcclusionCheck(Mathf.Max(angle1, angle2));
            return;
        }
        else
        {
            UpwardsOcclusionCheck(Mathf.Min(angle1, angle2));
        }
    }

    private void DownwardsOcclusionCheck(ref float angle)
    {
        angle += SkinAngle;
        var rayOrigin = new Vector2(transform.position.x, transform.position.y);
        var rayDistance = Radius + SkinRadius;
        var rayDirection = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

        var raycastHit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, EnemyMask);
        _debugNumberOfRays++;
        if (!raycastHit)
            return;

        var script = raycastHit.collider.GetComponent<EnemyReconManager>();
        if (script == null)
        {
            Debug.LogError("ReconManager not found");
            return;
        }

        if (script.IsDetected)
            return;

        var collider = (BoxCollider2D)raycastHit.collider;
        if (collider == null)
        {
            Debug.LogError("Box Collider is necessary for enemies");
            return;
        }

        if (DebugInformation)
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        script.IsDetected = true;
        _detectedEnemies.Add(collider);

        var position1 = new Vector2(0, 0);
        var position2 = new Vector2(0, 0);
        PerformColliderCalculations(collider, ref position1, ref position2);

        var angle1 = Resources.AngleWithUpwardsClockwise(position1 - new Vector2(transform.position.x, transform.position.y));
        var angle2 = Resources.AngleWithUpwardsClockwise(position2 - new Vector2(transform.position.x, transform.position.y));

        if (Mathf.Max(angle2, angle1) - Mathf.Min(angle1, angle2) > 180f)
        {
            angle = Mathf.Min(angle1, angle2);
            DownwardsOcclusionCheck(ref angle);
            return;
        }
        if (Mathf.Min(angle1, angle2) < 90f && Mathf.Max(angle2, angle1) > 180f)
        {
            if (Mathf.Max(angle1, angle2) > 270f)
            {
                angle = Mathf.Min(angle1, angle2);
                DownwardsOcclusionCheck(ref angle);
                return;
            }
        }
        else
        {
            angle = Mathf.Max(angle1, angle2);
            DownwardsOcclusionCheck(ref angle);
        }
    }

    private void InitializeEnemiesForDetection()
    {
        foreach (BoxCollider2D enemy in _detectedEnemies)
        {
            var script = enemy.gameObject.GetComponent<EnemyReconManager>();
            script.IsDetected = false;
        }

        _detectedEnemies.Clear();
    }

    private void PerformColliderCalculations(BoxCollider2D collider, ref Vector2 position1, ref Vector2 position2)
    {
        var script = collider.GetComponent<PositionHelper>();
        if (script == null)
        {
            Debug.LogError("Detected Enemy does not have a Position Helper component");
            return;
        }

        var relativePosition
            = new Vector2(transform.position.x - collider.transform.position.x, transform.position.y - collider.transform.position.y);

        Resources.RotatePositionVectorAboutOrigin(ref relativePosition, script.AngleOfRotation);

        var cornerPositions = script.Positions;
        var cornerRelativePositions = script.RelativePositionsToCenter;

        if (Resources.IsBetween(relativePosition.x, cornerRelativePositions._topLeft.x, cornerRelativePositions._bottomRight.x))
        {
            if (relativePosition.y > cornerRelativePositions._topLeft.y)
            {
                position1 = cornerPositions._topLeft;
                position2 = cornerPositions._topRight;
                _backgroundManager.AddPositions(position1, position2);
                return;
            }

            if (relativePosition.y < cornerRelativePositions._bottomRight.y)
            {
                position1 = cornerPositions._bottomLeft;
                position2 = cornerPositions._bottomRight;
                _backgroundManager.AddPositions(position1, position2);
                return;
            }
        }

        if (Resources.IsBetween(relativePosition.y, cornerRelativePositions._bottomRight.y, cornerRelativePositions._topRight.y))
        {
            if (relativePosition.x > cornerRelativePositions._topRight.x)
            {
                position1 = cornerPositions._bottomRight;
                position2 = cornerPositions._topRight;
                _backgroundManager.AddPositions(position1, position2);
                return;
            }

            if (relativePosition.x < cornerRelativePositions._bottomRight.x)
            {
                position1 = cornerPositions._bottomLeft;
                position2 = cornerPositions._topLeft;
                _backgroundManager.AddPositions(position1, position2);
                return;
            }
        }

        if (relativePosition.x > cornerRelativePositions._topRight.x && relativePosition.y > cornerRelativePositions._topRight.y)
        {
            position1 = cornerPositions._topLeft;
            position2 = cornerPositions._bottomRight;
            _backgroundManager.AddPositions(position1, position2);
            return;
        }

        if (relativePosition.x > cornerRelativePositions._bottomRight.x && relativePosition.y < cornerRelativePositions._bottomRight.y)
        {
            position2 = cornerPositions._topRight;
            position1 = cornerPositions._bottomLeft;
            _backgroundManager.AddPositions(position1, position2);
            return;
        }

        if (relativePosition.x < cornerRelativePositions._bottomLeft.x && relativePosition.y < cornerRelativePositions._bottomLeft.y)
        {
            position1 = cornerPositions._topLeft;
            position2 = cornerPositions._bottomRight;
            _backgroundManager.AddPositions(position1, position2);
            return;
        }

        if (relativePosition.x < cornerRelativePositions._topLeft.x && relativePosition.y > cornerRelativePositions._topLeft.y)
        {
            position1 = cornerPositions._topRight;
            position2 = cornerPositions._bottomLeft;
            _backgroundManager.AddPositions(position1, position2);
            return;
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!_sonarDetection)
            return;

        var collider = (BoxCollider2D)other;
        if (collider == null)
            return;

        var script = collider.GetComponent<EnemyReconManager>();
        if (script == null)
            return;

        script.IsDetected = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (!_sonarDetection)
            return;

        var collider = (BoxCollider2D)other;
        if (collider == null)
            return;

        var script = collider.GetComponent<EnemyReconManager>();
        if (script == null)
            return;

        script.IsDetected = false;
    }
}
