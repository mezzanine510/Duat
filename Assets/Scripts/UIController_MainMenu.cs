using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class UIController_MainMenu : MonoBehaviour
{
    public Label title;
    public Button startButton;
    public Button exitButton;
    

    [SerializeField]
    float titleFadeDelay = 7.5f;

    [SerializeField]
    float startButtonFadeDelay = 10f;

    [SerializeField]
    float exitButtonFadeDelay = 11f;

    [SerializeField] GameObject introTextGameObject;

    IntroText introText;

    void Start()
    {
        introText = introTextGameObject.GetComponent<IntroText>();
        var root = GetComponent<UIDocument>().rootVisualElement;
        title = root.Q<Label>("Title");
        startButton = root.Q<Button>("StartButton");
        exitButton = root.Q<Button>("ExitButton");

        StartCoroutine(FadeInLabel(title, titleFadeDelay));
        StartCoroutine(FadeInButton(startButton, startButtonFadeDelay));
        StartCoroutine(FadeInButton(exitButton, exitButtonFadeDelay));
        StartCoroutine(ActivateButtons(10f));
    }

    private void StartButtonPressed()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        startButton.clicked -= StartButtonPressed;
        exitButton.clicked -= ExitButtonPressed;
        
        StartCoroutine(FadeOutLabel(title, 0));
        StartCoroutine(FadeOutButton(startButton, 0));
        StartCoroutine(FadeOutButton(exitButton, 0));
        
        yield return new WaitForSeconds(7f);

        StartCoroutine(introText.StartIntro());

        yield break;
    }

    private void ExitButtonPressed()
    {
        Application.Quit();
    }

    private IEnumerator FadeInLabel(Label label, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        title.style.opacity = 1f;
        yield break;
    }

    private IEnumerator FadeInButton(Button button, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        button.style.opacity = 1f;
        yield break;
    }

    private IEnumerator FadeOutLabel(Label label, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        label.style.opacity = 0;
        yield break;
    }

    private IEnumerator ActivateButtons(float delay)
    {
        yield return new WaitForSeconds(delay);
        startButton.clicked += StartButtonPressed;
        exitButton.clicked += ExitButtonPressed;
        yield return null;
    }

    private void DisableButtons()
    {
        startButton.clicked -= StartButtonPressed;
        exitButton.clicked -= ExitButtonPressed;
    }

    private IEnumerator FadeOutButton(Button button, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        button.style.opacity = 0;
        yield break;
    }
}
