
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusyState : PlayerState
{
    public override string GiveName()
    {
        return "BusyState";
    }
    public override void RunState(Player self, Rigidbody body, PlayerActions actions, ArmourCheck armour, InputState input, Calculating calculate)
    {
        if (self.CanMove == true)
        {
            if (self.VerticalState == Player.VState.grounded)
            {
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;
            }
            else
            {
                body.velocity = new Vector3(input.horizontalInput * (calculate.characterSpeed / 1.5f), body.velocity.y, 0) + calculate.addForce;
                //body.velocity = new Vector3(body.velocity.x, body.velocity.y, 0) + calculate.addForce;
            }
        }
        else if (self.CanMove == false)
        {
            //body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
            //body.velocity = new Vector3(body.velocity.x, body.velocity.y, 0) + calculate.addForce;
            //self.CanMove = true;
            //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
            body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0) + calculate.addForce;
            if (self.GetFacingDirection() > 0)
            {
                if (body.velocity.x < 0.1f)
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
                }
            }
            else if (self.GetFacingDirection() < 0)
            {
                if (body.velocity.x > -0.1f)
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0) + calculate.addForce;
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
                body.velocity = new Vector3(input.horizontalInput * calculate.characterSpeed, body.velocity.y, 0) + calculate.addForce;

                self.SetState(new MovingState());
            }
            if (CrouchingCheck(input.crouchInput))
            {
                self.landing = false;
                Debug.Log("Crouching");
                //body.velocity = new Vector3(Mathf.Lerp(body.velocity.x, 0, calculate.friction), body.velocity.y, 0);
                body.velocity = new Vector3(0, 0, 0) + calculate.addForce;
                self.SetState(new CrouchingState());
            }
        }
    }
}


