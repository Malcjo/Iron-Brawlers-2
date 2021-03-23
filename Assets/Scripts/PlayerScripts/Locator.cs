using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationTag { Legs, Chest, Head}
public class Locator : MonoBehaviour
{
    [SerializeField]
    private LocationTag BodyLocation;
    [SerializeField]
    private Transform Location;
    [SerializeField]
    private float Radius;

    // Start is called before the first frame update
    void Awake()
    {
        transform.SetParent(Location);
    }

    public float GetRadius() => Radius;
    public LocationTag GetBodyLocation() => BodyLocation;
}
