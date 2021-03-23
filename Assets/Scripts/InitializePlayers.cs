using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializePlayers : MonoBehaviour
{
    [SerializeField] Transform player1Spawn, player2Spawn;
    private void Awake()
    {
        GameManager.instance.SetPlayerSpawns(player1Spawn, player2Spawn);
    }
}
