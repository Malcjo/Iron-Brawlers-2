using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourStats : MonoBehaviour
{
    [Header("Chest Armour Stats")]
    public Vector3 knockBackResistanceChest;
    public float weightChest;
    public float reduceSpeedChest;
    public float reduceJumpChest;

    [Header("Leg Armour Stats")]
    public Vector3 knockBackRsistanceLegs;
    public float weightLegs;
    public float reduceSpeedLegs;
    public float reduceJumpLegs;

    [Header("Head Armour Stats")]
    public Vector3 knockBackResistanceHead;
    public float weightHead;
    public float reduceSpeedHead;
    public float reduceJumpHead;
}
