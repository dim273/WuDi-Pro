using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "UnderGround";
    [SerializeField] private GameObject continueButton;
    private void Start()
    {
        if(SaveManager.instance.HasSaveGame() == false)
            continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Debug.Log("ÍË³ö");
        //Application.Quit();
    }
}
