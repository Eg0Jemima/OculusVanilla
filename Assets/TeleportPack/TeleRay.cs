using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleRay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (OVRInput.GetDown(OVRInput.Button.Any))
        {
            if (Physics.Raycast(ray, out hit, 1000))
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            if (hit.collider.name.Equals("floor_cut"))
            {
                Vector3 teleposition = hit.point;
                teleposition.y = 20;
                this.transform.parent.parent.position = teleposition;
            }
        }
        
    }
}
