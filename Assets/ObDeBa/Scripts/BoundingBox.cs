using Newtonsoft.Json;
using UnityEngine;

public class BoundingBox
{
    [JsonIgnore]
    public BoundingBoxDimensions Dimensions { get; set; }

    public string Label { get; set; }

    public float Confidence { get; set; }

    [JsonProperty("center")]
    public Vector2 Center
    {
        get
        {
            return new Vector2(
                Dimensions.X + Dimensions.Width / 2,
                Dimensions.Y + Dimensions.Height / 2
            );
        }
    }

    [JsonProperty("size")]
    public Vector2 Size
    {
        get
        {
            return new Vector2(
                Dimensions.Width,
                Dimensions.Height
            );
        }
    }

    [JsonIgnore]
    public Rect Rect
    {
        get { return new Rect(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
    }

    public override string ToString()
    {
        return $"{Label}:{Confidence}, {Dimensions.X}:{Dimensions.Y} - {Dimensions.Width}:{Dimensions.Height}";
    }
}

public class DimensionsBase
{
    public float X;
    public float Y;
    public float Height;
    public float Width;
}


public class BoundingBoxDimensions : DimensionsBase { }

class CellDimensions : DimensionsBase { }
