using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class ByeInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject bird;
    private bool isMovingBird = false;

    public float birdMoveSpeed = 2f;
    public Vector3 moveDirection = new Vector3(0, 1, 1);

    private BirdFollower birdFollower;

    void Start()
    {
        bird = GameObject.FindWithTag("MyBird");
        moveDirection = moveDirection.normalized;
        birdFollower = GetComponent<BirdFollower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingBird && bird != null)
        {
            bird.transform.position += moveDirection * birdMoveSpeed * Time.deltaTime;
        }
    }

    public void TriggerBirdFlyAway()
    {
        isMovingBird = true;
        // 새가 따라오지 않도록 설정
        birdFollower?.SetExternalControl(true);
        StateManager.instance.SetInteractionState(StateManager.InteractionState.Bye);

        // 사용자를 바라보게 회전
        //Vector3 lookTarget = hmdTransform.position;
        //lookTarget.y = transform.position.y; // 회전축 흔들림 방지용 (수평만 보게)
        //                                     // 새의 현재 위치에서 HMD 방향으로 가는 벡터
        //Vector3 direction = (lookTarget - transform.position).normalized;

        //// y축 기준으로 10도 회전된 방향을 만들기
        //Quaternion yawRotation = Quaternion.AngleAxis(16f, Vector3.up); // 10도만큼 y축 회전
        //Vector3 tiltedDirection = yawRotation * direction;

        //// 최종적으로 해당 방향을 바라보게
        //Quaternion targetRotation = Quaternion.LookRotation(tiltedDirection);
        //transform.rotation = targetRotation;
    }

    public void CloseBirdFlyAway()
    {
        isMovingBird = false;
        // 새가 따라오도록 설정
        birdFollower?.SetExternalControl(false);
        StateManager.instance.SetInteractionState(StateManager.InteractionState.Follow);
    }
}
