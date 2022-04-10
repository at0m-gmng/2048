using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private TextMeshProUGUI fieldSizeResult; // отображает размер поля
    [SerializeField]
    private TextMeshProUGUI gameResult; //отображает результат игры
    [SerializeField]
    private TextMeshProUGUI pointsText; // отображает кол-во набранных очков
    [SerializeField]
    private TextMeshProUGUI hightScoreResultText; // отображает наименьшее кол-во набранных очков при победе
    [SerializeField]
    private GameObject hightScoreTablePanel;
    [SerializeField]
    private GameObject inputWindow;

    //[SerializeField]  private hightScoreTable hightScoreTable;
    public static int FieldSize { get; private set; } // хранит размер поля
    public static int Points { get; private set; } // хранит кол-во очков

    public static int HightSCorePoints { get; set; } // хранит наименьшее кол-во очков при победе
    public static bool GameStarted { get; private set; } // флаг определяющий начало игры

    public string playerName = "Player";

    private int clickCounter = 0;

    [SerializeField] private Text text;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        hightScoreTablePanel.SetActive(false);
        StartGame();
        SetFieldSize(Field.Instance.FieldSize);
    }
    private void Update()
    {
        if (hightScoreTablePanel.active)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (!inputWindow.active)
                    clickCounter++;
                text.text = clickCounter.ToString();
            }
            if (clickCounter > 1)
            {
                hightScoreTableWindowOFF();
                clickCounter = 0;
            }
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (!inputWindow.active)
            //        clickCounter++;
            //    Debug.Log(clickCounter);
            //}

            //if (clickCounter > 1)
            //{
            //    hightScoreTableWindowOFF();
            //    clickCounter = 0;
            //}
        }
    }


    public void showInputWindow()
    {
        inputWindow.SetActive(true);
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "You Win!";
        // вызываем только при выйгрыше, иначе
        // наименьшего результата будет легко достигнуть
        hightScoreResult();
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "You Lose!";
        hightScoreResult();
    }

    public void hightScoreTableWindow()
    {
        Time.timeScale = 0;
        FindObjectOfType<hightScoreTable>().Awake();
        hightScoreTablePanel.SetActive(true);
    }

    public void hightScoreTableWindowOFF()
    {
        Time.timeScale = 1;
        hightScoreTablePanel.SetActive(false);
        FindObjectOfType<hightScoreTable>().Destroy();
    }

    private void hightScoreResult()
    {
        if (HightSCorePoints == 0)
        {
            //Debug.Log("HightSCorePoints: " + HightSCorePoints + "<==>" + "Points: " + Points);
            HightSCorePoints = Points;
            hightScoreResultText.text = Points.ToString();

            FindObjectOfType<hightScoreTable>().LoadTableOrDefault();
            FindObjectOfType<hightScoreTable>().AddHightScoreAndSave(HightSCorePoints, playerName);
        } else if (HightSCorePoints < Points) 
        {
            //Debug.Log("HightSCorePoints: " + HightSCorePoints + "<==>" + "Points: " + Points);
            HightSCorePoints = Points;
            hightScoreResultText.text = Points.ToString();

            FindObjectOfType<hightScoreTable>().LoadTableOrDefault();
            FindObjectOfType<hightScoreTable>().AddHightScoreAndSave(HightSCorePoints, playerName);
        } else if(HightSCorePoints > Points)
        {
            FindObjectOfType<hightScoreTable>().LoadTableOrDefault();
            FindObjectOfType<hightScoreTable>().AddHightScoreAndSave(Points, playerName);
        }
    }

    // очищаем кол-во очков и устанавливаем флаг начала игры
    public void StartGame()
    {
        gameResult.text = "";

        SetPoints(0); // обнуляем очки перед началом игры
        GameStarted = true;

        Field.Instance.GenerateField();
    }

    // добавляет очки
    public void AddPoints(int points)
    {
        SetPoints(Points + points);
    }
    // устанавливает очки и выводит на экран
    private void SetPoints(int points)
    {
        Points = points;
        pointsText.text = Points.ToString();
    }

    public void moreFieldSize()
    {
        Field.Instance.FieldSize++;
        Field.Instance.ClearField();
        SetPoints(0); // обнуляем очки перед началом игры
        gameResult.text = "";
        SetFieldSize(Field.Instance.FieldSize);
    }

    public void lessFieldSize()
    {
        Field.Instance.FieldSize--;
        Field.Instance.ClearField();
        SetPoints(0); // обнуляем очки перед началом игры
        gameResult.text = "";
        SetFieldSize(Field.Instance.FieldSize);
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
