using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PlayerState
{
    public struct InputState
    {
        public float horizontalInput;
        public bool attackInput;
        public bool jumpInput;
        public bool crouchInput;
        public bool armourBreakInput;
        public bool blockInput;
        public bool heavyInput;
        public bool upDirectionInput;
        public bool rightTriggerInput;
        public bool leftTriggerInput;
        public bool rightBumperInput;
        public bool leftBumperInput;
    }
    public struct Calculating
    {
        public float jumpForce;
        public float friction;
        public float characterSpeed;
        public Vector3 overrideForce;
        public float gravityValue;
    }
    /*
    * idle state doing nothing
    * moving state running or moving in the air
    * airborne state in the air
    * crouching state crouching
    * busy state attacking, blocking or doing a special move, intro, knockdown, victory
    */
    public abstract string GiveName();
    public abstract void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate);

    protected bool MovementCheck(float horizontalInput)
    {
        return horizontalInput != 0;
    }
    protected bool CrouchingCheck(bool crouchInput)
    {
        return crouchInput;
    }
    protected bool JumpingCheck(bool jumpInput)
    {
        return jumpInput;
    }
    protected bool AttackCheck(bool attackInput)
    {
        return attackInput;
    }
    protected bool ArmourBreakCheck(bool rightBumperInput, bool leftBumperInput)
    {
        return rightBumperInput && leftBumperInput;
        //if(rightBumperInput && leftBumperInput)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }
    protected bool BlockCheck(bool blockInput)
    {
        return blockInput;
    }

    protected bool HeavyCheck(bool heavyInput)
    {
        return heavyInput;
    }

    protected bool UpDirectionCheck(bool upDirectionInput)
    {
        return upDirectionInput;
    }
    protected bool DashCheck(bool dashInput)
    {
        return dashInput;
    }
    public virtual bool StickToGround() => true;
}

