using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomPanelThree : MonoBehaviour
{

	private InfoPanel infoPanel;
	private Button buttonOne, buttonTwo, buttonThree, title;

	// Use this for initialization
	void Start ()
	{
		infoPanel = (InfoPanel)GameObject.FindWithTag ("InfoPanel").GetComponent ("InfoPanel");
		buttonOne = this.transform.Find ("Button1").GetComponent<Button> ();
		buttonOne.onClick.AddListener (() => onButtonOneClick ());
		buttonTwo = this.transform.Find ("Button2").GetComponent<Button> ();
		buttonTwo.onClick.AddListener (() => onButtonTwoClick ());
		buttonThree = this.transform.Find ("Button3").GetComponent<Button> ();
		buttonThree.onClick.AddListener (() => onButtonThreeClick ());
		title = this.transform.Find ("Title").GetComponent<Button> ();
		title.onClick.AddListener (() => onTitleClick ());
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public void onButtonOneClick ()
	{
		//infoPanel.setPanel (InfoPanelEnum.KINGDOM_STATISTICS);
	}

	public void onButtonTwoClick ()
	{
		//infoPanel.setPanel (InfoPanelEnum.KINGDOM_RESOURCES);
	}

	public void onButtonThreeClick ()
	{
		//infoPanel.setPanel (InfoPanelEnum.KINGDOM_CIVILIZATION_POINTS);
	}

	public void onTitleClick ()
	{
	}
}
