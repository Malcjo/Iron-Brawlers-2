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
        self.CanMove = true;
        if(self.VerticalState == Player.VState.grounded)
        {
            var actionTaken = false;
            if (MovementCheck(input.horizontalInput))
            {
                self.CanTurn = true;
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;

                self.SetState(new MovingState());
                actionTaken = true;
            }
            if (!MovementCheck(input.horizontalInput))
            {
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            }
            if (CrouchingCheck(input.crouchInput))
            {
                //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                body.velocity = new Vector3(0, 0, 0) + calculate.addForce;
                self.SetState(new CrouchingState());
                actionTaken = true;
            }

            if (JumpingCheck(input.jumpInput))
            {
                if (self.CanJumpIndex < self.GetMaxJumps())
                {
                    self.CanTurn = false;
                    self.InAir = true;
                    body.velocity = (new Vector3(body.velocity.x, calculate.jumpForce, body.velocity.z)) + calculate.addForce;
                    self.JumpingOrFallingAnimations();
                    self.AddOneToJumpIndex();
                    self.SetState(new JumpingState());
                    actionTaken = true;
                }
            }
            if (AttackCheck(input.attackInput))
            {
                self.CanMove = false;
                //body.velocity = new Vector3(0, body.velocity.y, 0);
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.CanTurn = false;
                actions.JabCombo();
                self.SetState(new BusyState());
                actionTaken = true;
            }
            if (HeavyCheck(input.heavyInput))
            {
                self.CanMove = false;
                actions.Heavy();
                self.CanTurn = false;
                self.SetState(new BusyState());
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                //self.StopMovingCharacterOnXAxis();
                actionTaken = true;
            }
            if (BlockCheck(input.blockInput))
            {
                actions.EnterBlock();
                self.Blocking = true;
                body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                self.SetState(new BlockState());
                actionTaken = true;
            }
            if (!BlockCheck(input.blockInput))
            {
                self.Blocking = false;
            }
            if (!actionTaken)
            {
                actions.Idle();
            }
        }
        else
        {
            self.SetState(new JumpingState());
        }


    }
}

