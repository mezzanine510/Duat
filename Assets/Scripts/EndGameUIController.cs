using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EndGameUIController : MonoBehaviour
{
    public VisualElement background;
    public Label endTitle;
    public Button exitButton;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        background = root.Q<VisualElement>("Background");
        endTitle = root.Q<Label>("EndTitle");
        exitButton = root.Q<Button>("ExitButton");

        StartCoroutine(FadeInScene());
    }

    public IEnumerator FadeInScene()
    {
        yield return new WaitForSeconds(3f);
        background.style.opacity = 0;
    }

    public IEnumerator FadeOutScene()
    {
        background.style.opacity = 1f;

        yield return new WaitForSeconds(2f);

        endTitle.style.opacity = 1f;

        yield return new WaitForSeconds(2f);

        exitButton.style.opacity = 1f;
        exitButton.clicked += ExitButtonPressed;
    }

    public void EndGame()
    {
        StartCoroutine(FadeOutScene());
    }
    
    private void ExitButtonPressed()
    {
        Application.Quit();
    }

}
