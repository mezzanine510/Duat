using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadePlaneFader : MonoBehaviour
{
    [SerializeField]
    float earlySpeed = 0.1f;

    [SerializeField]
    float lateSpeed = 1f;

    [SerializeField]
    float delay = 1f;

    GameObject fadePlane;
    MeshRenderer meshRenderer;
    Material material;
    Color color;

    void Start()
    {
        fadePlane = transform.gameObject;
        meshRenderer = fadePlane.GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        color = material.color;
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delay);

        Color newColor = material.GetColor("_BaseColor");

        while (material.GetColor("_BaseColor").a > 0.99f)
        {
            newColor.a -= earlySpeed * Time.deltaTime;
            material.SetColor("_BaseColor", newColor);
            yield return null;
        }

        while (material.GetColor("_BaseColor").a > 0f)
        {
            newColor.a -= lateSpeed * Time.deltaTime;
            material.SetColor("_BaseColor", newColor);
            yield return null;
        }

        newColor.a = 0;
        material.SetColor("_BaseColor", newColor);
    }

    public void FadeOutIntro()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        Debug.Log("FADE PLANE FADER STARTED");
        Color newColor = material.GetColor("_BaseColor");

        while (material.GetColor("_BaseColor").a < 1f)
        {
            newColor.a += .3f * Time.deltaTime;
            material.SetColor("_BaseColor", newColor);
            yield return null;
        }

        SceneManager.LoadScene(1);
    }
}
