using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAnimationTester : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] ParticleSystem Heavy;
    [SerializeField] ParticleSystem Sweap;
    [SerializeField] ParticleSystem Jab;
    [SerializeField] ParticleSystem Dash;
    [SerializeField] ParticleSystem FowardAir;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.Play("JAB");
            Jab.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.Play("HEAVY");
            Heavy.Play();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.Play("SWEEP");
            Sweap.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            anim.Play("AERIAL");
            FowardAir.Play();
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            anim.Play("RUN");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            anim.Play("DASH");
            Dash.Play();
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
