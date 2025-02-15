using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Object _scene;
    public void LoadScene()
    {
        SceneManager.LoadScene(_scene.name);
    }
}
