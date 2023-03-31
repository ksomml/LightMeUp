using UnityEngine;

public class BounceToMusic : MonoBehaviour
{
    public AudioSource audioSource;

    [Range(0.0f, 2.0f)]
    public float bounceIntensity = 1.0f;

    [Range(0.0f, 0.5f)]
    public float beatIntensity = 0.1f;

    [Range(0.0f, 0.1f)]
    public float smoothingSpeed = 0.025f;

    private float[] spectrumData;
    private float currentSample;
    private Vector3 scaleAnchor;
    private Vector3 positionAnchor;

    void Start()
    {
        spectrumData = new float[256];
        scaleAnchor = gameObject.GetComponent<Transform>().localScale;
        positionAnchor = gameObject.GetComponent<Transform>().position;
    }

    void Update()
    {
        if (audioSource == null || audioSource.clip == null)
            return;

        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);
        currentSample = 0f;

        for (int i = 0; i < spectrumData.Length; i++)
            if (spectrumData[i] > currentSample)
                currentSample = spectrumData[i];

        if (currentSample < 0.1f)
            currentSample *= 0.7f;

        float randomValue = UnityEngine.Random.Range(0.9f, 1.1f);

        // Up and down bouncing
        float positionY = currentSample * bounceIntensity + positionAnchor.y;
        Vector3 targetPosition = new Vector3(positionAnchor.x, positionY, positionAnchor.z);
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, smoothingSpeed);

        // Scaling to the beat
        float scaleX = currentSample * beatIntensity + scaleAnchor.x;
        float scaleY = currentSample * beatIntensity + scaleAnchor.y;
        float scaleZ = currentSample * beatIntensity + scaleAnchor.z;
        Vector3 targetScale = new Vector3(scaleX, scaleY, scaleZ);
        gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, targetScale * randomValue, smoothingSpeed);

    }
}
