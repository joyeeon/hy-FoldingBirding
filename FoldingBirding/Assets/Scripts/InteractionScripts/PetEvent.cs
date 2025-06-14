using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MyBird"))
        {
            StateManager.instance.birdState = StateManager.BirdState.Pet;
            Debug.Log($"{other.name} Pet My Bird");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MyBird"))
        {
            StateManager.instance.birdState = StateManager.BirdState.Sit;
            Debug.Log($"{other.name} Stop Petting My Bird");
        }
    }
}
