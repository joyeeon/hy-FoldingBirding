using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum BirdState
    {
        Sit,
        Translate,
        Follow,
        Land,
        TakeOff,
        Dance,
        Hop
    }
    public enum InteractionState
    {
        Call,
        Finger,
        Palm,
        Pet,
        Follow,
        Bye
    }
    public static StateManager instance;

    public BirdState birdState;
    public InteractionState interactionState;
    [SerializeField] TMP_Text debugTxt;

    private Animator animator;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        SetInteractionState(InteractionState.Follow);
    }

    // Update is called once per frame
    void Update()
    {
        debugTxt.text = birdState.ToString();
    }

    public void SetInteractionState(InteractionState iState)
    {
        switch (iState)
        {
            case InteractionState.Call:
                interactionState = InteractionState.Call;
                SetBirdState(BirdState.Translate);
                break;
            case InteractionState.Bye:
                interactionState = InteractionState.Bye;
                SetBirdState(BirdState.Translate);
                break;
            case InteractionState.Palm:
                interactionState = InteractionState.Palm;
                if (birdState != BirdState.Sit)
                {
                    SetBirdState(BirdState.Land);
                }
                break;
            case InteractionState.Finger:
                interactionState = InteractionState.Finger;
                if (birdState != BirdState.Sit)
                {
                    SetBirdState(BirdState.Land);
                }
                break;
            case InteractionState.Follow:
                interactionState = InteractionState.Follow;
                SetBirdState(BirdState.Follow);
                break;
            case InteractionState.Pet:
                interactionState = InteractionState.Pet;
                SetBirdState(BirdState.Dance);
                break;
        }
    }

    private void SetBirdState(BirdState bState)
    {
        switch (bState)
        {
            case BirdState.Sit:
                birdState = BirdState.Sit;
                break;
            case BirdState.Translate:
                if(birdState == BirdState.Sit)
                {
                    animator.SetTrigger("isTakeOff");
                }
                birdState = BirdState.Translate;
                animator.SetBool("isTranslate", true);
                animator.SetBool("isFollow", false);
                break;
            case BirdState.Follow:
                if (birdState == BirdState.Sit)
                {
                    animator.SetTrigger("isTakeOff");
                }
                birdState = BirdState.Follow;
                animator.SetBool("isTranslate", false);
                animator.SetBool("isFollow", true);
                break;
            case BirdState.Land:
                //birdState = BirdState.Land;
                animator.SetTrigger("isLand");
                birdState = BirdState.Sit;
                break;
            case BirdState.TakeOff:
                //birdState = BirdState.TakeOff;
                animator.SetTrigger("isTakeOff");
                break;
            case BirdState.Hop:
                //birdState = BirdState.Hop;
                animator.SetTrigger("isHop");
                birdState = BirdState.Sit;
                break;
            case BirdState.Dance:
                //birdState = BirdState.Dance;
                animator.SetTrigger("isDance");
                birdState = BirdState.Sit;
                break;
        }
    }

    //For Debugging
    public void OnCallButtonClicked() => SetInteractionState(InteractionState.Call);
    public void OnFingerButtonClicked() => SetInteractionState(InteractionState.Finger);
    public void OnPalmButtonClicked() => SetInteractionState(InteractionState.Palm);
    public void OnPetButtonClicked() => SetInteractionState(InteractionState.Pet);
    public void OnFollowButtonClicked() => SetInteractionState(InteractionState.Follow);
    public void OnByeButtonClicked() => SetInteractionState(InteractionState.Bye);

}
