using UnityEngine;
using UnityEngine.VFX;

public class NewBehaviourScript : MonoBehaviour
{
    //public VisualEffect birdTrailVFX;
    public VisualEffect vfx;                // Trail VFX 컴포넌트
    public Transform birdTransform;
    public Transform trailEffectObject;

    void Update()
    {
        //if (StateManager.instance != null && StateManager.instance.birdState == StateManager.BirdState.Follow)
        //{
        //    birdTrailVFX.SetVector3("BirdPosition", birdTransform.position);
        //    trailEffectObject.position = birdTransform.position;
        //}
        bool shouldPlayTrail =
            StateManager.instance != null &&
            (
                //StateManager.instance.birdState == StateManager.BirdState.Follow ||
                StateManager.instance.interactionState == StateManager.InteractionState.Call ||
                StateManager.instance.interactionState == StateManager.InteractionState.Bye
            );

        if (shouldPlayTrail)
        {
            // 위치값 전달
            vfx.SetVector3("BirdPosition", birdTransform.position);
            trailEffectObject.position = birdTransform.position;

            // VFX 활성화
            if (!vfx.enabled)
                vfx.enabled = true;
        }
        else
        {
            // VFX 비활성화
            if (vfx.enabled)
                vfx.enabled = false;
        }
    }

}

