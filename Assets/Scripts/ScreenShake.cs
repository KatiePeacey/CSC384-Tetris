using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    private Vector3 originalPos;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            randomPoint.z = originalPos.z; // Keep original z depth
            transform.localPosition = randomPoint;
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
