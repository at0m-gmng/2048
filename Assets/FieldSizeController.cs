using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldSizeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fieldSizeResult;

    public static int FieldSize { get;  private set; } // хранит размер поля

    void Start()
    {
        SetFieldSize(Field.Instance.FieldSize);
    }

    public void moreFieldSize()
    {
        Field.Instance.FieldSize++;
        SetFieldSize(Field.Instance.FieldSize);
        Field.Instance.ClearField();
    }

    public void lessFieldSize()
    {
        Field.Instance.FieldSize--;
        SetFieldSize(Field.Instance.FieldSize);
        Field.Instance.ClearField();
    }

    public void AddFieldSize(int fieldSize)
    {
        SetFieldSize(FieldSize + fieldSize);
    }
    // устанавливает очки и выводит на экран
    private void SetFieldSize(int fieldSize)
    {
        FieldSize = fieldSize;
        fieldSizeResult.text = FieldSize.ToString();
    }
}
