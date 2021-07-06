using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightColor : MonoBehaviour
{
    private Light light;

    public Material mat;
    public Material matModel;

    public float lightChangeduration = 10;
    public float lightChangePercent = 0.3f;
    void Start()
    {
        mat = RenderSettings.skybox;
        StartCoroutine(ColorChangeRoutine());
    }

    private IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            light = GetComponent<Light>();
            var startColor = light.color;
            var endColor = new Color32(System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), System.Convert.ToByte(Random.Range(0, 255)), 255);
            endColor = ChangeColorBrightness(endColor, lightChangePercent);
            float t = 0;
            while (t < 1)
            {
                t = Mathf.Min(1, t + Time.deltaTime); // Multiply Time.deltaTime by some constant to speed/slow the transition.
                light.color = Color.Lerp(startColor, endColor, t);
                mat.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(lightChangeduration);
        }
    }
    private IEnumerator ChangeMaterialColor()
    {
        int i = 0;
        while (true)
        {
            light = GetComponent<Light>();
            Color[] cols = { Color.red, new Color(1, 50/255f, 0),Color.yellow, Color.green, new Color(0, 80/255f, 1), Color.blue, new Color(80 / 255f, 0, 1) };
            var startColor = cols[i];
            Color endColor;
            if (i < cols.Length - 1)
            {
                endColor = cols[i + 1];
            }
            else
            {
                i = 0;
                endColor = cols[i];
            }
            float t = 0;
            while (t < 1)
            {
                t = Mathf.Min(1, t + Time.deltaTime); // Multiply Time.deltaTime by some constant to speed/slow the transition.
                light.color = Color.Lerp(startColor, endColor, t);
                mat.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }
            i++;
            yield return new WaitForSeconds(5f);
        }
    }

    private Color32 ChangeColorBrightness(Color32 color, float correctionFactor)
    {
        float red = (float)color.r;
        float green = (float)color.g;
        float blue = (float)color.b;

        if (correctionFactor < 0)
        {
            correctionFactor = 1 + correctionFactor;
            red *= correctionFactor;
            green *= correctionFactor;
            blue *= correctionFactor;
        }
        else
        {
            red = (255 - red) * correctionFactor + red;
            green = (255 - green) * correctionFactor + green;
            blue = (255 - blue) * correctionFactor + blue;
        }

        return new Color32(System.Convert.ToByte((int)red), System.Convert.ToByte((int)green), System.Convert.ToByte((int)blue), System.Convert.ToByte(color.a));
    }

}
