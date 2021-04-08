using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnGlow : MonoBehaviour
{
    [SerializeField] GameObject gloveGlow;

   
    public void TurnOnGlove()
    {
        gloveGlow.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
