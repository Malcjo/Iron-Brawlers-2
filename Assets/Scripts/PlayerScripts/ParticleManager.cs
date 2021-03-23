using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType { Landing, DoubleJump, Running, ArmourBreak, BreakArmourPiece }
public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ArmourCheck armourCheck;
    [SerializeField] private GameObject DoubleJumpDustParticles;
    [SerializeField] private GameObject landOnGroundDustParticle;
    [SerializeField] private ParticleSystem RunningParticle;
    [SerializeField] private GameObject armourBreak;
    [SerializeField] private GameObject breakArmourPieceParticle;

    private void Start()
    {
        //RunningParticle.Stop();
    }

    public void PlayDoubleJumpParticle()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject DoubleJumpParticles = Instantiate(DoubleJumpDustParticles, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(DoubleJumpParticles);
    }
    public void PlayLandOnGroundParticle()
    {
        Vector3 landOnGroundDustPartilePosition = new Vector3(transform.localPosition.x + 0.1f, transform.position.y + 0.1f, transform.position.z);
        Quaternion landOnGroundDustParticleRotation = Quaternion.Euler(90, 0, 0);
        GameObject LandingParticles = Instantiate(landOnGroundDustParticle, landOnGroundDustPartilePosition, landOnGroundDustParticleRotation);
        SetRemoveParticles(LandingParticles);
    }
    public void PlayArmourBreakParticle()
    {
        Vector3 armourBreakPosition = new Vector3(transform.localPosition.x, transform.position.y + 1f, transform.position.z);
        Quaternion armourBreakRotation = Quaternion.Euler(90, 0, 0);
        GameObject _armourBreak = Instantiate(armourBreak, armourBreakPosition, armourBreakRotation);
        SetRemoveParticles(_armourBreak);
    }
    public void PlayerBreakArmourPiece(Vector3 position)
    {
        Vector3 breakPiecePosition = new Vector3(transform.localPosition.x, transform.position.y + 1f, transform.position.z);
        Quaternion breakPieceRotation = Quaternion.Euler(90, 0, 0);
        GameObject _breakArmourPiece = Instantiate(breakArmourPieceParticle, position, breakPieceRotation);
        SetRemoveParticles(_breakArmourPiece);
    }
    public void StopArmourBreakParticle()
    {
    }
    public void PlayerRunningParticle()
    {
        RunningParticle.Play();
    }
    public void PlayHitParticles()
    {

    }
    private void SetRemoveParticles(GameObject obj)
    {
        StartCoroutine(RemoveParticles(obj));
    }
    IEnumerator RemoveParticles(GameObject obj)
    {
        yield return new WaitForSeconds(1);
        Destroy(obj.gameObject);
    }
}
