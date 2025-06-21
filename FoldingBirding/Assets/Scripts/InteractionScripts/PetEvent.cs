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
            Debug.Log($"{other.name} Pet My Bird");
            StateManager.instance.SetInteractionState(StateManager.InteractionState.Pet);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MyBird"))
        {
            Debug.Log($"{other.name} Stop Petting My Bird");
        }
    }
}
