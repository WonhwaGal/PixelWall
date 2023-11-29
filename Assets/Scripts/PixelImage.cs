using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PixelImage : IDisposable
{
    public List<Color> TextureColors = new();
    public SinglePixel[] SinglePixels;
    public int TextureSize;
    public bool IsMirror = false;

    private Sprite[] _subSprites;
    private ColorFilter _filter;

    public PixelImage(bool isMirror, ColorFilter filter)
    {
        IsMirror = isMirror;
        _filter = filter;
        _filter.AssignColors(TextureColors);
    }

    public void CreatePixels(string imageName)
    {
        _subSprites = Resources.LoadAll<Sprite>(imageName);
        SetUp();

        for (int i = 0; i < _subSprites.Length; i++)
            SinglePixels[i] = CreateSinglePixel(_subSprites[i]);
        Debug.Log($"{imageName}: number of pixels: {SinglePixels.Length}, number of colors: {TextureColors.Count}");
    }

    private void SetUp()
    {
        SinglePixels = new SinglePixel[_subSprites.Length];
        if (_subSprites[0].texture.width > _subSprites[0].texture.height)
            TextureSize = _subSprites[0].texture.width;
        else
            TextureSize = _subSprites[0].texture.height;
    }

    private SinglePixel CreateSinglePixel(Sprite spr)
    {
        Color pixelCenter = spr.texture.GetPixel((int)spr.rect.x, (int)spr.rect.y);
        if (_filter.HasMatch(pixelCenter))
            pixelCenter = _filter.CurrentMatchedColor;
        else
            TextureColors.Add(pixelCenter);

        if (_filter.IsToIgnore(pixelCenter))
            return new SinglePixel();

        var pixel = new SinglePixel(TextureColors.IndexOf(pixelCenter), spr.rect.x, spr.rect.y, (byte)spr.rect.width);
        return pixel;
    }

    public void Dispose()
    {
        TextureColors.Clear();
        GC.SuppressFinalize(this);
    }
}