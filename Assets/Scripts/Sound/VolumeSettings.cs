using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class VolumeSettings : MonoBehaviour
{
  [SerializeField] private AudioMixer mixer;
  [SerializeField] private Slider SliderMusic;
  [SerializeField] private Slider SliderSFX;

  public void SetMusicVolume()
  {
    float volume = SliderMusic.value;
    mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
  }
  
  public void SetSFXVolume()
  {
    float volume = SliderSFX.value;
    mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
  }
}
