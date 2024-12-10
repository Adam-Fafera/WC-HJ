using UnityEngine;
using UnityEngine.UI;
using TMPro; // Dodaj, jeœli korzystasz z TextMeshPro

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public TextMeshProUGUI percentageText;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
        UpdatePercentage(savedVolume);
    }

    void SetVolume(float value)
    {
        AudioListener.volume = value;
        UpdatePercentage(value);
        PlayerPrefs.SetFloat("Volume", value);
    }

    void UpdatePercentage(float value)
    {
        if (percentageText != null)
        {
            float percentage = value * 100f;
            percentageText.text = $"{percentage:0}%";
        }
    }
}
