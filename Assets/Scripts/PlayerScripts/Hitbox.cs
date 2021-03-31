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

    public JabVariables jabVariables;
    public HeavyVariables heavyVariables;
    public SweepVariables sweepVariables;
    public NeutralAerialVariables neutralAerialVariables;
    public ArmourBreakVariables armourBreakVariables;
    [HideInInspector] public float knockbackXStrength;
    [HideInInspector] public float knockbackYStrength;
    private Vector3 _knockbackStrength;
    [System.Serializable]
    public struct JabVariables
    { 
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge; 
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter; 
    }
    [System.Serializable]
    public struct HeavyVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct SweepVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct NeutralAerialVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    }
    [System.Serializable]
    public struct ArmourBreakVariables
    {
        public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
        public float KnockbackXStrength; public float KnockbackYStrength;
        public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    }



    MeshRenderer meshRenderer;

    Collider hitboxCollider;
    [Space(10)]
    public Transform HeadArmourPosition;
    public Transform ChestArmourPosition;
    public Transform LegArmourPosition;
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
        HitboxPosition();
        switch (_attackType)
        {
            case AttackType.Jab:
                _followDes = jabVariables.FollowingDestination;
                gaugeDamageValue = jabVariables.DamageOnGauge;
                transform.localScale = Vector3.one * jabVariables.HitBoxSize;
                _knockbackStrength = new Vector3(player.GetFacingDirection() * jabVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
                break;
            case AttackType.HeavyJab:
                _followDes = heavyVariables.FollowingDestination;
                gaugeDamageValue = heavyVariables.DamageOnGauge;
                transform.localScale = Vector3.one * heavyVariables.HitBoxSize;
                _knockbackStrength = new Vector3(player.GetFacingDirection() * heavyVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
                break;
            case AttackType.LegSweep:
                _followDes = sweepVariables.FollowingDestination;
                gaugeDamageValue = sweepVariables.DamageOnGauge;
                transform.localScale = Vector3.one * sweepVariables.HitBoxSize;
                _knockbackStrength = new Vector3(player.GetFacingDirection() * sweepVariables.KnockbackXStrength, sweepVariables.KnockbackYStrength, 0);
                break;
            case AttackType.Aerial:
                _followDes = neutralAerialVariables.FollowingDestination;
                gaugeDamageValue = neutralAerialVariables.DamageOnGauge;
                transform.localScale = Vector3.one * neutralAerialVariables.HitBoxSize;
                _knockbackStrength = new Vector3(player.GetFacingDirection() * neutralAerialVariables.KnockbackXStrength, neutralAerialVariables.KnockbackYStrength, 0);
                break;
            case AttackType.ArmourBreak:
                _followDes = armourBreakVariables.FollowingDestination;
                gaugeDamageValue = armourBreakVariables.DamageOnGauge;
                transform.localScale = Vector3.one * armourBreakVariables.HitBoxSize;
                _knockbackStrength = new Vector3(player.GetFacingDirection() * armourBreakVariables.KnockbackXStrength, armourBreakVariables.KnockbackYStrength, 0);
                break;

        }
    }
    /*
     * Sol
     * jab- gague damage: 2.5f |Scale: 0.4 |Position: left hand |Strength: 25, 10 |Cancel time: 0.5 |MaxMoveTimeValue: 0.25 |Move strength: 6 |WhenToMoveCharacter: 0.2
     * heavy- gague damage: 4 |Scale: 0.4 |Position: right hand |Strength: 40, 2 |Cancel time: 0.8 |MaxMoveTimeValue: 3 |Move strength: 0.5 |WhenToMoveCharacter: 0.2
     * sweep- gague damage: 3 |Scale: 0.4 |Position: right hand |Strength: 20, 30 |Cancel time: 0.6 |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
     * aerial- gague damage: 3 |Scale: 0.4 |Position: right elbow |Strength: 35, -0.5 |Cancel time: 0.7 |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
     * armourbreak- gague damage: 5 |Scale: 1 |Position: Center |Strength: 50, 2 |Cancel time: |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
     * 
     * Goblin
     * jab- gague damage :Scale :Position :Strength :
     * heavy- gague damage :Scale :Position :Strength :
     * sweep- gague damage :Scale :Position :Strength :
     * aerial- gague damage :Scale :Position :Strength :
     * armourbreak- gague damage :Scale :Position :Strength :
     * 
     * 
     * 
     * gague damage:  |Scale:  |Position: |Strength:  |Cancel time: |MaxMoveTimeValue:  |Move strength:  |WhenToMoveCharacter:
     */

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

    public Vector3 KnockBackStrength()
    {
        return _knockbackStrength;
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
        //DefendingPlayer.changeHeavyMoveValue(AttackType.HeavyJab ,600);
        //DefendingPlayer.changeHeavyMoveValue(AttackType.Jab, 200);
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
        defendingPlayer.FreezeCharacterBeingAttacked(KnockBackStrength(), attackingPlayer.GetFacingDirection());
        attackingPlayer.FreezeCharacterAttacking();
        if(attackType == AttackType.Aerial)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.aerialHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();
        }
        else if (attackType == AttackType.HeavyJab)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.heavyHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.ArmourBreak)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.armourBreakHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.LegSweep)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.sweepHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.KnockDown();

        }
        else if(attackType == AttackType.Jab)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.jabHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            defendingPlayer.JabKnockBack();
        }

        ResetMoveValues(defendingPlayer, attackingPlayer);
        HideHitBoxes();
    }
}
