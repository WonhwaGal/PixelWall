using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorFilter
{
    public Color[] ColorsToIgnore;
    public float RTolerance;
    public float BTolerance;
    public float GTolerance;

    private List<Color> _colorsInUse;
    private int _colorsOutOfnBounds;

    public Color CurrentMatchedColor { get; private set; }

    public void AssignColors(List<Color> textureColors) => _colorsInUse = textureColors;

    public bool HasMatch(Color pixelCenter)
    {
        for (int i = 0; i < _colorsInUse.Count; i++)
        {
            if (IsMatchFound(_colorsInUse[i], pixelCenter))
            {
                CurrentMatchedColor = _colorsInUse[i];
                return true;
            }
        }
        return false;
    }

    public bool IsToIgnore(Color pixelCenter)
    {
        for (int i = 0; i < ColorsToIgnore.Length; i++)
        {
            if (IsMatchFound(ColorsToIgnore[i], pixelCenter))
                return true;
        }
        return false;
    }

    private bool IsMatchFound(Color registedredColor, Color pixelColor)
    {
        _colorsOutOfnBounds = 0;
        Compare(registedredColor.r - pixelColor.r, RTolerance);
        Compare(registedredColor.b - pixelColor.b, BTolerance);
        Compare(registedredColor.g - pixelColor.g, GTolerance);

        return _colorsOutOfnBounds == 0;
    }

    private void Compare(float difference, float tolerance)
    {
        if (Mathf.Abs(difference) > tolerance)
            _colorsOutOfnBounds++;
    }
}