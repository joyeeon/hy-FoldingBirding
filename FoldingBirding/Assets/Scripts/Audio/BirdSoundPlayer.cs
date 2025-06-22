using UnityEngine;
using TMPro;

public class BirdSoundPlayer : MonoBehaviour
{
    [Header("Stateº° AudioSource ¼³Á¤")]
    public AudioSource callSource;
    public AudioSource byeSource;
    public AudioSource palmSource;
    public AudioSource fingerSource;
    public AudioSource followSource;
    public AudioSource petSource;

    [Header("µð¹ö±ë¿ë ÅØ½ºÆ® Ãâ·Â")]
    public TMP_Text debugText;

    public void PlaySound(StateManager.InteractionState state)
    {
        StopAllSources();

        string debugMessage = $"[BirdSoundPlayer] ½ÇÇà ";

        switch (state)
        {
            case StateManager.InteractionState.Call: 
                if (callSource != null)
                {
                    callSource.Play();
                    debugMessage = "callSource Àç»ýµÊ";
                }
                break;
            case StateManager.InteractionState.Bye:
                if (byeSource != null)
                {
                    byeSource.Play();
                    debugMessage = "byeSource Àç»ýµÊ";
                }
                break;
            case StateManager.InteractionState.Palm:
                if (palmSource != null)
                {
                    palmSource.Play();
                    debugMessage = "palmSource Àç»ýµÊ";
                }
                break;
            case StateManager.InteractionState.Finger:
                if (fingerSource != null)
                {
                    fingerSource.Play();
                    debugMessage = "fingerSource Àç»ýµÊ";
                }
                break;
            case StateManager.InteractionState.Follow:
                if (followSource != null)
                {
                    followSource.Play();
                    debugMessage = "followSource Àç»ýµÊ";
                }
                break;
            case StateManager.InteractionState.Pet:
                if (petSource != null)
                {
                    petSource.Play();
                    debugMessage = "petSource Àç»ýµÊ";
                }
                break;
        }

        Debug.Log(debugMessage);
        if (debugText != null)
        {
            debugText.text = debugMessage;
        }
    }

    private void StopAllSources()
    {
        callSource?.Stop();
        byeSource?.Stop();
        palmSource?.Stop();
        fingerSource?.Stop();
        followSource?.Stop();
        petSource?.Stop();
    }
}
