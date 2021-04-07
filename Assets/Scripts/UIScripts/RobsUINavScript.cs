using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RobsUINavScript : MonoBehaviour
{
    [SerializeField] EventSystem Event = null;
    [SerializeField] Scrollbar scroller = null;
    [SerializeField] GameObject contents = null;
    public Toggle[] ToggleObjs = null;
    public List<GameObject> contentChildren = new List<GameObject>();

    void Start()
    {
        scroller = gameObject.GetComponentInChildren<Scrollbar>();

        //Get all children of object and assign object variable to contentChildren list
        ToggleObjs = contents.GetComponentsInChildren<Toggle>();
        foreach (Toggle obj in ToggleObjs)
        {
            contentChildren.Add(obj.gameObject);
        }
    }

    private void Update()
    {
        float value = (GetSelectedToggle() / contentChildren.Count) / 1;
        float invertValue = 1 - value;
        scroller.value = invertValue;

        print(value);
    }

    float GetSelectedToggle()
    {
        for (int i = 1; i < contentChildren.Count; i++)
        {
            if (contentChildren[i] == Event.currentSelectedGameObject)
            {
                return i + 1;
            }
        }
        return 0;
    }

}

