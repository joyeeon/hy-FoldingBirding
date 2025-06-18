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
        // ���� ������� �ʵ��� ����
        birdFollower?.SetExternalControl(true);
        //bird.GetComponent<BirdFollower>()?.SetFollowing(false);
    }

    public void CloseBirdFlyAway()
    {
        isMovingBird = false;
        // ���� ��������� ����
        birdFollower?.SetExternalControl(false);
        //bird.GetComponent<BirdFollower>()?.SetFollowing(true);
    }
}
