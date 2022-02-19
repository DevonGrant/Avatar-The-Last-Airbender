using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadScene("Instructions", LoadSceneMode.Single);
   }

    public void GoToArt()
    {
        SceneManager.LoadScene("ArtStagingArea", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
