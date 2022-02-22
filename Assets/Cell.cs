using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public int X { get; private set; } // X(hor) и Y(ver) расположение плитки в массиве
    public int Y { get; private set; }
    public int Value { get; private set; } // номинал плитки в степени 2
    public int Points => IsEmpty ? 0 : (int)Mathf.Pow(2, Value); // номинал плитки в игровом виде 
    public bool IsEmpty => Value == 0; // пустая плитка или нет
    public const int MaxValue = 11; // 2048
    public bool HasMerged { get; private set; } // объединялась ли плитка с другой

    [SerializeField] private Image image; // для смены цвета плитки
    [SerializeField] private TextMeshProUGUI points; // для отображение номинала

    public void SetValue(int x, int y, int value)
    {
        X = x;
        Y = y;
        Value = value;

        UpdateCell();
    }

    // вызывается у той ячейки, в которую объединяются
    public void IncreaseValue()
    {
        Value++;
        HasMerged = true;
        GameManager.Instance.AddPoints(Points);

        UpdateCell(); //визуально отображаем изменения
    }

    // вызываем для всех плиток перед каждым ходом игрока
    public void ResetFlags()
    {
        HasMerged = false;
    }

    // вызывается у плитки, которая вливается в плитку своего номинала
    // плитки не меняются, меняются только значения
    public void MergeWithCell(Cell otherCell)
    {
        otherCell.IncreaseValue(); // значение удвоится
        SetValue(X, Y, 0); // старое значение меняем на 0

        UpdateCell(); // отображаем изменения
    }

    // вызывается при перемещении плитки в свободную ячейку
    public void MoveToCell(Cell target)
    {
        target.SetValue(target.X, target.Y, Value); // плитке target задаём значение нашей плитки
        SetValue(X, Y, 0); // старую плитку обнуляем

        UpdateCell();

    }

    // отображает номинал и изменяет цвет плитки
    public void UpdateCell()
    {
        points.text = IsEmpty ? string.Empty : Points.ToString(); // плитка пустая => текст пустая строка, иначе кол-во очков

        // устанавливаем цвета номинала и плитки в зависимости от value
        points.color = Value <= 2 ? ColorManager.Instance.PointsDarkColor : ColorManager.Instance.PointsLightColor; 
        image.color = ColorManager.Instance.CellColor[Value];
    }

}
