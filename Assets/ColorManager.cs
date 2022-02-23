using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// хранит массив всех возможных цветов плиток
public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] CellColor; // хранит цвета плиток

    [Space(5)]
    public Color PointsDarkColor; // у плиток 2 и 4 цвет тёмный, у остальных светлый
    public Color PointsLightColor;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
