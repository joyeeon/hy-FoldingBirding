using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//왼손이 새에 닿았을 때, 감지하는 스크립트 
public class BirdTriggerDetect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag ("MyBird"))
        {
            Debug.Log("[BirdTriggerDetect] 새가 왼손에 닿았습니다!");
        }
    }
}
