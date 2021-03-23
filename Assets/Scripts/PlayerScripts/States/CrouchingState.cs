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
        body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
        actions.Crouching();
        if (!CrouchingCheck(input.crouchInput))
        {
            actions.ExitCrouch();
        }
        if (HeavyCheck(input.heavyInput))
        {
            self.CanMove = false;
            self.CanTurn = false;
            actions.LegSweep();
            self.SetState(new BusyState());
        }
        if (ArmourBreakCheck(input.armourBreakInput))
        {
            if(armour.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none && armour.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none)
            {
                return;
            }
            self.PlayParticle(ParticleType.ArmourBreak, Vector3.zero);
            actions.ArmourBreak();
            self.SetState(new BusyState());
        }

    }
}

