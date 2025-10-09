using UnityEngine;
using UnityEngine.SceneManagement;

public class GunReset : MonoBehaviour
{
    // Zet hier de gewenste positie en rotatie t.o.v. de camera
    public Vector3 defaultLocalPosition = new Vector3(0.5f, -0.5f, 1f);
  

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset de lokale transform
        transform.localPosition = defaultLocalPosition;
        
    }
}


