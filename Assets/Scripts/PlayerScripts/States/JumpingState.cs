using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumpingState : PlayerState
{
    public override string GiveName()
    {
        return "Jumping";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        if (self.VerticalState == Player.VState.grounded)
        {
            self.CanJumpIndex = 0;
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(body.velocity.x, body.velocity.y, 0);
            //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
            if (body.velocity.x < 0.25f && body.velocity.x > -0.25f)
            {
                body.velocity = new Vector3(0, body.velocity.y, 0);
            }
            if (body.velocity.x == 0)
            {
                self.SetState(new IdleState());
            }
        }
        else
        {
            if (HeavyCheck(input.heavyInput))
            {
                self.CanActOutOf = false;
                actions.AerialAttack();
                self.CanTurn = false;
                self.WasAttacking = true;
                self.SetState(new BusyState());
            }
            if (HeavyCheck(input.heavyInput) && (self.GetFacingDirection() > 0 || self.GetFacingDirection() < 0))
            {
                self.CanActOutOf = false;
                actions.AerialAttack();
                self.CanTurn = false;
                self.WasAttacking = true;
                self.SetState(new BusyState());
            }
            if (!actions.IsDoubleJump)
            {
                if (self.VerticalState == Player.VState.jumping)
                {
                    actions.Jumping();
                }
                else
                {
                    actions.Falling();
                }
            }


            if (JumpingCheck(input.jumpInput))
            {
                if (self.CanJumpIndex < self.GetMaxJumps())
                {
                    if (self.canDoubleJump == true)
                    {
                        actions.IsDoubleJump = true;
                        self.canDoubleJump = false;
                        self.CanTurn = false;
                        self.InAir = true;
                        body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce + 2, body.velocity.z));
                        self.JumpingOrFallingAnimations();
                        self.AddOneToJumpIndex();
                        self.PlayParticle(ParticleType.DoubleJump, Vector3.zero);
                        actions.DoubleJump();
                        self.SetState(new JumpingState());
                    }

                }
            }

        }

        if (MovementCheck(input.horizontalInput))
        {
            self.CanMove = true;
            self.CanTurn = true;
            body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0);

            self.SetState(new MovingState());
        }



        if (HeavyCheck(input.heavyInput))
        {
            self.CanActOutOf = false;
            actions.AerialAttack();
            self.CanTurn = false;
            self.WasAttacking = true;
            self.SetState(new BusyState());
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
    }

    public override bool StickToGround() => false;
}

