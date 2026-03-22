using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartLesson()
    {
        SceneManager.LoadScene("LessonSelection");
    }

    public void QuitGame()
    {
        Application.Quit();
#if     UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}