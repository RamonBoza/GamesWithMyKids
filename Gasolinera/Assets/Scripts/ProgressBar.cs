using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public void SetProgress01(float t)
    {
        if (slider) slider.value = Mathf.Clamp01(t);
    }

    public void Show(bool on)
    {
        gameObject.SetActive(on);
    }
}
