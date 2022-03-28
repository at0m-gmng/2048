using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public static int FieldSize { get; private set; } // хранит размер поля
    public static int Points { get; private set; } // хранит кол-во очков

    public static int HightSCorePoints { get; set; } // хранит наименьшее кол-во очков при победе
    public static bool GameStarted { get; private set; } // флаг определяющий начало игры


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        StartGame();
        SetFieldSize(Field.Instance.FieldSize);
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
    }

    private void hightScoreResult()
    {
        if (HightSCorePoints == 0)
        {
            //Debug.Log("HightSCorePoints: " + HightSCorePoints + "<==>" + "Points: " + Points);
            HightSCorePoints = Points;
            hightScoreResultText.text = Points.ToString();
        } else if (HightSCorePoints > Points) 
        {
            //Debug.Log("HightSCorePoints: " + HightSCorePoints + "<==>" + "Points: " + Points);
            HightSCorePoints = Points;
            hightScoreResultText.text = Points.ToString();
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
        SetFieldSize(Field.Instance.FieldSize);
    }

    public void lessFieldSize()
    {
        Field.Instance.FieldSize--;
        Field.Instance.ClearField();
        SetPoints(0); // обнуляем очки перед началом игры
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
