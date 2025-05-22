using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Sound
{
  public class VolumeSettings : MonoBehaviour
  {
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider SliderMusic;
    [SerializeField] private Slider SliderSFX;
  
    public VolumeSettings Instance;
    
    public void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optionnel, pour garder l'instance entre les scènes    
      }
      else
      {
        Destroy(gameObject); // Évite les doublons
      }
    }

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
}
