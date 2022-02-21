using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // X(hor) � Y(ver) ������������ ������ � �������
    public int Y { get; private set; }
    public int Value { get; private set; } // ������� ������ � ������� 2
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // ������� ������ � ������� ���� 
    public bool IsEmpty => Value == 0; // ������ ������ ��� ���
    public const int MaxValue = 11; // 2048

    [SerializeField] private Image image; // ��� ����� ����� ������
    [SerializeField] private TextMeshProUGUI points; // ��� ����������� ��������

    public void SetValue(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;

        UpdateCell();
    }

    // ���������� ������� � �������� ���� ������
    public void UpdateCell()
    {
        points.text = IsEmpty ? string.Empty : Points.ToString(); // ������ ������ => ����� ������ ������, ����� ���-�� �����

        // ������������� ����� �������� � ������ � ����������� �� value
        points.color = Value <= 2 ? ColorManager.Instance.PointsDarkColor : ColorManager.Instance.PointsLightColor; 
        image.color = ColorManager.Instance.CellColor[Value];
    }

}
