using UnityEngine;
using UnityEngine.Animations;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource SFX;
    
    public AudioClip BackgroundMusic;
    public AudioClip Clic;
    public AudioClip Construction;
    public AudioClip AttackZeus;
    public AudioClip AttackPoseidon;
    public AudioClip AttackDionysos;
    public AudioClip AttackAthena;
    public AudioClip AttackHades;
    
    private void Start ()
    {
        Music.clip = BackgroundMusic;
        Music.Play();
    }

    public void PlayAttackZeus()
    {
        SFX.PlayOneShot(AttackZeus);
    }
    public void PlayAttackPoseidon()
    {
        SFX.PlayOneShot(AttackPoseidon);
    }
    public void PlayAttackAthena()
    {
        SFX.PlayOneShot(AttackAthena);
    }
    public void PlayAttackHades()
    {
        SFX.PlayOneShot(AttackHades);
    }
    public void PlayAttackDionysos()
    {
        SFX.PlayOneShot(AttackDionysos);
    }
    public void PlayConstruction()
    {
        SFX.PlayOneShot(Construction);
    }
    public void PlayClic()
    {
        SFX.PlayOneShot(Clic);
    }
}
