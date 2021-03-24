using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Null, Jab, LegSweep, Aerial, ArmourBreak, HeavyJab };
public enum Attackdirection { Forward, Low, Aerial, Down };
public enum HitBoxScale { Jab, ArmourBreak, Aerial };
public enum FollowDes { Centre, RightHand, RightElbow, LeftHand, LeftElbow , RightFoot, LeftFoot}
public class Hitbox : MonoBehaviour
{
    private float gaugeDamageValue = 1.5f;
    public bool viewHitBox;
    public HitBoxScale _hitBoxScale;
    public Attackdirection _attackDir;
    public AttackType _attackType;
    private FollowDes _followDes;
    MeshRenderer meshRenderer;
    Collider hitboxCollider;
    public Transform HeadArmourPosition, ChestArmourPosition, LegArmourPosition;
    [SerializeField] float LegLocationOffset;

    [SerializeField] private float freezeCounter;
    [SerializeField] private float freezeStep;
    [SerializeField] private float freezeMaxValue;
    public PlayerActions animantionManager;
    public Animator anim;
    [SerializeField] Player player;
    PlayerInputHandler playerInput;
    HitBoxManager hitBoxManager;

    [SerializeField] private bool freezeCharacter;
    public List<GameObject> HitHurtBoxes = new List<GameObject>();
    public List<GameObject> HurtboxList = new List<GameObject>();

    public int armIndex;
    public GameObject tipHitBox, midHitBox;
    public GameObject rightHand, leftHand,rightElbow, leftElbow, rightFoot, leftFoot, rightKnee, leftKnee, waist;

    public ParticleSystem hitParticle;
    public ParticleSystem dustHitParticle;
    [SerializeField] ParticleManager particleManager;

    Vector3 hitDirection;

    public void AddHurtBoxToList(GameObject obj)
    {
        HurtboxList.Add(obj);
    }
    private void Awake()
    {
        hitboxCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        hitBoxManager = GetComponent<HitBoxManager>();
        playerInput = GetComponentInParent<PlayerInputHandler>();
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
    }
    private void FixedUpdate()
    {
        AttackTypeCall();
        HitBoxSize();
        GaugeDamage();
    }
    void AttackTypeCall()
    {
        switch(_attackType)
        {
            case AttackType.Jab:
                FollowHand();
                break;
            case AttackType.LegSweep:
                FollowHand();
                break;
            case AttackType.Aerial:
                FollowRightElbow();
                break;
            case AttackType.ArmourBreak:
                FollowCenter();
                break;
            case AttackType.HeavyJab:
                FollowHand();
                break;
        }
    }
    private void GaugeDamage()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                gaugeDamageValue = 2.5f;
                break;
            case AttackType.HeavyJab:
                gaugeDamageValue = 4;
                break;
            case AttackType.Aerial:
                gaugeDamageValue = 3;
                break;
            case AttackType.LegSweep:
                gaugeDamageValue = 3;
                break;
            case AttackType.ArmourBreak:
                gaugeDamageValue = 5;
                break;
        }
    }
    void HitBoxSize()
    {
        switch (_hitBoxScale)
        {
            case HitBoxScale.Jab:
                transform.localScale = new Vector3(0.4f,0.4f,0.4f);
                break;
            case HitBoxScale.ArmourBreak:
                transform.localScale = new Vector3(1.5f,1.5f,1.5f);
                break;
            case HitBoxScale.Aerial:
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                break;
        }
    }
    void HitboxPosition()
    {
        switch(_followDes)
        {
            case FollowDes.Centre:
                break;
            case FollowDes.RightHand:
                tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
                break;
            case FollowDes.RightElbow:
                tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
                break;
            case FollowDes.RightFoot:
                tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
                break;
            case FollowDes.LeftHand:
                tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
                break;
            case FollowDes.LeftElbow:
                break;
            case FollowDes.LeftFoot:
                tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
                tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
                break;
        }
    }
    public void FollowCenter()
    {
        tipHitBox.gameObject.transform.position = waist.transform.position;
        tipHitBox.gameObject.transform.rotation = waist.transform.rotation;
    }
    public void FollowHand()
    {
        if (armIndex == 0)
        {
            tipHitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = leftHand.transform.rotation;
        }
        else if (armIndex == 1)
        {
            tipHitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
            tipHitBox.gameObject.transform.rotation = rightHand.transform.rotation;
        }
    }
    public void FollowRightElbow()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
    }
    public void FollowRightFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
    }
    public void FollowLeftFoot()
    {
        tipHitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
        tipHitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
    }
    [SerializeField] private Vector3 JabStrength;//30,3
    [SerializeField] private Vector3 LegSweepStrength;//20,7
    [SerializeField] private Vector3 AerialStrength;//35,-0.5f
    [SerializeField] private Vector3 ArmourBreakStrength;//50,2
    [SerializeField] private Vector3 HeavyStrength;//40,2

    public Vector3 KnockBackStrength()
    {
        switch (_attackType)
        {
            case AttackType.Jab:
                return new Vector3(player.GetFacingDirection() * JabStrength.x, JabStrength.y, 0);
            case AttackType.LegSweep:
                return new Vector3(player.GetFacingDirection() * LegSweepStrength.x, LegSweepStrength.y, 0);
            case AttackType.Aerial:
                return new Vector3(player.GetFacingDirection() * AerialStrength.x, AerialStrength.y, 0);
            case AttackType.ArmourBreak:
                return new Vector3(player.GetFacingDirection() * ArmourBreakStrength.x, ArmourBreakStrength.y, 0);
            case AttackType.HeavyJab:
                return new Vector3(player.GetFacingDirection() * HeavyStrength.x, HeavyStrength.y, 0);
        }
        return new Vector3(player.GetFacingDirection() * 5, 2, 0);
    }

    public void ShowHitBoxes()
    {
        //meshRenderer.enabled = true;
        hitboxCollider.enabled = true;
    }
    public void HideHitBoxes()
    {
        meshRenderer.enabled = false;
        hitboxCollider.enabled = false;

    }
    private void ResetMoveValues(Player DefendingPlayer, Player attackingPlayer)
    {
        DefendingPlayer.changeHeavyMoveValue(AttackType.HeavyJab ,600);
        DefendingPlayer.changeHeavyMoveValue(AttackType.Jab, 200);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            return;
        }


        if (other.gameObject.CompareTag("Hurtbox"))
        {
            
            var tempDefendingPlayer = other.GetComponentInParent<Player>();
            var tempAttackingPlayer = GetComponentInParent<Player>();

            var tempDefendingPlayerHitBox = other.GetComponentInChildren<Hitbox>();
            var temptArmourCheck = other.GetComponentInParent<ArmourCheck>();
            HurtBox tempHurtBox = other.gameObject.GetComponent<HurtBox>();
            var tempHitPosiiton = other.transform.position;
            tempHurtBox.TurnOnHitBoxHit();
            Debug.Log("Hit Character");
            if (tempDefendingPlayer.Blocking == true)
            {
                if(_attackType != AttackType.LegSweep)
                {
                    ResetMoveValues(tempDefendingPlayer, tempAttackingPlayer);
                    HideHitBoxes();
                    return;
                }
                else
                {
                    DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                    tempDefendingPlayer.HideHitBoxes();
                    //tempDefendingPlayer.ResetCharacterMaterialToStandard();
                }
            }
            else
            {
                DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                tempDefendingPlayer.HideHitBoxes();
            }
        }
    }
    private void DamagingPlayer(Player DefendingPlayer, Player attackingPlayer, ArmourCheck armourCheck, HurtBox hurtBox)
    {
        DefendingPlayer.SetAddforceZero();
        DefendingPlayer.SetVelocityToZero();
        DefendingPlayer.changeHeavyMoveValue(AttackType.HeavyJab ,0);
        DefendingPlayer.changeHeavyMoveValue(AttackType.Jab, 0);
        ApplyDamageToPlayer(DefendingPlayer, attackingPlayer, _attackType);
        if (hurtBox.BodyLocation == LocationTag.Chest)
        {
            Hitbox DefendingHitbox = DefendingPlayer.GetComponentInChildren<Hitbox>();
            Transform hitLocation = DefendingHitbox.ChestArmourPosition;
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Chest, _attackType, hitLocation.position);
            Instantiate(hitParticle, transform.position, transform.rotation);
            //Instantiate(dustHitParticle, transform.position, transform.rotation);

        }
        else if (hurtBox.BodyLocation == LocationTag.Legs)
        {
            Hitbox DefendingHitbox = DefendingPlayer.GetComponentInChildren<Hitbox>();
            Transform hitLocation = DefendingHitbox.LegArmourPosition;
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Legs, _attackType, (hitLocation.position + new Vector3(LegArmourPosition.position.x, (LegArmourPosition.position.y - 1), LegArmourPosition.position.z)));
            Instantiate(hitParticle, transform.position, transform.rotation);
            //Instantiate(dustHitParticle, transform.position, transform.rotation);
        }
        else if(hurtBox.BodyLocation == LocationTag.Head)
        {
            Hitbox DefendingHitbox = DefendingPlayer.GetComponentInChildren<Hitbox>();
            Transform hitLocation = DefendingHitbox.HeadArmourPosition;
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Head, _attackType, hitLocation.position);
            Instantiate(hitParticle, transform.position, transform.rotation);
        }
    }
    void ApplyDamageToPlayer(Player defendingPlayer, Player attackingPlayer, AttackType attackType)
    {
        Debug.Log("Defender hit stun");


        defendingPlayer.FreezeCharacterBeingAttacked(KnockBackStrength(), attackingPlayer.GetFacingDirection());
        attackingPlayer.FreezeCharacterAttacking();
        if(attackType == AttackType.Aerial || attackType == AttackType.ArmourBreak || attackType == AttackType.HeavyJab)
        {
            defendingPlayer.MaxHitStun = 1.5f;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.LegSweep)
        {
            defendingPlayer.MaxHitStun = 1.5f;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();

        }
        else if(attackType == AttackType.Jab)
        {
            defendingPlayer.MaxHitStun = 1f;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.JabKnockBack();
        }

        ResetMoveValues(defendingPlayer, attackingPlayer);
        HideHitBoxes();
    }
}
