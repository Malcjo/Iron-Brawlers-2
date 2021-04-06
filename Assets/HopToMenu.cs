using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HopToMenu : MonoBehaviour
{
    private void Awake()
    {
        GameManager.instance.ClearBindToPlayer();
    }
}
