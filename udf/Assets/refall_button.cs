using UnityEngine;
using UnityEngine.SceneManagement;

public class refall_button : MonoBehaviour
{
    void restart()
    {
        PlayerPrefs.SetInt("Restart", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
