using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonMainMenu : MonoBehaviour
{
    public void OnButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void ToCombat()
    {
        SceneManager.LoadScene("Combat", LoadSceneMode.Single);
    }
}
