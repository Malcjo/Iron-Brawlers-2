using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrouchingState : PlayerState
{
    public override string GiveName()
    {
        return "Crouching";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        //var actionTaken = false;
        body.velocity = new Vector3(0, body.velocity.y, 0);

        if (CrouchingCheck(input.crouchInput))
        {
            self.Crouching = true;
            actions.Crouching();
            if (HeavyCheck(input.heavyInput))
            {
                //actionTaken = true;
                self.CanMove = false;
                self.CanTurn = false;
                actions.LegSweep();
                self.SetState(new BusyState());
            }
            if (ArmourBreakCheck(input.armourBreakInput))
            {
                //actionTaken = false;
                if (armour.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none && armour.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none)
                {
                    return;
                }
                self.PlayParticle(ParticleType.ArmourBreak, Vector3.zero);
                actions.ArmourBreak();
                self.SetState(new BusyState());
            }
        }
        if (!CrouchingCheck(input.crouchInput))
        {
            self.Crouching = false;
            actions.ExitCrouch();
            //actions.Idle();
        }

        //if (!actionTaken)
        //{
        //    self.SetState(new IdleState());
        //}

    }
}

