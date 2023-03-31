using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleLightBeatController : MonoBehaviour
{
    public AudioSource audioSource; // the audio source to react to

    private Light spotlightLight; // the light component to change color

    public float duration = 0.8f;

    public float scalingFactor;
    private float[] samples = new float[8192];

    void Start()
    {
        spotlightLight = gameObject.transform.Find("DirectionControl/Spotlight").GetComponent<Light>();
    }

    void Update()
    {
        if (audioSource.isPlaying)
        {
            audioSource.GetOutputData(samples, 0);

            float sum = 0;

            for (int i = 0; i < samples.Length; i++)
            {
                sum += Mathf.Abs(samples[i]);
            }

            float average = sum / samples.Length * scalingFactor;
            
            if (average < 1.0f)
                average = UnityEngine.Random.Range(0.9f, 1.1f);

            spotlightLight.intensity = average;
            //spotlightLight.color = new Color(average, average, average);

            float t = Mathf.PingPong(Time.time, duration) / duration;
            spotlightLight.color = Color.Lerp(Color.red, Color.blue, t);

        }
    }
}

