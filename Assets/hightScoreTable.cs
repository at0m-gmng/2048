using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class hightScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private Transform entryTemplateClone;
    private Transform entryWindow;
    private List<Transform> hightScoreEntryTransformList;

    private const int MaxScoresInTable = 10; // ������������ ���-�� �������� � �������
    private const string ScoreTableSaveKey = "ScoreTable.saveData"; // ��� ��� ���������� � PlayerPrefs

    [Space]
    [Header("DebugSaveData")]
    [SerializeField]
    private ScoreTable _scoreTable; // ����������� ������� � ���������� ��� ������

    public void Awake()
    {
        entryWindow = transform.Find("ScoreTable");
        entryContainer = entryWindow.transform.Find("ScoreTableContainer");
        entryTemplate = entryContainer.Find("ScoreCell");
        entryTemplate.gameObject.SetActive(false);

        hightScoreEntryTransformList = new List<Transform>();
        LoadTableOrDefault(); // ��������� �������, ���� ������� ������, ���� ���������� �� ����

        // ���� ������� ������, �� ��������� �������� ����
        if (_scoreTable.IsEmpty)
        {
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
            AddHightScoreAndSave(score: 10000, name: "Player");
        }
        // ��������� UI �������
        RedrawTableUI();
        //Debug.Log(JsonUtility.ToJson(_scoreTable));
    }

    public void AddHightScoreAndSave(int score, string name)
    { // ���������� ������� � ������� � �������������� 
        _scoreTable.AddScore(score, name);
        //Debug.Log(JsonUtility.ToJson(_scoreTable));
        SaveTable();
        // Debug.Log(JsonUtility.ToJson(_scoreTable));
    }

    public void SaveTable()
    { // ���������� ������� � PlayerPrefs 
        SortAndCutTable();
        PlayerPrefs.SetString(ScoreTableSaveKey, JsonUtility.ToJson(_scoreTable));
    }

    public void LoadTableOrDefault()
    { // �������� ������� �� PlayerPrefs  
        _scoreTable = JsonUtility.FromJson<ScoreTable>(PlayerPrefs.GetString(ScoreTableSaveKey));
        _scoreTable = _scoreTable ?? new ScoreTable(); //-> ���� ���������� �� ���� � ������� NULL, �� ������ ����� �������
    }

    private void SortAndCutTable()
    { // ������������� �������
        _scoreTable.ScoresList = _scoreTable.ScoresList //-> ���������� �� �������� � �������� OrderBy
                                                        .OrderBy(i => i.Score) //-> ���������� �� �������� � �������� OrderByDescending
                                                        .Take(MaxScoresInTable) //-> ��������� ������ ������ ���-�� ��������
                                                        .ToList();
    }
    private void RedrawTableUI()
    {
        foreach (ScoreData scoreData in _scoreTable.ScoresList) //��������� ����� ������� �������� 
        {
            createHightScoreEntryTransform(scoreData.Score, scoreData.Name, entryContainer, hightScoreEntryTransformList);
        }
    }

    public void Destroy()
    {
        foreach (Transform entryTemplate in entryContainer)
        {
            if (entryTemplate.name == "ScoreCell(Clone)")
            {
                Destroy(entryTemplate.gameObject);
            }
        }
    }

    private void createHightScoreEntryTransform(int score, string name, Transform container, List<Transform> transformList)
    {
        float templateHight = 155f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -5 + (-templateHight * (transformList.Count)));
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + ""; break;
            case 1:
                rankString = rank + "��"; break;
            case 2:
                rankString = rank + "��"; break;
            case 3:
                rankString = rank + "��"; break;
        }
        entryTransform.Find("position (1)").GetComponent<TextMeshProUGUI>().text = rankString;
        entryTransform.Find("score (1)").GetComponent<TextMeshProUGUI>().text = score.ToString();
        entryTransform.Find("name (1)").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    [System.Serializable]
    public class ScoreTable
    {
        public List<ScoreData> ScoresList = new List<ScoreData>();

        public bool IsEmpty => ScoresList.Count == 0;
        public void AddScore(int score, string name)
        {
            //Debug.Log(PlayerPrefs.GetString(ScoreTableSaveKey));
            ScoresList.Add(new ScoreData(score, name));
            // Debug.Log(PlayerPrefs.GetString(ScoreTableSaveKey));
        }
    }

    [System.Serializable]
    public class ScoreData
    {
        public int Score;
        public string Name;

        public ScoreData(int score, string name)
        {
            Score = score;
            Name = name;
        }
    }
}
