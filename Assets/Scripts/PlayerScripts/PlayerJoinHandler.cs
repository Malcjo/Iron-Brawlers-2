using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinHandler : MonoBehaviour
{
    [SerializeField] BindToPlayer currentPlayerBind;

    public void JoinPlayer(PlayerInput input)
    {
        if (GameManager.instance.canJoin == true)
        {
            print("Spawn Player");
            currentPlayerBind.JoinGame(input);
        }
    }
    public void SetPlayerBind(BindToPlayer players)
    {
        currentPlayerBind = players;
    }
}
