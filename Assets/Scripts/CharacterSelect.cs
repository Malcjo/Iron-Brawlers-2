using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject player1SolAnimated, player1SolAltAnimated, player1GoblinAnimated, player1GoblinAltAnimated;
    [SerializeField] private GameObject player2SolAnimated, player2SolAltAnimated, player2GoblinAnimated, player2GoblinAltAnimated;
    [Space(10)]
    [SerializeField] private GameObject player1Character1PortraitPuck;
    [SerializeField] private GameObject player1Character2PortraitPuck;
    [SerializeField] private GameObject player2Character1PortraitPuck, player2Character2PortraitPuck;
    [SerializeField] private GameObject player1CharacterPuck, player2CharacterPuck;
    [Space(10)]
    [SerializeField] private GameObject Display1;
    [SerializeField] private GameObject Display2;
    [SerializeField] private GameObject Display3;
    [SerializeField] private GameObject Display4;
    [Space(10)]
    [SerializeField] private GameObject player1Character1Selected;
    [SerializeField] private GameObject player1Character2Selected;
    [SerializeField] private GameObject player2Character1Selected, player2Character2Selected;
    [SerializeField] private GameObject character1ButtonSelected, character2ButtonSelected;
    [Space(10)]
    [SerializeField] private GameObject LevelDisplay1Obj;
    [SerializeField] private GameObject LevelDisplay2Obj;
    [SerializeField] private GameObject level1DisplayImage, level1HighlightImage;
    [SerializeField] private GameObject level2DisplayImage, level2HighlightImage;
    [Space(10)]
    [SerializeField] private GameObject player1pressAToJoinUI;
    [SerializeField] private GameObject player2pressAToJoinUI;
    [SerializeField] private GameObject player1Character1Background, player1Character2Background;
    [SerializeField] private GameObject player2Character1Background, player2Character2Background;
    [Space(10)]
    [SerializeField] private GameObject player1SolPortrait;
    [SerializeField] private GameObject player1SolAltPortrait;
    [SerializeField] private GameObject player1GoblinPortrait;
    [SerializeField] private GameObject player1GoblinAltPortrait;
    [Space(10)]
    [SerializeField] private GameObject player2SolPortrait;
    [SerializeField] private GameObject player2SolAltPortrait;
    [SerializeField] private GameObject player2GoblinPortrait;
    [SerializeField] private GameObject player2GoblinAltPortrait;

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
