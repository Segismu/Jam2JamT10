using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource preparejump;
    [SerializeField] AudioSource jump;
    [SerializeField] AudioSource climb;
    [SerializeField] AudioSource land;

    public static SoundManager Instance { get; private set; }

    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        backgroundMusic.Play();
    }

    public AudioSource GetBGM() => backgroundMusic;
    public AudioSource GetWalk() => walk;
    public AudioSource GetPrepareJump() => preparejump;
    public AudioSource GetJump() => jump;
    public AudioSource GetClimb() => climb;
    public AudioSource GetLand() => land;

    public void StopAllSFX()
    {
        walk.Stop();
        preparejump.Stop();
        jump.Stop();
        climb.Stop();
        land.Stop();
    }
    
}
