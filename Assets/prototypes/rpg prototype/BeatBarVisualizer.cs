using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatBarVisualizer : MonoBehaviour
{
    // Start is called before the first frame update
    public Gradient barVisualizerGradient;
    public Slider slider;
    public float recordStartBeat;
    public Image fill;

    public void SetGradient(List<float> beats, float tolerance)
    {
        GradientColorKey[] colorKeys = new GradientColorKey[beats.Count * 2];

        //hard coded for 2 and 4 rn
        colorKeys[0].time = (beats[0] - tolerance) / 4;
        colorKeys[0].color = Color.green;
        colorKeys[1].time = (beats[0] + tolerance) / 4;
        colorKeys[1].color = Color.red;
        colorKeys[2].time = (beats[1] - tolerance) / 4;
        colorKeys[2].color = Color.green;
        colorKeys[3].time = (beats[1] + tolerance) / 4;
        colorKeys[3].color = Color.red;

        barVisualizerGradient.SetKeys(colorKeys, new GradientAlphaKey[] { });
    }

    public void InitSlider()
    {
        // InitIndicators();
        recordStartBeat = LightWeightTimeManager.current.songPositionInBeats;
        slider.maxValue = 4;
    }


    public void UpdateSlider()
    {
        float sliderValue = (LightWeightTimeManager.current.songPositionInBeats - recordStartBeat);
        if (sliderValue < 4)
        {
            slider.value = sliderValue;
            //fill.color = barVisualizerGradient.Evaluate(sliderValue);
        }
    }

    /*so we need to instantiate these at the positions the beat occur at 
    the length of the bar is 300 pixel
    so the positions on a 1-4 scale can be converted via : 
        -beatPosition * 75
    */
    public GameObject indicator, indicatorContainer;
    public void InitIndicators()
    {
        Vector3 indicatorPosition = indicatorContainer.transform.position + new Vector3(1 * 75, 0, 0);
        Instantiate(indicator, indicatorPosition, Quaternion.identity, indicatorContainer.transform);

        Vector3 indicatorPosition2 = indicatorContainer.transform.position + new Vector3(3 * 75, 0, 0);
        Instantiate(indicator, indicatorPosition2, Quaternion.identity, indicatorContainer.transform);
    }
}
