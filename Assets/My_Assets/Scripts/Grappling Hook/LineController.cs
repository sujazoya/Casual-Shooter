using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] Texture[] texures;
    private int animationStep;

    [SerializeField] float fps = 30;
    float fpsCounter;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        fpsCounter += Time.deltaTime;

        if (fpsCounter >= 1f / fps)
        {
            animationStep++;
            if (animationStep == texures.Length)
                animationStep = 0;
            lineRenderer.material.SetTexture("_MainTex", texures[animationStep]);

            fpsCounter = 0;
        }
    }
}
