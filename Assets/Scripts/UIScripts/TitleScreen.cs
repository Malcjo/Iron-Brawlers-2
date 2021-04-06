using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private Animator titleAnim;
    [SerializeField] private GameObject eventSystem;
    

    void Start()
    {
        eventSystem.SetActive(false);

    }

  
    void Update()
    {
        if (Input.anyKey)
        {
            titleAnim.SetTrigger("FadeToMainMenu");
            eventSystem.SetActive(true);
        }
    }
}
