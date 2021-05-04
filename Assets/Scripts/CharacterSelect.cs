using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterSelect : MonoBehaviour
{
    public PlayerInputManager playerInputManager;

    [SerializeField] public GameObject player1SolAnimated, player1SolAltAnimated, player1GoblinAnimated, player1GoblinAltAnimated;
    [SerializeField] public GameObject player2SolAnimated, player2SolAltAnimated, player2GoblinAnimated, player2GoblinAltAnimated;
    [Space(10)]
    [SerializeField] public GameObject player1Character1PortraitPuck;
    [SerializeField] public GameObject player1Character2PortraitPuck;
    [SerializeField] public GameObject player2Character1PortraitPuck, player2Character2PortraitPuck;
    [SerializeField] public GameObject player1CharacterPuck, player2CharacterPuck;
    [Space(10)]
    [SerializeField] public GameObject Display1;
    [SerializeField] public GameObject Display2;
    [SerializeField] public GameObject Display3;
    [SerializeField] public GameObject Display4;
    [Space(10)] 
    [SerializeField] public GameObject player1Character1Selected;
    [SerializeField] public GameObject player1Character2Selected;
    [SerializeField] public GameObject player2Character1Selected, player2Character2Selected;
    [SerializeField] public GameObject character1ButtonSelected, character2ButtonSelected;
    [Space(10)]
    [SerializeField] public GameObject LevelDisplay1Obj;
    [SerializeField] public GameObject LevelDisplay2Obj;
    [SerializeField] public GameObject level1DisplayImage, level1HighlightImage;
    [SerializeField] public GameObject level2DisplayImage, level2HighlightImage;
    [Space(10)]
    [SerializeField] public GameObject player1pressAToJoinUI;
    [SerializeField] public GameObject player2pressAToJoinUI;
    [SerializeField] public GameObject player1Character1Background, player1Character2Background;
    [SerializeField] public GameObject player2Character1Background, player2Character2Background;
    [Space(10)]
    [SerializeField] public GameObject player1SolPortrait;
    [SerializeField] public GameObject player1SolAltPortrait;
    [SerializeField] public GameObject player1GoblinPortrait;
    [SerializeField] public GameObject player1GoblinAltPortrait;
    [Space(10)]
    [SerializeField] public GameObject player2SolPortrait;
    [SerializeField] public GameObject player2SolAltPortrait;
    [SerializeField] public GameObject player2GoblinPortrait;
    [SerializeField] public GameObject player2GoblinAltPortrait;

    public void SetUpPlayer1()
    {
        player1pressAToJoinUI.SetActive(false);
        player1Character1PortraitPuck.SetActive(true);
        player1Character1Background.SetActive(true);
    }
    public void SetUpPlayer2()
    {
        player2pressAToJoinUI.SetActive(false);
        player2Character1PortraitPuck.SetActive(true);
        player2Character1Background.SetActive(true);
    }
}
