using UnityEngine;
using System.Collections;

public class KingdomStatisticPanel : MonoBehaviour , Panel{

	//Public and used for testing
	public bool isActiveTab;

	// Use this for initialization
	void Start () {
	}

	public void setInitialInfo(InfoPanel infoPanel){
		this.transform.localPosition = new Vector3 (0, 0, 0);
		this.setInactive ();
	}
	public void setActive ()
	{
		this.gameObject.SetActive (true);
		isActiveTab = true;
	}

	public void setInactive ()
	{
		this.gameObject.SetActive (false);
		isActiveTab = false;

	}

	// Update is called once per frame
	void Update () {
	
	}
	public void infomationHasChanged(){
	}
}
