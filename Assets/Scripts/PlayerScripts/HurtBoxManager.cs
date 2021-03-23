using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState { Closed, Open, Colliding }
public class HurtBoxManager : MonoBehaviour
{
    private ColliderState _state;
    [SerializeField] private Hitbox hitBoxScript;
    public bool viewHurtBoxes;

    public GameObject[] locator;
    public GameObject hurtbox;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        for(int i = 0; i < locator.Length; i++)
        {
            Locator locatorScript = locator[i].GetComponent<Locator>();
            float tempLocatorRadius = locatorScript.GetRadius();

            GameObject tempHurtBox = Instantiate(hurtbox, locator[i].transform.position, Quaternion.identity, locator[i].transform);
            hitBoxScript.AddHurtBoxToList(tempHurtBox);

            HurtBox tempHurtBoxScript = tempHurtBox.GetComponent<HurtBox>();
            tempHurtBoxScript.BodyLocation = locatorScript.GetBodyLocation();
            tempHurtBoxScript.SetRadius(tempLocatorRadius * 0.1f);
            tempHurtBox.transform.localScale = Vector3.one * (tempLocatorRadius);
            if(viewHurtBoxes == true)
            {
                tempHurtBox.GetComponent<MeshRenderer>().enabled = true;
            }
            else if (viewHurtBoxes == false)
            {
                tempHurtBox.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
