using UnityEngine;
using UnityEngine.Audio;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer mixer;

    private void Start()
    {
        float bgm = PlayerPrefs.GetFloat(Prefs.BgmVolume, 0.5f);
        float sfx = PlayerPrefs.GetFloat(Prefs.SfxVolume, 0.5f);

        SetBGMVolume(bgm);
        SetSFXVolume(sfx);
    }

    // 0~1 범위를 로그 스케일로 변환해서 적용
    public void SetBGMVolume(float value)
    {
        if (value <= 0.0001f)
        {
            mixer.SetFloat(Prefs.BgmVolume, -80f); 
        }
        else
        {
            mixer.SetFloat(Prefs.BgmVolume, Mathf.Log10(value) * 20f);
        }

        PlayerPrefs.SetFloat(Prefs.BgmVolume, value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f)
        {
            mixer.SetFloat(Prefs.SfxVolume, -80f);
        }
        else
        {
            mixer.SetFloat(Prefs.SfxVolume, Mathf.Log10(value) * 20f);
        }

        PlayerPrefs.SetFloat(Prefs.SfxVolume, value);
    }
}