using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isFly = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (isFly)
        {
            transform.Translate(transform.forward * Time.deltaTime * 0.5f);
        }
    }

    public void PressFlyBtn()
    {
        isFly = true;
        animator.SetBool("isFly", true);
        animator.SetBool("isIdle", false);
    }
    public void PressLandBtn()
    {
        isFly = false;
        animator.SetTrigger("isLand");
        animator.SetBool("isFly", false);
    }
    public void PressFlyIdleBtn()
    {
        animator.SetBool("isFly", true);
        animator.SetBool("isIdle", true);

    }
    public void PressTakeOffBtm()
    {
        animator.SetTrigger("isTakeOff");
        animator.SetBool("isFly", false);
        animator.SetBool("isIdle", true);
    }


}
