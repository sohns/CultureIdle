using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class BuildingColumnScript : MonoBehaviour
{
	private Text nameText, amountText, costText;
	private Button buyButton, infoButton;
	private BuildingManager buildingManager;
	private BuildingType buildingType;
	private InfoPanel infoPanel;

	public void SetBase (BuildingType buildingTypeToSet, InfoPanel infoPanel)
	{
		this.infoPanel = infoPanel;
		nameText = this.transform.Find ("Name").Find ("Text").gameObject.GetComponent<Text> ();
		amountText = this.transform.Find ("Amount").Find ("Text").gameObject.GetComponent<Text> ();
		costText = this.transform.Find ("Cost").Find ("Text").gameObject.GetComponent<Text> ();
		buyButton = this.transform.Find ("Buy").gameObject.GetComponent<Button> ();
		infoButton = this.transform.Find ("Info").gameObject.GetComponent<Button> ();
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
		buyButton.onClick.AddListener (() => onBuyButtonClick ());
		SetShallowData (buildingTypeToSet);
	}

	public void SetShallowData (BuildingType buildingTypeToSet)
	{
		this.buildingType = buildingTypeToSet;
		SummonExtraInfo summonExtraInfo = ((SummonExtraInfo)infoButton.gameObject.GetComponent<SummonExtraInfo> ());
		summonExtraInfo.setInitialInfo (infoPanel, buildingTypeToSet.ExtraInfo);
	}

	void Start ()
	{
	}

	public void onBuyButtonClick ()
	{
		buildingManager.addBuildingToQueue (buildingType);
	}

	public Text NameText {
		get {
			return this.nameText;
		}
	}

	public Text AmountText {
		get {
			return this.amountText;
		}
	}

	public Text CostText {
		get {
			return this.costText;
		}
	}

	public Button BuyButton {
		get {
			return this.buyButton;
		}
	}

	public BuildingType BuildingType {
		get {
			return this.buildingType;
		}
	}
}
