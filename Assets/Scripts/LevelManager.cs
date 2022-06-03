using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject musicManager;

    void Start()
    {
        if (GameObject.Find(gameObject.name)) Destroy(gameObject);
        
        if (GameObject.Find(musicManager.name)) return;
        Instantiate(musicManager);        
    }
}
