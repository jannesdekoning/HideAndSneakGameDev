using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.transform.position.x > 25 || obj.transform.position.x < -25)
            {
                obj.transform.position = new Vector3(obj.transform.position.x/25, obj.transform.position.y, obj.transform.position.z);
            }
            if (obj.transform.position.z > 25 || obj.transform.position.z < -25)
            {
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z/25);
            }
        }
    }
}
