using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    protected Light2D light;

    [Header("Intensity parameters")]
    [SerializeField] private bool randomiseIntensity;
    [SerializeField] private float minIntensityUpdateTime;
    [SerializeField] private float maxIntensityUpdateTime;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField, Range(0, 1)] private float intensityLerping;

    [Header("Color parameters")]
    [SerializeField] private bool randomiseColor;
    [SerializeField] private float minColorUpdateTime;
    [SerializeField] private float maxColorUpdateTime;
    [SerializeField] private Gradient colorGradient;
    [SerializeField, Range(0,1)] private float colorLerping;
    


    
    private float intensityTime;
    private float intensityUpdateTime;
    private float colorTime;
    private float colorUpdateTime;
    private float colorGradientValue;

    protected virtual void Awake()
    {
        light = GetComponent<Light2D>();

        intensityTime = 0;
        intensityUpdateTime = Random.Range(minIntensityUpdateTime, maxIntensityUpdateTime);

        colorTime = 0;
        colorUpdateTime = Random.Range(minColorUpdateTime, maxColorUpdateTime);
        colorGradientValue = Random.Range(0f, 1f);
    }

    protected virtual void Update()
    {
        intensityTime += Time.deltaTime;
        colorTime += Time.deltaTime;
        if (randomiseIntensity && intensityTime > intensityUpdateTime)
        {
            intensityTime = 0;
            intensityUpdateTime = Random.Range(minIntensityUpdateTime, maxIntensityUpdateTime);
            float newIntensity = Random.Range(minIntensity, maxIntensity);
            light.intensity = Mathf.Lerp(light.intensity, newIntensity, intensityLerping);
        }
        if (randomiseColor && colorTime > colorUpdateTime)
        {
            colorTime = 0;
            colorUpdateTime = Random.Range(minColorUpdateTime, maxColorUpdateTime);
            colorGradientValue = Mathf.Lerp(colorGradientValue, Random.Range(0f, 1f), colorLerping);
            light.color = colorGradient.Evaluate(colorGradientValue);
        }
    }
}