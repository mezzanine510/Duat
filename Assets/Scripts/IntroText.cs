using System.Collections;
using UnityEngine;
using TMPro;

public class IntroText : MonoBehaviour
{
    [SerializeField]
    float showTextDelay = 3f;

    [SerializeField]
    float transitionDelay = 1f;

    [SerializeField]
    float fadeSpeed = 1f;

    [SerializeField]
    GameObject fadePlane;
    string[] introText = {
        "Anubis,\nLord of the Duat",
        "You must lead the lost souls\nto their destination",
        "Show them your light,\nguide the way..."
    };

    TMP_Text tmpTextComponent;

    void Start()
    {
        tmpTextComponent = GetComponent<TMP_Text>();
        fadePlane = GameObject.Find("Fade Plane");
    }

    public IEnumerator StartIntro()
    {
        foreach (string text in introText)
        {
            Color newColor = new Color(255, 255, 255, 0);
            newColor.a = 0;
            float alpha = 0;
            tmpTextComponent.text = text;

            while (alpha < 1)
            {
                alpha += fadeSpeed * Time.deltaTime;
                newColor.a = alpha;
                tmpTextComponent.color = newColor;
                yield return null;
            }

            alpha = 1;

            Debug.Log("FADE IN DONE");

            yield return new WaitForSeconds(showTextDelay);

            while (alpha > 0)
            {
                alpha -= fadeSpeed * Time.deltaTime;
                newColor.a = alpha;
                tmpTextComponent.color = newColor;
                yield return null;
            }
        }

        fadePlane.GetComponent<FadePlaneFader>().FadeOutIntro();

        yield break;
    }
}