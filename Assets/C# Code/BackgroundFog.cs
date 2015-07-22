using UnityEngine;

public class BackgroundFog : MonoBehaviour
{
    [Range(0, 1)]
    public float Transparency;

    public int ResolutionWidth;
    public int ResolutionHeight;

    public static BackgroundFog BackgroundScript { get; private set; }
    public Texture2D Texture { get; private set; }
    public float PixelsToUnits { get; private set; }

    private SpriteRenderer _renderer;
    private Sprite _sprite;
    private int _area;

    private Vector2 _bottomLeftPosition;
    private Vector2 _topRightPosition;

    public void Start()
    {
        _area = ResolutionWidth * ResolutionHeight;
        Texture = new Texture2D(ResolutionWidth, ResolutionHeight, TextureFormat.ARGB32, false);
        _renderer = GetComponent<SpriteRenderer>();

        DeterminePixelsToUnits();
        ColourizeTexture();

        _bottomLeftPosition = new Vector2
            (transform.position.x - ResolutionWidth * transform.localScale.x / (2 * PixelsToUnits),
            transform.position.y - ResolutionHeight * transform.localScale.y / (2 * PixelsToUnits));

        _topRightPosition = new Vector2
            (transform.position.x + ResolutionWidth * transform.localScale.x / (2 * PixelsToUnits),
            transform.position.y + ResolutionHeight * transform.localScale.y / (2 * PixelsToUnits));

    }

    public Vector2 ConvertIntoPixelPosition(Vector2 position)
    {
        var xComponent = position.x - _bottomLeftPosition.x;
        var yComponent = position.y - _bottomLeftPosition.y;

        xComponent *= PixelsToUnits;
        yComponent *= PixelsToUnits;

        if (xComponent < -0.001f || xComponent > ResolutionWidth + 0.001f)
            return new Vector2(-1, -1);

        if (yComponent < -0.001f || yComponent > ResolutionHeight + 0.001f)
            return new Vector2(-1, -1);

        return new Vector2(xComponent, yComponent);
    }

    private void ColourizeTexture()
    {
        var rect = new Rect(0, 0, ResolutionWidth, ResolutionHeight);
        var pivot = new Vector2(0.5f, 0.5f);

        _sprite = Sprite.Create(Texture, rect, pivot, PixelsToUnits);
        _renderer.sprite = _sprite;

        Color[] colors = new Color[_area];
        for (var i = 0; i < _area; i++)
            colors[i] = new Color(0, 0, 0, Transparency);

        Texture.SetPixels(colors);
        Texture.Apply();
    }

    private void DeterminePixelsToUnits()
    {
        var gameObject = transform.parent.gameObject;
        if (gameObject == null)
            Debug.LogError("Fog is not parented a sprite holder");

        var renderer = gameObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
            Debug.LogError("Backgrounds is improperly set up");

        PixelsToUnits = renderer.sprite.pixelsPerUnit;

        if (string.Compare(renderer.sortingLayerName, "Backgrounds") == 0)
        {
            if (BackgroundScript != null)
                Debug.LogError("Cannot have two backgrounds with fogs");

            BackgroundScript = this;
        }
    }

}
