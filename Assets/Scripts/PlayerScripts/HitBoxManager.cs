using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxManager : MonoBehaviour
{
    public GameObject hitBoxObj, blockObj;
    public Hitbox hitBox;
    [Range(0, 1)]
    private int armIndex;
    public PlayerActions animationScript;
    public Player player;
    private PlayerInputHandler playerInput;
    public Vector3 blockOffset;

    private Attackdirection _attackDirection;
    private FollowDes _hitboxFollow;
    private HitBoxScale _hitboxScale;
    private AttackType _attackType;
    public AttackType GetAttackType() { return _attackType; }
    public Attackdirection GetAttackDirection() { return _attackDirection; }
    public FollowDes GetHitboxFollowDestination() { return _hitboxFollow; }
    public HitBoxScale GetHitboxScale() { return _hitboxScale; }
    public void SetAttackType(AttackType attackType) { _attackType = attackType; }
    public void SetAttackDirection(Attackdirection attackDirection) { _attackDirection = attackDirection; }
    public void SetHitboxFollowDestination(FollowDes followDes) { _hitboxFollow = followDes; }
    public void SetHitBoxScale(HitBoxScale hitboxScale) { _hitboxScale = hitboxScale; }

    void Start()
    {
        playerInput = GetComponentInParent<PlayerInputHandler>();
        animationScript = GetComponentInChildren<PlayerActions>();
        hitBox.HideHitBoxes();
        blockObj.SetActive(false);
    }
    public void Block()
    {
        //blockBox.transform.position = transform.position + new Vector3(player.GetFacingDirection() * blockOffset.x, blockOffset.y, 0); 
        //blockObj.SetActive(true); 
    }
    public void StopBlock()
    {
        //blockObj.SetActive(false); 
    }
    public void SwapHands(int _armIndex)
    {
        hitBox._hitBoxScale = HitBoxScale.Jab;
        armIndex = _armIndex;
        if (armIndex == 0)
        {
            hitBox.armIndex = 0;
        }
        else if (armIndex == 1)
        {
            hitBox.armIndex = 1;
        }
        hitBox.FollowHand();//to snap into place before hitbox is played 
    }
    public void JabAttack(float spawnTime)
    {
        StartCoroutine(SpawnHitBox(spawnTime));
        StopCoroutine(SpawnHitBox(0));
    }
    public void LegSweep(float spawnTime)
    {
        //hitBox.FollowHand();//to snap into place before hitbox is played 
        hitBox._hitBoxScale = HitBoxScale.Jab;
        StartCoroutine(SpawnHitBox(spawnTime));
        StopCoroutine(SpawnHitBox(0));
    }
    public void AeiralAttack()
    {
        hitBox.FollowRightElbow();//to snap into place before hitbox is played 
        hitBox._hitBoxScale = HitBoxScale.Aerial;
        StartCoroutine(SpawnHitBox(0.3f));
        StopCoroutine(SpawnHitBox(0));
    }
    public void ArmourBreak()
    {
        hitBox.FollowCenter();//to snap into place before hitbox is played 
        hitBox._hitBoxScale = HitBoxScale.ArmourBreak;
        StartCoroutine(FreezeFrames(0.1f, 0.1f));
        StartCoroutine(SpawnHitBox(0.25f));
        StopCoroutine(FreezeFrames(0, 0));
        StopCoroutine(SpawnHitBox(0));
    }
    public IEnumerator SpawnHitBox(float spawnTime)
    {
        hitBox.ShowHitBoxes();
        yield return new WaitForSeconds(spawnTime);
        hitBox.HideHitBoxes();
    }

    public IEnumerator FreezeFrames(float delayTime, float AnimationTime)
    {
        yield return new WaitForSeconds(delayTime);

        yield return new WaitForSeconds(AnimationTime);
    }
}
