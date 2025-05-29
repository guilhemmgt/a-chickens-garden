using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene(string nameScene)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(nameScene);
    }

    public void Reload()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        LoadScene("Menu");
    }

    private void OnApplicationQuit()
    {
        DOTween.KillAll();
    }

    public void Quit()
    {
        Application.Quit();
    }

}