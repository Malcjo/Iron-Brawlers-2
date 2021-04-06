using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using static UnityEngine.InputSystem.InputAction;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator titleAnim;
    [SerializeField] private GameObject eventSystem;
    [SerializeField] private InputSystemUIInputModule inputSystemUIModule;
    

    void Start()
    {
        //eventSystem.SetActive(false);

    }

  
    void Update()
    {
        if (Input.anyKey)
        {
            titleAnim.SetTrigger("FadeToMainMenu");
            StartCoroutine (EventSystemActivate());
        }
    }

    IEnumerator EventSystemActivate()
    {
        while (titleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }
        inputSystemUIModule.enabled = true;
    }
}
