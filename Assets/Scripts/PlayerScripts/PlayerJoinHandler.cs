using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinHandler : MonoBehaviour
{
    [SerializeField] BindToPlayer currentPlayerBind;

    public void JoinPlayer(PlayerInput input)
    {
        currentPlayerBind.JoinGame(input);
        if (GameManager.instance.canJoin == true)
        {

        }
    }
    public void SetPlayerBind(BindToPlayer players)
    {
        currentPlayerBind = players;
    }
}
