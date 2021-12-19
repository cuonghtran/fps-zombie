using System;
using UnityEngine;

public class UtilsClass : MonoBehaviour
{
    public static Color GetColorFromString(string color)
    {
        float red = Hex_to_Dec(color.Substring(0, 2));
        float green = Hex_to_Dec(color.Substring(2, 2));
        float blue = Hex_to_Dec(color.Substring(4, 2));
        float alpha = 1f;
        if (color.Length >= 8)
        {
            // Color string contains alpha
            alpha = Hex_to_Dec(color.Substring(6, 2));
        }
        return new Color(red, green, blue, alpha);
    }

    public static float Hex_to_Dec(string hex)
    {
        return Convert.ToInt32(hex, 16) / 255f;
    }
}
