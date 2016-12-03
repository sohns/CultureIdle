using UnityEngine;
using System.Collections;

public class Hide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
	}
	
	public void setInactive ()
	{
		this.gameObject.SetActive (false);
	}
}
