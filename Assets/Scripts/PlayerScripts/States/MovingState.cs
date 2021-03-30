using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovingState : PlayerState
{
    public override string GiveName()
    {
        return "Moving";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {

        if (self.VerticalState == Player.VState.grounded)
        {;
            self.SlideValue += self.SliderCountUpSetValue * Time.deltaTime;
            if (MovementCheck(input.horizontalInput))
            {
                actions.Running();
                self.Moving = true;
                self.InteruptSliderSetToZero = true;

                if(self.CanTurn == true)
                {
                    if (input.horizontalInput > 0.001f)
                    {
                        self.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (input.horizontalInput < -0.001f)
                    {
                        self.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }

                //self.PlayRunningParticle();

                self.CanMove = true;
                self.CanTurn = true;
                //body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0);
            }

            if (!MovementCheck(input.horizontalInput))
            {
                actions.Idle();
                self.Moving = false;
                if(self.SlideValue >= self.maxSliderValue)
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
                if (BlockCheck(input.blockInput))
                {
                    actions.Block();
                    self.Blocking = true;
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                    self.SetState(new BlockState());
                }
            }

            if (HeavyCheck(input.heavyInput) && MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                actions.Heavy();
                self.CanTurn = false;
                self.SetState(new BusyState());
                self.StopMovingCharacterOnXAxis();
            }
            if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                //body.velocity = new Vector3(0, body.velocity.y, 0);
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.CanTurn = false;
                actions.JabCombo();
                self.SetState(new BusyState());
            }

            if (AttackCheck(input.attackInput) && !MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                body.velocity = new Vector3(0, body.velocity.y, 0);
                self.CanTurn = false;
                actions.JabCombo();
                self.SetState(new BusyState());
            }
            if (HeavyCheck(input.heavyInput) && !MovementCheck(input.horizontalInput))
            {
                self.CanMove = false;
                actions.Heavy();
                self.CanTurn = false;
                self.SetState(new BusyState());
                self.StopMovingCharacterOnXAxis();
            }

            if (CrouchingCheck(input.crouchInput))
            {
                //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                body.velocity = new Vector3(0, 0, 0) + calculate.overrideForce;
                actions.Crouching();
                self.SetState(new CrouchingState());
            }
            if (BlockCheck(input.blockInput))
            {
                actions.Block();
                self.Blocking = true;
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.SetState(new BlockState());
            }
        }
        else
        {
            if (HeavyCheck(input.heavyInput) && MovementCheck(input.horizontalInput))
            {
                self.CanActOutOf = false;
                actions.AerialAttack();
                self.CanTurn = false;
                self.WasAttacking = true;
                self.SetState(new BusyState());
            }
            if (HeavyCheck(input.heavyInput))
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

            if(self.GetCanAirMove() == true)
            {

                if (MovementCheck(input.horizontalInput))
                {
                    //body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                    body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0);

                    if (self.CanTurn == true)
                    {
                        if (input.horizontalInput > 0)
                        {
                            self.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else if (input.horizontalInput < 0)
                        {
                            self.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }

                    if (HeavyCheck(input.heavyInput) && MovementCheck(input.horizontalInput))
                    {
                        self.CanActOutOf = false;
                        actions.AerialAttack();
                        self.CanTurn = false;
                        self.WasAttacking = true;
                        self.SetState(new BusyState());
                    }

                    //if (input.horizontalInput == self.GetFacingDirection())
                    //{
                    //    if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                    //    {
                    //        actions.AerialAttack();
                    //        self.CanTurn = false;
                    //        self.WasAttacking = true;
                    //        self.SetState(new BusyState());
                    //    }
                    //}
                    //if (input.horizontalInput == self.GetFacingDirection() * -1)
                    //{
                    //    if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                    //    {
                    //        actions.AerialAttack();
                    //        self.CanTurn = false;
                    //        self.WasAttacking = true;
                    //        self.SetState(new BusyState());
                    //    }
                    //}
                }
                //if (JumpingCheck(input.jumpInput))
                //{
                //    if(self.canDoubleJump == true)
                //    {
                //        self.canDoubleJump = false;
                //        self.CanTurn = false;
                //        self.InAir = true;
                //        body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce + 2, body.velocity.z)) + calculate.addForce;
                //        self.JumpingOrFallingAnimations();
                //        self.AddOneToJumpIndex();
                //        Debug.Log("DoubleJump");
                //        self.PlayParticle(ParticleType.DoubleJump);
                //        self.SetState(new JumpingState());
                //    }
                //}
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

            if (!MovementCheck(input.horizontalInput))
            {
                self.CanMove = true;
                self.CanTurn = false;
                self.InAir = true;
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                if (body.velocity.x < 0.25f && body.velocity.x > -0.25f)
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.overrideForce;
                }
                if (body.velocity.x == 0)
                {
                    self.SetState(new JumpingState());
                }
            }
        }

        if (JumpingCheck(input.jumpInput))
        {
            if (self.CanJumpIndex < self.GetMaxJumps())
            {
                self.CanTurn = false;
                self.InAir = true;
                body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce + 2, body.velocity.z));
                self.JumpingOrFallingAnimations();
                self.AddOneToJumpIndex();
                self.SetState(new JumpingState());
            }
        }

    }
}

