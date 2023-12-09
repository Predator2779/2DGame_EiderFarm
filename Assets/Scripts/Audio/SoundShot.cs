using FMODUnity;
using UnityEngine;

public class SoundShot : MonoBehaviour
{
    [SerializeField] private string[] _sounds;

    public void Shot()
    {
        if (_sounds == null) return;
        
        foreach (var sound in _sounds)
            RuntimeManager.PlayOneShot(sound);
    }
}