using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static int Points { get; private set; } // хранит кол-во очков
    public static bool GameStarted { get; private set; } // флаг определяющий начало игры

    [SerializeField]
    private TextMeshProUGUI gameResult; //результат игры
    [SerializeField]
    private TextMeshProUGUI pointsText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        StartGame();
    }

    public void Win()
    {
        GameStarted = false;
        gameResult.text = "You Win!";
    }

    public void Lose()
    {
        GameStarted = false;
        gameResult.text = "You Lose!";
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
}
