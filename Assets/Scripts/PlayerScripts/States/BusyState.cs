
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
            body.velocity = calculate.overrideForce;
        } 
        else
        {
            if (!self.CanMove)
            {
                body.velocity = calculate.overrideForce + Vector3.down * gravityAdder;

            }
            else
            {
                body.velocity = new Vector3((calculate.overrideForce.x + (input.horizontalInput * calculate.characterSpeed)), calculate.overrideForce.y, 0) + Vector3.down * gravityAdder;
            }
        }
        //need to add a can act out of bool to exit out of busy state, need to be set in player actions script near the end of certain animations
        if (self.CanActOutOf)
        {
            if(self.VerticalState != Player.VState.grounded)
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
            }
            else
            {
                if (HeavyCheck(input.heavyInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    actions.Heavy();
                    self.CanTurn = false;
                    self.SetState(new BusyState());
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                }
                if (HeavyCheck(input.heavyInput) && MovementCheck(input.horizontalInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    actions.Heavy();
                    self.CanTurn = false;
                    self.SetState(new BusyState());
                    self.StopMovingCharacterOnXAxis();
                }
                if (AttackCheck(input.attackInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    //body.velocity = new Vector3(0, body.velocity.y, 0);
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                    self.CanTurn = false;
                    actions.Jab();
                    self.SetState(new BusyState());
                }
                if (AttackCheck(input.attackInput) && MovementCheck(input.horizontalInput))
                {
                    self.CanActOutOf = false;
                    self.CanMove = false;
                    //body.velocity = new Vector3(0, body.velocity.y, 0);
                    body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                    self.CanTurn = false;
                    actions.Jab();
                    self.SetState(new BusyState());
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
            }

        }
        if (self.landing == true)
        {
            if (MovementCheck(input.horizontalInput))
            {
                self.landing = false;
                self.CanMove = true;
                self.CanTurn = true;
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.overrideForce;
                self.SetMoveStrengthXTo0();
                self.SetMoveStrengthYTo0();
                self.SetState(new MovingState());
            }
            if (CrouchingCheck(input.crouchInput))
            {
                self.landing = false;
                //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                body.velocity = new Vector3(0, 0, 0) + calculate.overrideForce;
                self.SetState(new CrouchingState());
            }
        }
    }
}


