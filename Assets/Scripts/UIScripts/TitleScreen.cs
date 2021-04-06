using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using static UnityEngine.InputSystem.InputAction;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator titleAnim;
    [SerializeField] private InputSystemUIInputModule inputSystemUIModule;
    public bool haveBeenThroughTitle;

    private void Start()
    {
        haveBeenThroughTitle = false;
    }
    public void TurnOnInputSystem()
    {
        inputSystemUIModule.enabled = true;
    }
    public void SetInputSystemModule()
    {
        inputSystemUIModule = GameObject.FindGameObjectWithTag("Event").GetComponent<InputSystemUIInputModule>();
    }
    void Update()
    {
        if (Input.anyKey)
        {
            haveBeenThroughTitle = true;
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
