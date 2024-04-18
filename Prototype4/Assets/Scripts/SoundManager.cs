using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private Slider volumeSlider;
    private TMP_Text volumeText; // TextMeshPro Text component
    private float currentVolume = 1f; // Default volume (full volume)

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Find the Volume Slider and Volume TextMeshPro Text in the scene
        volumeSlider = GameObject.FindWithTag("VolumeSlider").GetComponent<Slider>();
        volumeText = GameObject.FindWithTag("VolumeText").GetComponent<TMP_Text>();

        // Add listener to the slider value change event
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Set the initial volume based on the slider's value
        AudioListener.volume = currentVolume;

        // Update the volume text display
        UpdateVolumeText(currentVolume);
    }

    private void OnVolumeChanged(float volume)
    {
        currentVolume = volume;
        AudioListener.volume = currentVolume; // Set the game volume based on the slider value

        // Update the volume text display
        UpdateVolumeText(currentVolume);
    }

    private void UpdateVolumeText(float volume)
    {
        int volumePercentage = Mathf.RoundToInt(volume * 100f); // Convert volume value to percentage
        volumeText.text = "Volume: " + volumePercentage.ToString() + "%";
    }
}
