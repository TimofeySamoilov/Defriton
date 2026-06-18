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
        Debug.Log("NewGame <<<<");
        menuPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        Debug.Log("OpenSettings <<<<");
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        Debug.Log("CloseSettings <<<<");
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}