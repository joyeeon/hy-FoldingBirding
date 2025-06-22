using UnityEngine;

public class AudioToggleController : MonoBehaviour
{
    [Tooltip("AudioSource")]
    public AudioSource audioSource;

    public void PlayAudio()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopAudio()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
