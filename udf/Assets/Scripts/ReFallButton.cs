using UnityEngine;
using UnityEngine.SceneManagement;

public class ReFallButton : MonoBehaviour
{
    void Restart()
    {
        PlayerPrefs.SetInt("Restart", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
