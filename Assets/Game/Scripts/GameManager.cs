using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;

    void Start()
    {
        menuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void NewGame()
    {
        menuPanel.SetActive(false);
        Debug.Log("Новая игра начата");
    }

    public void OpenSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}