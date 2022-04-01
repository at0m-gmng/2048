using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    public static Field Instance;

    [Header("Fild Properties")]
    public float CellSize; // размер плитки
    public float Spacing; // отступ между плитками
    public int FieldSize; // размер поля
    private int minFieldSize = 3, maxFieldSize = 10; // min и max размерность поля
    public int InitCellsCount; // кол-во заполненных плиток 

    [Space(10)]
    [SerializeField]
    private Cell cellPref; // префаб плитки
    [SerializeField]
    private RectTransform rt;
    float canvasWidth;
    [SerializeField]
    private RectTransform cancas;

    private Cell[,] field; // храним всё поле в двумерном массиве

    private bool anyCellMoved; // перемещалась ли плитка


    private void Start()
    {
        //float canvasHeight = rt.GetComponent<RectTransform>().rect.height;
        canvasWidth = rt.GetComponent<RectTransform>().rect.width;
        //gameObject.GetComponent<RectTransform>();

        //GetComponentInParent<RectTransform>();
        SwipeController.SwipeEvent += OnInput;

        cellPref.width = CellSize; // устанавливаем размер ячеек, если не задано, то берётся из инспектора
        //cellPref.width = 100f; // устанавливаем размер ячеек, если не задано, то берётся из инспектора
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.W))
            OnInput(Vector2.up);
        if (Input.GetKeyDown(KeyCode.A))
            OnInput(Vector2.left);
        if (Input.GetKeyDown(KeyCode.S))
            OnInput(Vector2.down);
        if (Input.GetKeyDown(KeyCode.D))
            OnInput(Vector2.right);

#endif
    }

    // вызывается при начале действий игроком
    private void OnInput(Vector2 direction)
    {
        if (!GameManager.GameStarted) // началась ли игра, если нет, выходим
            return;
        anyCellMoved = false;
        ResetCellsFlags(); // обнуляем у всех флаг anyCellMoved

        Move(direction); // anyCellMoved изменяется внутри Move

        // если хоть одна плитка переместилась, генерируем рандомную
        // и вызываем проверку результата игры
        if (anyCellMoved)
        {
            GenerateRandomCell();
            CheckGameResult();
        }
    }

    // логика совершения хода
    private void Move(Vector2 direction)
    {
        // если  x>0 (ход вправо) или y<0 (ход вниз)
        // то startXY = размер поля - 1 = 3
        // иначе индекс 0
        int startXY = direction.x > 0 || direction.y < 0 ? FieldSize - 1 : 0;

        // хранит значение направления хода
        // если dir!=0, то походили горизонтально, присваиваем direction.x 
        // иначе ход горизонтально, присваиваем direction.у 
        int dir = direction.x != 0 ? (int)direction.x : -(int)direction.y;

        // в цикле проходимся по стобцам либо рядам
        // определяем cell в зависимости от hor/vert
        for (int i = 0; i < FieldSize; i++)
        {
            for (int k = startXY; k >= 0 && k < FieldSize; k -=dir)
            {
                var cell = direction.x != 0 ? field[k, i] : field[i, k];

                if (cell.IsEmpty)
                    continue;
                var cellToMerge = FindCellToMerge(cell, direction); // найдём плитку для объединения
                if(cellToMerge != null) // если она нашлась
                {
                    cell.MergeWithCell(cellToMerge); // то вызываем MergeWithCell
                    anyCellMoved = true;
                    continue;
                }
                // если плитка для объединения не нашлась
                // ищем пустую для перемещения
                var emptyCell = FindEmptyCell(cell, direction);
                if(emptyCell!=null)
                {
                    cell.MoveToCell(emptyCell);
                    anyCellMoved = true;
                }
            }
        }

    }

    // поискл плитки для объединения
    private Cell FindCellToMerge(Cell cell, Vector2 direction)
    {
        // определяем Х и У первой след плитки по ходу движения
        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        // в цикле проверяем, что не вышли за границы поля
        for(int x = startX, y = startY; 
            x>=0 && x < FieldSize && y>=0 && y< FieldSize;
            x+= (int)direction.x, y-= (int)direction.y)
        {
            if (field[x, y].IsEmpty)
                continue;
            // если плитка не пустая, проверяем номинал
            // в отношении нашей плитки и объединялась ли она на этом ходу 
            if (field[x, y].Value == cell.Value && !field[x, y].HasMerged)
                return field[x, y];

            break;
        }
        return null; // объединиться не с кем
    }

    // поиск пустой плитки для перемещения
    private Cell FindEmptyCell(Cell cell, Vector2 direction)
    {
        Cell emptyCell = null;

        int startX = cell.X + (int)direction.x;
        int startY = cell.Y - (int)direction.y;

        // если клетка пустая, заносим в emptyCell
        for (int x = startX, y = startY; 
            x>=0 && x < FieldSize && y>=0 && y< FieldSize;
            x+= (int)direction.x, y-= (int)direction.y)
        {
            if (field[x, y].IsEmpty)
                emptyCell = field[x, y];
            else
                break;
        }
        return emptyCell;
    }

    // проверка поля на состояние выигрыша/проигрыша
    private void CheckGameResult()
    {
        bool lose = true;
        for (int x = 0; x < FieldSize; x++)
        {
            for (int y = 0; y < FieldSize; y++)
            {
                // если макс значение, то победа и выход
                if(field[x,y].Value == Cell.MaxValue)
                {
                    GameManager.Instance.Win();
                    return;
                }
                // проверка на то, что проигрыш не настал
                if( lose &&
                    field[x,y].IsEmpty 
                    || FindCellToMerge(field[x, y], Vector2.left)
                    || FindCellToMerge(field[x, y], Vector2.right)
                    || FindCellToMerge(field[x, y], Vector2.up)
                    || FindCellToMerge(field[x, y], Vector2.down))
                {
                    lose = false;
                }
            }
        }
        // проигрыш
        if (lose)
            GameManager.Instance.Lose();

    }

    //private void Start() // старт игры теперь контролирует GameController
    //{
    //    GenerateField();
    //}

    //создаём поле
    private void CreateField()
    {
        field = new Cell[FieldSize, FieldSize]; //инициализируем массив
        float fieldWidth = canvasWidth; 
            //canvasWidth;
            //rtParent.renderingDisplaySize.x;
        //Debug.Log(fieldWidth);
        //cn.referenceResolution.x;
        //FieldSize * (CellSize + Spacing) + Spacing; // считаем ширину поля

        //Debug.Log(rtParent.renderingDisplaySize); //<==== найти размер канваса!!!

        //if (fieldWidth != canvasWidth) // считаем размер клеток, когда размер поля максимально
        //    fieldWidth = canvasWidth;

        Spacing = fieldWidth * 0.02f;
        CellSize = (fieldWidth - Spacing ) / FieldSize - Spacing ;
        cellPref.width = CellSize ;
        //}

        rt.sizeDelta = new Vector2(canvasWidth, canvasWidth); // устанавливаем размер на canvas
        //Vector2 asd = new Vector2(canvasWidth, canvasWidth);
        //gameObject.rect
        //fieldWidth1.pos = new Vector2(canvasWidth, canvasWidth);
        //gameObject.transform.rect

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

    public void ClearField() // очищаем поле перед созданием нового, проверяем диапозон размерности
    {
        foreach(Cell field in field)
        {
            Destroy(field.gameObject);
        }

        if (FieldSize < minFieldSize)
            FieldSize = minFieldSize;
        else if (FieldSize > maxFieldSize)
            FieldSize = maxFieldSize;

        CreateField();
        GenerateField();
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

        int value = Random.Range(0, 10) == 0 ? 2: 1; // 90% номинал 2, 10% номинал 4
        var cell = emptyCells[Random.Range(0, emptyCells.Count)]; // заносим значения в рандомную плитку
        cell.SetValue(cell.X, cell.Y, value, false); // передаём false, чтобы не обновлять плитку визуально

        CellAnimationController.Instance.SmoothAppear(cell);
    }

    private void ResetCellsFlags()
    {
        for (int x = 0; x < FieldSize; x++)
            for (int y = 0; y < FieldSize; y++)
                field[x, y].ResetFlags();
    }
}
