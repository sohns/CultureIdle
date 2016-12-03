using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class TechnologyColumnScript : MonoBehaviour {

	public Text nameText, costText;
	public Button statusButton, infoButton;
	private TechnologyManager technologyManager;
	private TechnologyType technologyType;
	private InfoPanel infoPanel;

	public void SetBase (TechnologyType technologyTypeToSet, InfoPanel infoPanel)
	{
		this.infoPanel = infoPanel;
		nameText = this.transform.Find ("Name").Find ("Text").gameObject.GetComponent<Text> ();
		costText = this.transform.Find ("Cost").Find ("Text").gameObject.GetComponent<Text> ();
		statusButton = this.transform.Find ("Status").gameObject.GetComponent<Button> ();
		infoButton = this.transform.Find ("Info").gameObject.GetComponent<Button> ();
		technologyManager = (TechnologyManager)GameObject.Find ("Technologies").GetComponent ("TechnologyManager");
		statusButton.onClick.AddListener (() => onStatusButtonClick ());
		SetShallowData (technologyTypeToSet);
	}

	public void SetShallowData (TechnologyType technologyTypeToSet){
		this.technologyType = technologyTypeToSet;
		SummonExtraInfo summonExtraInfo=((SummonExtraInfo)infoButton.gameObject.GetComponent<SummonExtraInfo> ());
		summonExtraInfo.setInitialInfo (infoPanel, technologyTypeToSet.ExtraInfo);
	}

	void Start ()
	{
	}

	public void onStatusButtonClick ()
	{
		technologyManager.addTechnologyToQueue (technologyType);
	}
	public TechnologyType TechnologyType {
		get {
			return this.technologyType;
		}
	}
	public Text NameText {
		get {
			return this.nameText;
		}
	}

	public Text CostText {
		get {
			return this.costText;
		}
	}

	public Button StatusButton {
		get {
			return this.statusButton;
		}
	}

	public Button InfoButton {
		get {
			return this.infoButton;
		}
	}
}
