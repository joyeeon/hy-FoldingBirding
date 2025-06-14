using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public enum BirdState
    {
        Idle,
        Follow,
        Sit,
        Pet
    }
    public static StateManager instance;

    public BirdState birdState;
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
        birdState = BirdState.Idle;   
    }

    // Update is called once per frame
    void Update()
    {
        debugTxt.text = birdState.ToString();    
    }

/*    public void ChangeState(BirdState newState)
    {
        if (birdState == newState) return;

        if (birdState == BirdState.Fly && newState == BirdState.Sit)
        {
            animator.SetTrigger("FlyToSit");
        }
        else if (birdState == BirdState.Sit && newState == BirdState.Fly)
        {
            animator.SetTrigger("TakeOff");
        }
        else if (newState == BirdState.Pet)
        {
            animator.SetTrigger("PetReaction");
        }

        Debug.Log($"State: {birdState} ¡æ {newState}");
        birdState = newState;
    }*/
}
