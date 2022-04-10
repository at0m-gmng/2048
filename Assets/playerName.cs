using UnityEngine;
using TMPro;

public class playerName : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Start()
    {
        inputField = transform.Find("InputField").GetComponent<TMP_InputField>();
    }

    public void SendName()
    {
        GameManager.Instance.playerName = inputField.text;
        Hide();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
