using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager_New : MonoBehaviour
{
    public delegate void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    public static OnSceneLoaded onSceneLoaded;
    public static Scene scene;
    public static LoadSceneMode mode;
    // Start is called before the first frame update
    void Start()
    {
        if (onSceneLoaded != null)
        {
            onSceneLoaded( scene,  mode);
        }
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   
}
