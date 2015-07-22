using UnityEngine;
using System.Collections.Generic;

public class FriendlyBackgroundManager : MonoBehaviour
{
    public SpriteRenderer BackgroundRenderer;
    public int CenterNumber = 1;

    public Resources.FriendlyReconType Type { get; set; }

    private List<Vector2> _positions;
    private List<Vector4> _vectorValues;
    private List<Vector4> _boolValues;
    private Vector2 _center;
    private float _radius;
    private int _numberToAdd;
    private bool _shouldAddToBool;

    public void Start()
    {
        _positions = new List<Vector2>();
        _vectorValues = new List<Vector4>();
        _boolValues = new List<Vector4>();
        _center = new Vector2(0, 0);
        Type = Resources.FriendlyReconType.Occlusion;
    }

    public void Update()
    {
        InitializeShader();
        var i = 0;
        BackgroundRenderer.material.SetVector("_Center" + CenterNumber.ToString(), new Vector4(_center.x, _center.y, 0, 0));
        BackgroundRenderer.material.SetFloat("_Radius" + CenterNumber.ToString(), _radius);

        if (Type == Resources.FriendlyReconType.Occlusion)
            while (i < _positions.Count)
            {
                _shouldAddToBool = false;
                var position1 = _positions[i];
                var position2 = _positions[i + 1];
                var vector = new Vector4(position1.x, position1.y, position2.x, position2.y);

                _vectorValues.Add(vector);
                BackgroundRenderer.material.SetVector("_Vector" + CenterNumber.ToString() + (_numberToAdd + 1).ToString(), vector);

                var boolNumber = _numberToAdd / 4;

                if (boolNumber > _boolValues.Count - 1)
                {
                    vector = new Vector4(0, 0, 0, 0);
                    _shouldAddToBool = true;
                }
                else
                {
                    vector = _boolValues[boolNumber];
                }

                var positionToAdd = _numberToAdd % 4;
                switch (positionToAdd)
                {
                    case 0:
                        vector.x = 1;
                        break;

                    case 1:
                        vector.y = 1;
                        break;

                    case 2:
                        vector.z = 1;
                        break;

                    case 3:
                        vector.w = 1;
                        break;
                }

                if (_shouldAddToBool)
                    _boolValues.Add(vector);
                else
                    _boolValues[boolNumber] = vector;

                BackgroundRenderer.material.SetVector("_Bool" + CenterNumber.ToString() + boolNumber.ToString(), vector);
                i += 2;
                _numberToAdd += 1;
            }
    }

    public void InitializeVectors()
    {
        _center = new Vector2(0, 0);
        _positions.Clear();
        _radius = 0f;
        _boolValues.Clear();
        _vectorValues.Clear();
    }

    public void AddPositions(Vector2 position1, Vector2 position2)
    {
        _positions.Add(position1);
        _positions.Add(position2);
    }

    public void Initialize(Vector2 center, float radius, Resources.FriendlyReconType Type)
    {
        _numberToAdd = 0;
        _center = center;
        _radius = radius;
    }

    private void InitializeShader()
    {
        var shaderPropertiesNumber = 0;
        var boolPropertiesNumber = 0;
        switch(CenterNumber)
        {
            case 1:
                shaderPropertiesNumber = Resources.NumberOfShaderProperties_1;
                boolPropertiesNumber = Resources.NumberOfBoolProperties_1;
                break;

            case 2:
                shaderPropertiesNumber = Resources.NumberOfShaderProperties_2;
                boolPropertiesNumber = Resources.NumberOfBoolProperties_2;
                break;
        }

        var vector = new Vector4(0, 0, 0, 0);
        for (var i = 1; i <= shaderPropertiesNumber; i++)
        {
            BackgroundRenderer.material.SetVector("_Vector" + CenterNumber.ToString() + i.ToString(), vector);
        }

        for (var i = 0; i < boolPropertiesNumber; i++)
        {
            BackgroundRenderer.material.SetVector("_Bool" + CenterNumber.ToString() + i.ToString(), vector);
        }
    }
}
