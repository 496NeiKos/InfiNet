using UnityEngine;
using UnityEngine.SceneManagement;

public class LessonSelectionController : MonoBehaviour
{
    public void LoadLesson1()
    {
        SceneManager.LoadScene("HardwareAssembly");
    }
}
