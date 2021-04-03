using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimationTester : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem SolHeavy;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.Play("JAB");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.Play("HEAVY");
            SolHeavy.Play();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.Play("SWEEP");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.Play("AERIAL");
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            anim.Play("RUN");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            anim.Play("DASH");
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            anim.Play("UP_AIR");
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            anim.Play("BACK_AIR");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            anim.Play("ARMOUR_BREAK");
        }
    }
}
