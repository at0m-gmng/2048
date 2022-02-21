using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Fild Properties")]
    public float CellSize; // размер плитки
    public float Spacing; // отступ между плитками
    public int FieldSize; // размер поля
    public int InitCellsCount; // кол-во заполненных плиток 

    [Space(10)]
    [SerializeField]
    private Cell cellPref; // префаб плитки
    [SerializeField]
    private RectTransform rt;

    private Cell[,] field; // храним всё поле в двумерном массиве

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        GenerateField();
    }

    //создаём поле
    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize]; //инициализируем массив
        float fieldWidth = FieldSize * (CellSize + Spacing) + Spacing; // считаем ширину поля
        rt.sizeDelta = new Vector2(fieldWidth, fieldWidth); // устанавливаем размер на canvas

        float startX = -(fieldWidth / 2) + (CellSize / 2) + Spacing; // нач.позиции для первой клетки
        float startY =  (fieldWidth / 2) - (CellSize / 2) - Spacing;

        //заполняем поле и создаём объекты по префабу
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y <FieldSize; y++ )
            {
                var cell = Instantiate(cellPref, transform, false);
                var position = new Vector2(startX + (x * (CellSize + Spacing)), startY - (y * (CellSize + Spacing)));
                cell.transform.localPosition = position;

                field[x, y] = cell; // созданный объект заносим в массив

                cell.SetValue(x, y, 0); // передаём х и у плитке
            }
        }
    }

    // очистка поля и подготовка к новой игре
    public void GenerateField()
    {
        if (field == null)
            CreateField();

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].SetValue(x, y, 0);

        for (int i = 0; i < InitCellsCount; i++)
            GenerateRandomCell();
    }

    // генерация 2ух начальных плиток
    private void GenerateRandomCell()
    {
        var emptyCells = new List<Cell>(); 

        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                if (field[x, y].IsEmpty)
                    emptyCells.Add(field[x, y]); // добавляем в список все пустые клетки

        if (emptyCells.Count == 0)
            throw new System.Exception("There is no any empty cell!"); // проверка на пустые клетки

        int value = Random.Range(0, 10) == 0 ? 2 : 1; // 90% номинал 2, 10% номинал 4
        var cell = emptyCells[Random.Range(0, emptyCells.Count)]; // заносим значения в рандомную плитку
        cell.SetValue(cell.X, cell.Y, value);

    }
}
