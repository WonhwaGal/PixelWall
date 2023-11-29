using System.Collections.Generic;
using UnityEngine;

public class TextureHandler : MonoBehaviour
{
    [SerializeField] private Texture2D[] _textures;
    [SerializeField] private int _showIndex;
    [SerializeField] private bool _isMirror;
    [SerializeField] private GameObject _cube;
    [SerializeField] private int _finalScale;
    [SerializeField] private ColorFilter _colorFilter;

    private PixelImageList _pixelImageList;
    private readonly JSONSaver _jsonSaver = new();
    private readonly Dictionary<Color, Material> _colorDict = new();

    private void Start()
    {
        _pixelImageList = new PixelImageList(_colorFilter);

        for (int i = 0; i < _textures.Length; i++)
        {
            PixelImage pImage = new(_isMirror, _colorFilter);
            pImage.CreatePixels(_textures[i].name);
            _pixelImageList.Images.Add(pImage);
        }

        _jsonSaver.SaveToJson(_pixelImageList);
        CreateWall(_jsonSaver.LoadFromJSON());
    }

    private void CreateWall(PixelImageList imageList)
    {
        PixelImage pixelImg = imageList.Images[_showIndex];

        sbyte mirror = (sbyte)(pixelImg.IsMirror ? -1 : 1);
        var distanceMultiplier = pixelImg.SinglePixels[0].Width;
        var sizeMultiplier = (float)_finalScale / pixelImg.TextureSize * distanceMultiplier;

        for (int i = 0; i < pixelImg.SinglePixels.Length; i++)
        {
            var pixel = pixelImg.SinglePixels[i];
            if (pixel.Width == 0)
                continue;

            var cube = PlaceCube(pixel, mirror, distanceMultiplier, sizeMultiplier);

            Color pixelColor = pixelImg.TextureColors[pixelImg.SinglePixels[i].ColorIndex];
            SetColor(pixelColor, cube);
        }
    }

    private GameObject PlaceCube(SinglePixel pixel, sbyte mirror, byte distanceMultiplier, float sizeMultiplier)
    {
        var pos = new Vector3(pixel.XPos / distanceMultiplier * mirror, 
                                pixel.YPos / distanceMultiplier, 0) * sizeMultiplier;
        var cube = Instantiate(_cube, pos, Quaternion.identity);
        cube.transform.SetParent(transform);
        cube.transform.localScale *= sizeMultiplier;

        return cube;
    }

    private void SetColor(Color pixelColor, GameObject cube)
    {
        if (!_colorDict.ContainsKey(pixelColor))
        {
            cube.GetComponent<Renderer>().material.color = pixelColor;
            _colorDict.Add(pixelColor, cube.GetComponent<Renderer>().material);
        }
        else
        {
            cube.GetComponent<Renderer>().material = _colorDict[pixelColor];
        }
    }

    private void OnDestroy()
    {
        for(int i = 0; i < _pixelImageList.Images.Count; i++)
            _pixelImageList.Images[i].Dispose();

        _pixelImageList.Images.Clear();
    }
}