using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public LocationTag BodyLocation;
    [SerializeField] private float radius;
    public MeshRenderer hitBoxMesh;
    HurtBoxManager manager;
    [SerializeField] private int playerLayer;
    [SerializeField] private int opponentLayer;
    [SerializeField] private int activeLayer;
    [SerializeField] private Player self;
    private float newRadius;
    public bool HasBeenHit = false;
    private void Start()
    {
        self = GetComponentInParent<Player>();
        manager = GetComponentInParent<HurtBoxManager>();
        transform.localScale = Vector3.one * radius;
        switch (self.playerNumber)
        {
            case Player.PlayerIndex.Player1:
                playerLayer = 8;
                opponentLayer = 9;
                break;
            case Player.PlayerIndex.Player2:
                playerLayer = 9;
                opponentLayer = 8;
                break;
        }
        this.gameObject.layer = playerLayer;

        if(manager.viewHurtBoxes == false)
        {
            hitBoxMesh.enabled = false;
        }
        else if (manager.viewHurtBoxes == true)
        {
            hitBoxMesh.enabled = true;
        }
    }

    private void Update()
    {
        transform.localPosition = Vector3.zero;
    }
    public void TurnOnHitBoxHit()
    {
        HasBeenHit = true;
        Debug.Log("Hurtbox has been hit");
    }
    public void SetRadius(float externalRadius)
    {
        radius = externalRadius;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
