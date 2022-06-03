using System.Collections;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    GameObject UIDocument;

    EndGameUIController endGameUIController;
    
    void Start()
    {
        endGameUIController = UIDocument.GetComponent<EndGameUIController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FadeToBlack()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        endGameUIController.EndGame();
    }
}
