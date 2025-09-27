using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public GameObject vibrateOnbutton;
    public GameObject vibrateOffbutton;

    private void Start()
    {
        var isVibrateOn = PlayerPrefs.GetInt(Prefs.Vibrate, 1) == 1;
        vibrateOnbutton.SetActive(isVibrateOn);
        vibrateOffbutton.SetActive(!isVibrateOn);

        bgmSlider.value = PlayerPrefs.GetFloat(Prefs.BgmVolume, 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat(Prefs.SfxVolume, 0.5f);
    }

    public void SetVibrate(bool value)
    {
        PlayerPrefs.SetInt(Prefs.Vibrate, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}