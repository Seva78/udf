using UnityEngine;
using UnityEngine.SceneManagement;

public class refall_button : MonoBehaviour
{
    void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
