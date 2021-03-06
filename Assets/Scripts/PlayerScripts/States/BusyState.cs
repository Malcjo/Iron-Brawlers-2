
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusyState : PlayerState
{
    private float gravityAdder;
    public override string GiveName()
    {
        return "BusyState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        // when bouncing do not use overrideforce, set velocity directly
        gravityAdder += calculate.gravityValue * Time.deltaTime;
        if (self.VerticalState == Player.VState.grounded)
        {
            //body.velocity = calculate.overrideForce;
        } 
        else
        {
            
            //if (!self.CanMove)
            //{
            //    body.velocity = calculate.overrideForce + Vector3.down * gravityAdder;
            //    if (ArmourBreakCheck(input.rightBumperInput, input.leftBumperInput))
            //    {
            //        //actionTaken = false;
            //        if (armour.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none && armour.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none)
            //        {
            //            return;
            //        }
            //        self.PlayParticle(ParticleType.ArmourBreak, Vector3.zero);
            //        actions.ArmourBreak();
            //        self.SetState(new BusyState());
            //    }
            //}
            //else
            //{
            //    body.velocity = new Vector3((calculate.overrideForce.x + (input.horizontalInput * calculate.characterSpeed)), calculate.overrideForce.y, 0) + Vector3.down * gravityAdder;
            //    if (ArmourBreakCheck(input.rightBumperInput, input.leftBumperInput))
            //    {
            //        //actionTaken = false;
            //        if (armour.GetChestArmourCondiditon() == ArmourCheck.ArmourCondition.none && armour.GetLegArmourCondition() == ArmourCheck.ArmourCondition.none)
            //        {
            //            return;
            //        }
            //        self.PlayParticle(ParticleType.ArmourBreak, Vector3.zero);
            //        actions.ArmourBreak();
            //        self.SetState(new BusyState());
            //    }
            //}
        }
        //need to add a can act out of bool to exit out of busy state, need to be set in player actions script near the end of certain animations
        if (self.CanActOutOf)
        {
            if(self.VerticalState != Player.VState.grounded)
            {
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
                if (CrouchingCheck(input.crouchInput))
                {
                    self.Crouching = false;
                    actions.ExitCrouch();
                }
            }
            else
            {
                if (HeavyCheck(input.heavyInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    self.CanTurn = false;
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                    
                    actions.Heavy();
                    self.SetState(new BusyState());
                }
                if (HeavyCheck(input.heavyInput) && MovementCheck(input.horizontalInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    self.CanTurn = false;
                    self.StopMovingCharacterOnXAxis();

                    actions.Heavy();
                    self.SetState(new BusyState());
                }
                if (AttackCheck(input.attackInput))
                {
                    self.CanMove = false;
                    self.CanTurn = false;
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);

                    actions.Jab();
                    self.SetState(new BusyState());
                }
                if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                {
                    self.CanMove = false;
                    self.CanTurn = false;
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);

                    actions.Jab();
                    self.SetState(new BusyState());
                    //actions.Idle();
                    //self.Moving = false;
                    //if (self.SlideValue >= self.maxSliderValue)
                    //{
                    //    self.InteruptSliderSetToZero = false;
                    //}
                    //else
                    //{
                    //    self.SlideValue = 0;
                    //}

                }
                if (MovementCheck(input.horizontalInput))
                {
                    self.CanMove = true;
                    self.CanTurn = true;
                    body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0);
                    self.SetMoveStrengthXTo0();
                    self.SetMoveStrengthYTo0();

                    self.SetState(new MovingState());
                }
                if (!MovementCheck(input.horizontalInput))
                {
                    actions.Idle();
                    self.Moving = false;
                    if (self.SlideValue >= self.maxSliderValue)
                    {
                        self.InteruptSliderSetToZero = false;
                    }
                    else
                    {
                        self.SlideValue = 0;
                    }

                    self.SetSlideValueToZero();
                    //self.StopRunningParticle();
                    self.CanMove = true;
                    self.CanTurn = true;
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                    if (body.velocity.x < 0.25f && body.velocity.x > -0.25f)
                    {
                        body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.overrideForce;
                    }
                    if (body.velocity.x == 0)
                    {
                        self.SetState(new IdleState());
                    }
                    if (BlockCheck(input.rightTriggerInput))
                    {
                        actions.Block();
                        self.Blocking = true;
                        body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                        self.SetState(new BlockState());
                    }
                }
                if (CrouchingCheck(input.crouchInput))
                {
                    body.velocity = new Vector3(0, 0, 0);
                    self.SetState(new CrouchingState());
                }
                if (!CrouchingCheck(input.crouchInput))
                {
                    self.Crouching = false;
                    actions.ExitCrouch();
                    //actions.Idle();
                }

            }

        }
        else
        {
            if (self.VerticalState != Player.VState.grounded)
            {
                body.velocity = calculate.overrideForce + Vector3.down * gravityAdder;

            }
            else
            {
                body.velocity = calculate.overrideForce;
            }

        }
        if (self.landing == true)
        {
            //if (MovementCheck(input.horizontalInput))
            //{
            //    self.landing = false;
            //    self.CanMove = true;
            //    self.CanTurn = true;
            //    body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.overrideForce;
            //    self.SetMoveStrengthXTo0();
            //    self.SetMoveStrengthYTo0();
            //    self.SetState(new MovingState());
            //}
            //if (CrouchingCheck(input.crouchInput))
            //{
            //    self.landing = false;
            //    //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            //    body.velocity = new Vector3(0, 0, 0) + calculate.overrideForce;
            //    self.SetState(new CrouchingState());
            //}
            //if (JumpingCheck(input.jumpInput))
            //{
            //    if (self.CanJumpIndex < self.GetMaxJumps())
            //    {
            //        self.CanTurn = false;
            //        self.InAir = true;
            //        body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z));
            //        self.JumpingOrFallingAnimations();
            //        self.AddOneToJumpIndex();
            //        self.SetState(new JumpingState());
            //        //actionTaken = true;
            //    }
            //}
        }
    }
}


