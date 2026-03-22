using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReturnToLessonSelection()
    {
        SceneManager.LoadScene("LessonSelection");
    }
}