using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulController : MonoBehaviour
{
    [SerializeField]
    public SoulColor soulColor;

    [SerializeField]
    public float speed = 1.0f;

    [Range(0f, 1f)]
    [SerializeField]
    float colorLerpTime = 0.1f;

    [SerializeField]
    float emissionMultiplier = 8f;

    [SerializeField]
    Light soulLight;

    [SerializeField]
    float lightTransitionSpeed = 1f;

    [SerializeField]
    float rateOfSpeedChange = 1f;

    [SerializeField]
    float maxSpeed = 2.5f;

    [SerializeField]
    Animator animator;

    private Mover mover;
    private MeshRenderer meshRenderer;
    private Material material;
    public GameObject target = null;
    public GameObject followPosition = null;
    public GameObject basePositionObject;
    public Vector3 basePosition;
    public SphereCollider sphereCollider;
    private Color baseColor;
    private float baseLightIntensity;
    public float followSpeed;

    void Awake()
    {
        basePosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<Mover>();

        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        material = meshRenderer.material;

        basePositionObject = new GameObject(transform.gameObject.name);
        basePositionObject.transform.position = basePosition;

        followSpeed = speed;

        sphereCollider = GetComponent<SphereCollider>();

        baseColor = material.GetColor("_EmissionColor");
        baseLightIntensity = soulLight.intensity;

        animator.Play("Float", 0, UnityEngine.Random.Range(0, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 direction = target.transform.position - transform.position;
            mover.Move(direction * speed * Time.deltaTime);
        }
    }

    public void BrightenLight()
    {
        StartCoroutine(IncreaseGlow());
        StartCoroutine(IncreaseLight());
        StartCoroutine(IncreaseSpeed());
    }

    private IEnumerator IncreaseGlow()
    {
        Color currentColor = new Color(baseColor.r, baseColor.g, baseColor.b, baseColor.a);
        Color targetColor = baseColor * emissionMultiplier;

        while (currentColor.r <= targetColor.r - 0.05)
        {
            currentColor = Color.Lerp(currentColor, targetColor, colorLerpTime);
            material.SetColor("_EmissionColor", currentColor);

            yield return null;
        }
    }

    private IEnumerator IncreaseLight()
    {
        while (soulLight.intensity < 3f)
        {
            soulLight.intensity += lightTransitionSpeed * Time.deltaTime;
            yield return null;
        }

        soulLight.intensity = 3f;
    }

    private IEnumerator IncreaseSpeed()
    {
        while (speed < maxSpeed)
        {
            speed += rateOfSpeedChange * Time.deltaTime;
            yield return null;
        }

        speed = maxSpeed;
    }

    public void ResetFollowSpeed()
    {
        speed = followSpeed;
    }

    public void ResetFollowTarget()
    {
        target = followPosition;
    }

    public void ResetLight()
    {
        soulLight.intensity = baseLightIntensity;
        Debug.Log("<color=green>baseLightIntensity: " + baseLightIntensity + "</color>");
    }

    public void ResetColor()
    {
        material.SetColor("_EmissionColor", baseColor);
    }

    public void ReturnToSpawn()
    {
        sphereCollider.enabled = false;
        followPosition = null;
        ResetLight();
        ResetColor();
        StartCoroutine(SetTargetToSpawn());
    }

    private IEnumerator SetTargetToSpawn()
    {
        target = basePositionObject;

        yield return new WaitForSeconds(2f);

        target = null;
        sphereCollider.enabled = true;
    }

    public void SetUnlockingAnimation()
    {
        animator.SetBool("unlocking", true);
    }

    public void SetFloatingAnimation()
    {
        animator.SetBool("unlocking", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (target) return;
        // if (other.name != "Player") return;
    }
}
