using System;
using System.Collections.Generic;


[Serializable]
public class PixelImageList
{
    public List<PixelImage> Images = new();
    public ColorFilter ColorFilter;

    public PixelImageList(ColorFilter colorFilter) => ColorFilter = colorFilter;
}