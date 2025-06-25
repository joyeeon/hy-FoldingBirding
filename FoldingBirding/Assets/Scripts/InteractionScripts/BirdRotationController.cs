using UnityEngine;

public class BirdRotationController : MonoBehaviour
{
    public Transform birdTransform;
    public Transform hmdTransform;

    private Vector3 lookTarget; // HMD를 바라보는 타겟 위치

    private void Start()
    {
        lookTarget = hmdTransform.position;
    }

    void Update()
    {

        if (birdTransform == null || hmdTransform == null)
            return;

        //Follow 상태에서 사용자를 바라보도록 회전
        if (StateManager.instance != null && StateManager.instance.birdState == StateManager.BirdState.Follow)
        {
            Vector3 lookTarget = hmdTransform.position;

            lookTarget.y = birdTransform.position.y; // 회전축 흔들림 방지용 (수평만 보게)
                                                 // 새의 현재 위치에서 HMD 방향으로 가는 벡터
            Vector3 direction = (lookTarget - birdTransform.position).normalized;

            // y축 기준으로 10도 회전된 방향을 만들기
            Quaternion yawRotation = Quaternion.AngleAxis(16f, Vector3.up); // 10도만큼 y축 회전
            Vector3 tiltedDirection = yawRotation * direction;

            // 최종적으로 해당 방향을 바라보게
            Quaternion targetRotation = Quaternion.LookRotation(tiltedDirection);
            birdTransform.rotation = targetRotation;
        }

        //Bye 상태에서 사용자의 반대방향을 바라보고 날아가도록 회전
        if (StateManager.instance != null && StateManager.instance.interactionState == StateManager.InteractionState.Bye)
        {
            Vector3 lookTarget = hmdTransform.position;

            lookTarget.y = birdTransform.position.y; // 회전축 흔들림 방지용 (수평만 보게)
                                                     // 새의 현재 위치에서 HMD 방향으로 가는 벡터
            Vector3 direction = (lookTarget - birdTransform.position).normalized;

            // y축 기준으로 10도 회전된 방향을 만들기
            Quaternion yawRotation = Quaternion.AngleAxis(180f, Vector3.up); // 10도만큼 y축 회전
            Vector3 tiltedDirection = yawRotation * direction;

            // 최종적으로 해당 방향을 바라보게
            Quaternion targetRotation = Quaternion.LookRotation(tiltedDirection);
            birdTransform.rotation = targetRotation;
        }

        //Call 상태에서 사용자의 방향을 바라보고 날아오도록 회전 
        if (StateManager.instance != null && StateManager.instance.interactionState == StateManager.InteractionState.Call)
        {
            Vector3 lookTarget = hmdTransform.position;

            lookTarget.y = birdTransform.position.y; // 회전축 흔들림 방지용 (수평만 보게)
                                                     // 새의 현재 위치에서 HMD 방향으로 가는 벡터
            Vector3 direction = (lookTarget - birdTransform.position).normalized;

            // y축 기준으로 10도 회전된 방향을 만들기
            
            Vector3 tiltedDirection = direction;

            // 최종적으로 해당 방향을 바라보게
            Quaternion targetRotation = Quaternion.LookRotation(tiltedDirection);
            birdTransform.rotation = targetRotation;
        }

        ////Finger 상태에서 사용자를 바라보도록 수정
        //if (StateManager.instance != null && StateManager.instance.interactionState == StateManager.InteractionState.Call)
        //{
        //    Vector3 lookTarget = hmdTransform.position;

        //    lookTarget.y = birdTransform.position.y; // 회전축 흔들림 방지용 (수평만 보게)
        //                                             // 새의 현재 위치에서 HMD 방향으로 가는 벡터
        //    Vector3 direction = (lookTarget - birdTransform.position).normalized;

        //    // y축 기준으로 10도 회전된 방향을 만들기
        //    Quaternion yawRotation = Quaternion.AngleAxis(18f, Vector3.up); // 10도만큼 y축 회전
        //    Vector3 tiltedDirection = yawRotation * direction;

        //    // 최종적으로 해당 방향을 바라보게
        //    Quaternion targetRotation = Quaternion.Euler(tiltedDirection);
        //    birdTransform.rotation = targetRotation;
        //}

    }
}
