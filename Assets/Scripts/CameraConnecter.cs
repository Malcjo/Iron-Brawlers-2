using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConnecter : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    private void Start()
    {
        GameManager.instance.GetCameraObject(Camera);
    }

}
