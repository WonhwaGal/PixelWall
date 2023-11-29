using System;

[Serializable]
public struct SinglePixel
{
    public int ColorIndex;
    public float XPos; 
    public float YPos;
    public byte Width;
    
    public SinglePixel(int colorIndex, float x, float y, byte width)
    {
        ColorIndex = colorIndex;
        XPos = x;
        YPos = y;
        Width = width;
    }
}