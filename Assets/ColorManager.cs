using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ������ ���� ��������� ������ ������
public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;

    public Color[] CellColor; // ������ ����� ������

    [Space(5)]
    public Color PointsDarkColor; // � ������ 2 � 4 ���� �����, � ��������� �������
    public Color PointsLightColor;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
