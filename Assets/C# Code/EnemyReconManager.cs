using UnityEngine;

public class EnemyReconManager : MonoBehaviour
{
    public Resources.Transform LastKnownPosition { get; private set; }
    public Resources.EnemyReconState State { get; private set; }
    public bool IsDetected { get; set; }

    public GameObject Sprite;

    private SpriteRenderer _renderer;
    private SpriteRenderer _fogRenderer;
    private Transform _initialSpriteTransform;
    private float _scaleMultiplicationFactor;
    private Vector3 _detectedLocalScale;

    public void Start()
    {
        LastKnownPosition = new Resources.Transform();
        IsDetected = false;
        State = Resources.EnemyReconState.Invisible;
        
        _renderer = Sprite.GetComponent<SpriteRenderer>();
        if (_renderer == null)
            Debug.LogError("No Sprite Renderer Child Found");

        var fog = Sprite.GetComponentInChildren<BackgroundFog>();
        if (fog == null)
            Debug.LogError("Fog not found");

        _fogRenderer = fog.gameObject.GetComponent<SpriteRenderer>();
        if (_fogRenderer == null)
            Debug.LogError("Fog Renderer not found");

        _initialSpriteTransform = Sprite.transform;

        LastKnownPosition.position = transform.position;
        LastKnownPosition.rotation = transform.rotation;
        LastKnownPosition.localScale = _initialSpriteTransform.localScale;

        _scaleMultiplicationFactor = Sprite.transform.localScale.x;

        if (Sprite.transform.localScale.x != Sprite.transform.localScale.y)
            Debug.LogError("ConditionNotImplemented: Equate x and y components of Scale of the Sprite GameObject");

        DetermineRendererStateFromState();
    }

    public void Update()
    {
        CalculateState();
        CalculateLastKnownPosition();
        DetermineRendererStateFromState();

        switch (State)
        {
            case Resources.EnemyReconState.Detected:
                OnDetection();
                break;

            case Resources.EnemyReconState.Invisible:

                break;

            case Resources.EnemyReconState.LKP:
                OnLostDetection();
                break;
        }

        PerformSpriteTranslation();
    }

    public void OnDetection()
    {

    }

    public void OnLostDetection()
    {

    }

    private void DetermineRendererStateFromState()
    {
        switch (State)
        {
            case Resources.EnemyReconState.Invisible:
                _renderer.enabled = false;
                _fogRenderer.enabled = false;
                break;

            case Resources.EnemyReconState.Detected:
                _renderer.enabled = true;
                _fogRenderer.enabled = false;
                break;

            case Resources.EnemyReconState.LKP:
                _renderer.enabled = true;
                _fogRenderer.enabled = true;
                break;
        }
    }

    private void CalculateLastKnownPosition()
    {
        var newScaleMultiplicationFactor =
            new Vector2(_scaleMultiplicationFactor * _detectedLocalScale.x, _scaleMultiplicationFactor * _detectedLocalScale.y);

        switch (State)
        {
            case Resources.EnemyReconState.Invisible:
                LastKnownPosition.position = transform.position;
                LastKnownPosition.rotation = transform.rotation;
                LastKnownPosition.localScale = new Vector3(1, 1, 1) * _scaleMultiplicationFactor;
                _detectedLocalScale = transform.localScale;
                break;

            case Resources.EnemyReconState.Detected:
                LastKnownPosition.position = transform.position;
                LastKnownPosition.rotation = transform.rotation;
                LastKnownPosition.localScale = new Vector3(1, 1, 1) * _scaleMultiplicationFactor;
                _detectedLocalScale = transform.localScale;
                break;

            case Resources.EnemyReconState.LKP:
                LastKnownPosition.localScale.x = newScaleMultiplicationFactor.x / transform.localScale.x;
                LastKnownPosition.localScale.y = newScaleMultiplicationFactor.y / transform.localScale.y;
                break;
        }
    }

    private void CalculateState()
    {
        if (IsDetected)
        {
            State = Resources.EnemyReconState.Detected;
        }
        else
        {
            if (State == Resources.EnemyReconState.Detected)
                State = Resources.EnemyReconState.LKP;
        }
    }

    private void PerformSpriteTranslation()
    {
        Sprite.transform.position = LastKnownPosition.position;
        Sprite.transform.rotation = LastKnownPosition.rotation;
        Sprite.transform.localScale = LastKnownPosition.localScale;
    }
}