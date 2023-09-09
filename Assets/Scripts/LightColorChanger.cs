using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    public Light lightToChange;
    public Color[] colors; 
    public float colorChangeInterval = 1.0f; 
    public float colorTransitionTime = 0.5f; 

    private int currentColorIndex = 0;
    private float timeUntilNextColorChange;
    private Color startColor;
    private Color endColor;
    private float colorTransitionTimer = 0.0f;

    private void Start()
    {
        lightToChange.color = colors[currentColorIndex]; 
        timeUntilNextColorChange = colorChangeInterval;
    }

    private void Update()
    {
        timeUntilNextColorChange -= Time.deltaTime;
        if (timeUntilNextColorChange <= 0)
        {
            SetNextColor();
            timeUntilNextColorChange = colorChangeInterval;
        }
        if (colorTransitionTimer > 0.0f)
        {
            float t = (colorTransitionTime - colorTransitionTimer) / colorTransitionTime;
            lightToChange.color = Color.Lerp(startColor, endColor, t);
            colorTransitionTimer -= Time.deltaTime;
        }
    }

    private void SetNextColor()
    {
        currentColorIndex++;
        if (currentColorIndex >= colors.Length)
        {
            currentColorIndex = 0;
        }

        startColor = lightToChange.color;
        endColor = colors[currentColorIndex];
        colorTransitionTimer = colorTransitionTime;
    }
}