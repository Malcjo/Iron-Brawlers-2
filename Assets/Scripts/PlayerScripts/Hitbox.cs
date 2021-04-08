using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Null, Jab, LegSweep, Aerial, ArmourBreak, Heavy, BackAir, UpAir};
public enum Attackdirection { Forward, Low, Aerial, Down };
public enum HitBoxScale { Jab, ArmourBreak, Aerial };
public enum FollowDes { Centre, RightHand, RightElbow, LeftHand, LeftElbow , RightFoot, LeftFoot, Head}
public class Hitbox : MonoBehaviour
{

    public bool viewHitBox;
    public HitBoxScale _hitBoxScale;
    public Attackdirection _attackDir;
    public AttackType _attackType;

    public float gaugeDamageValue = 1.5f;
    public FollowDes _followDes;
    public Vector3 _knockbackStrength;

    //public JabVariables jabVariables;
    //public HeavyVariables heavyVariables;
    //public SweepVariables sweepVariables;
    //public NeutralAerialVariables neutralAerialVariables;
    //public ArmourBreakVariables armourBreakVariables;

    //[System.Serializable]
    //public struct JabVariables
    //{ 
    //    public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge; 
    //    public float KnockbackXStrength; public float KnockbackYStrength;
    //    public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter; 
    //}
    //[System.Serializable]
    //public struct HeavyVariables
    //{
    //    public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
    //    public float KnockbackXStrength; public float KnockbackYStrength;
    //    public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    //}
    //[System.Serializable]
    //public struct SweepVariables
    //{
    //    public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
    //    public float KnockbackXStrength; public float KnockbackYStrength;
    //    public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    //}
    //[System.Serializable]
    //public struct NeutralAerialVariables
    //{
    //    public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
    //    public float KnockbackXStrength; public float KnockbackYStrength;
    //    public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    //}
    //[System.Serializable]
    //public struct ArmourBreakVariables
    //{
    //    public FollowDes FollowingDestination; public float HitBoxSize; public float DamageOnGauge;
    //    public float KnockbackXStrength; public float KnockbackYStrength;
    //    public float CancelTime; public float WhenToMoveCharacterInAnimation; public float MoveCharacterStrength; public float MoveCharacterMaxCounter;
    //}



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
    public GameObject hitBox, midHitBox;
    public GameObject rightHand, leftHand,rightElbow, leftElbow, rightFoot, leftFoot, rightKnee, leftKnee, waist, head;

    public ParticleSystem armourHitParticle;
    public ParticleSystem noArmourHitParticle;
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
        //switch (_attackType)
        //{
        //    case AttackType.Jab:
        //        _followDes = jabVariables.FollowingDestination;
        //        gaugeDamageValue = jabVariables.DamageOnGauge;
        //        transform.localScale = Vector3.one * jabVariables.HitBoxSize;
        //        _knockbackStrength = new Vector3(player.GetFacingDirection() * jabVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
        //        break;
        //    case AttackType.HeavyJab:
        //        _followDes = heavyVariables.FollowingDestination;
        //        gaugeDamageValue = heavyVariables.DamageOnGauge;
        //        transform.localScale = Vector3.one * heavyVariables.HitBoxSize;
        //        _knockbackStrength = new Vector3(player.GetFacingDirection() * heavyVariables.KnockbackXStrength, jabVariables.KnockbackYStrength, 0);
        //        break;
        //    case AttackType.LegSweep:
        //        _followDes = sweepVariables.FollowingDestination;
        //        gaugeDamageValue = sweepVariables.DamageOnGauge;
        //        transform.localScale = Vector3.one * sweepVariables.HitBoxSize;
        //        _knockbackStrength = new Vector3(player.GetFacingDirection() * sweepVariables.KnockbackXStrength, sweepVariables.KnockbackYStrength, 0);
        //        break;
        //    case AttackType.Aerial:
        //        _followDes = neutralAerialVariables.FollowingDestination;
        //        gaugeDamageValue = neutralAerialVariables.DamageOnGauge;
        //        transform.localScale = Vector3.one * neutralAerialVariables.HitBoxSize;
        //        _knockbackStrength = new Vector3(player.GetFacingDirection() * neutralAerialVariables.KnockbackXStrength, neutralAerialVariables.KnockbackYStrength, 0);
        //        break;
        //    case AttackType.ArmourBreak:
        //        _followDes = armourBreakVariables.FollowingDestination;
        //        gaugeDamageValue = armourBreakVariables.DamageOnGauge;
        //        transform.localScale = Vector3.one * armourBreakVariables.HitBoxSize;
        //        _knockbackStrength = new Vector3(player.GetFacingDirection() * armourBreakVariables.KnockbackXStrength, armourBreakVariables.KnockbackYStrength, 0);
        //        break;
        //}
    }


    void HitboxPosition()
    {
        switch(_followDes)
        {
            case FollowDes.Centre:
                hitBox.gameObject.transform.position = new Vector3(waist.transform.position.x, waist.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = waist.transform.rotation;
                break;
            case FollowDes.RightHand:
                hitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = rightHand.transform.rotation;
                break;
            case FollowDes.RightElbow:
                hitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
                break;
            case FollowDes.RightFoot:
                hitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
                break;
            case FollowDes.LeftHand:
                hitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = leftHand.transform.rotation;
                break;
            case FollowDes.LeftElbow:
                hitBox.gameObject.transform.position = new Vector3(leftElbow.transform.position.x, hitBox.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = leftElbow.transform.rotation;
                break;
            case FollowDes.LeftFoot:
                hitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
                break;
            case FollowDes.Head:
                hitBox.gameObject.transform.position = new Vector3(head.transform.position.x, head.transform.position.y, 0);
                hitBox.gameObject.transform.rotation = head.transform.rotation;
                break;
        }
    }
    public void FollowCenter()
    {
        hitBox.gameObject.transform.position = waist.transform.position;
        hitBox.gameObject.transform.rotation = waist.transform.rotation;
    }
    public void FollowHand()
    {
        if (armIndex == 0)
        {
            hitBox.gameObject.transform.position = new Vector3(leftHand.transform.position.x, leftHand.transform.position.y, 0);
            hitBox.gameObject.transform.rotation = leftHand.transform.rotation;
        }
        else if (armIndex == 1)
        {
            hitBox.gameObject.transform.position = new Vector3(rightHand.transform.position.x, rightHand.transform.position.y, 0);
            hitBox.gameObject.transform.rotation = rightHand.transform.rotation;
        }
    }
    public void FollowRightElbow()
    {
        hitBox.gameObject.transform.position = new Vector3(rightElbow.transform.position.x, rightElbow.transform.position.y, 0);
        hitBox.gameObject.transform.rotation = rightElbow.transform.rotation;
    }
    public void FollowRightFoot()
    {
        hitBox.gameObject.transform.position = new Vector3(rightFoot.transform.position.x, rightFoot.transform.position.y, 0);
        hitBox.gameObject.transform.rotation = rightFoot.transform.rotation;
    }
    public void FollowLeftFoot()
    {
        hitBox.gameObject.transform.position = new Vector3(leftFoot.transform.position.x, leftFoot.transform.position.y, 0);
        hitBox.gameObject.transform.rotation = leftFoot.transform.rotation;
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
                else if (_attackType == AttackType.LegSweep)
                {
                    DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                    tempDefendingPlayer.HideHitBoxes();
                    //tempDefendingPlayer.ResetCharacterMaterialToStandard();
                }
            }
            if(tempDefendingPlayer.CrouchBlocking == true)
            {
                if(_attackType == AttackType.LegSweep)
                {
                    ResetMoveValues(tempDefendingPlayer, tempAttackingPlayer);
                    HideHitBoxes();
                    return;
                }
                else if(_attackType != AttackType.LegSweep)
                {
                    DamagingPlayer(tempDefendingPlayer, tempAttackingPlayer, temptArmourCheck, tempHurtBox);
                    tempDefendingPlayer.HideHitBoxes();
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
            if(armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                Instantiate(armourHitParticle, transform.position, transform.rotation);
            }
            else if (armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                Instantiate(noArmourHitParticle, transform.position, transform.rotation);
            }

            //Instantiate(dustHitParticle, transform.position, transform.rotation);

        }
        else if (hurtBox.BodyLocation == LocationTag.Legs)
        {
            Hitbox DefendingHitbox = DefendingPlayer.GetComponentInChildren<Hitbox>();
            Transform hitLocation = DefendingHitbox.LegArmourPosition;
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Legs, _attackType, (hitLocation.position + new Vector3(LegArmourPosition.position.x, (LegArmourPosition.position.y - 1), LegArmourPosition.position.z)));
            if (armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                Instantiate(armourHitParticle, transform.position, transform.rotation);
            }
            else if (armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                Instantiate(noArmourHitParticle, transform.position, transform.rotation);
            }
            //Instantiate(dustHitParticle, transform.position, transform.rotation);
        }
        else if(hurtBox.BodyLocation == LocationTag.Head)
        {
            Hitbox DefendingHitbox = DefendingPlayer.GetComponentInChildren<Hitbox>();
            Transform hitLocation = DefendingHitbox.HeadArmourPosition;
            DefendingPlayer.TakeDamageOnGauge(gaugeDamageValue, ArmourCheck.ArmourPlacement.Head, _attackType, hitLocation.position);
            if (armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.armour)
            {
                Instantiate(armourHitParticle, transform.position, transform.rotation);
            }
            else if (armourCheck.ChestArmourCondition == ArmourCheck.ArmourCondition.none)
            {
                Instantiate(noArmourHitParticle, transform.position, transform.rotation);
            }
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
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
            defendingPlayer.KnockDown();
        }
        else if (attackType == AttackType.Heavy)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.heavyHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.ArmourBreak)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.armourBreakHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            FindObjectOfType<AudioManager>().Play(AudioManager.HEAVYHITUNARMOURED);
            defendingPlayer.KnockDown();
        }
        else if(attackType == AttackType.LegSweep)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.sweepHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            FindObjectOfType<AudioManager>().Play(AudioManager.JABHITUNARMOURED);
            defendingPlayer.KnockDown();

        }
        else if(attackType == AttackType.Jab)
        {
            defendingPlayer.MaxHitStun = defendingPlayer.jabHitStun;
            defendingPlayer.HitStunTimer = defendingPlayer.MaxHitStun;
            defendingPlayer.HitStun = true;
            FindObjectOfType<AudioManager>().Play(AudioManager.JABHITUNARMOURED);
            defendingPlayer.JabKnockBack();
        }

        ResetMoveValues(defendingPlayer, attackingPlayer);
        HideHitBoxes();
    }
}
