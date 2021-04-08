using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleState : PlayerState
{
    public override string GiveName()
    {
        return "Idle";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        actions.legShield.SetActive(false);
        self.CanMove = true;
        actions.Idle();
        if(self.VerticalState == Player.VState.grounded)
        {
            actions.Idle();
            //var actionTaken = false;
            if (MovementCheck(input.horizontalInput))
            {
                self.CanMove = true;
                self.CanTurn = true;
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0);

                self.SetState(new MovingState());
                //actionTaken = true;
            }
            if (!MovementCheck(input.horizontalInput))
            {
                actions.Idle();
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            }
            if (CrouchingCheck(input.crouchInput))
            {
                body.velocity = new Vector3(0, 0, 0);
                self.SetState(new CrouchingState());
                //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);

                //actionTaken = true;
            }

            if (JumpingCheck(input.jumpInput))
            {
                if (self.CanJumpIndex < self.GetMaxJumps())
                {
                    self.CanTurn = false;
                    self.InAir = true;
                    body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z));
                    self.JumpingOrFallingAnimations();
                    self.AddOneToJumpIndex();
                    self.SetState(new JumpingState());
                    //actionTaken = true;
                }
            }
            if (AttackCheck(input.attackInput))
            {
                self.CanMove = false;
                //body.velocity = new Vector3(0, body.velocity.y, 0);
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.CanTurn = false;
                actions.Jab();
                self.SetState(new BusyState());


                //actionTaken = true;
            }
            if (HeavyCheck(input.heavyInput))
            {
                self.CanMove = false;
                actions.Heavy();
                self.CanTurn = false;
                self.SetState(new BusyState());
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                //self.StopMovingCharacterOnXAxis();
                //actionTaken = true;
            }
            if (BlockCheck(input.rightTriggerInput))
            {
                actions.Block();
                self.Blocking = true;
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.SetState(new BlockState());
                //actionTaken = true;
            }
            if (!BlockCheck(input.rightTriggerInput))
            {
                actions.ExitBlock();
                self.Blocking = false;
            }
            if (ArmourBreakCheck(input.rightBumperInput, input.leftBumperInput))
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
            if (DashCheck(input.leftTriggerInput))
            {
                self.SetState(new BusyState());
                actions.Dash();
            }
            //if (!actionTaken)
            //{
            //    actions.Idle();
            //}
        }
        else
        {
            Debug.Log("Go To Jump");
            self.SetState(new JumpingState());
        }


    }
}

