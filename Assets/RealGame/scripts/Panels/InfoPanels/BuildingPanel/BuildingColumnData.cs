using UnityEngine;
using System.Collections;

public class BuildingColumnData {

	private string name;
	private BiggerNumber amount;
	private string costText;
	private bool canBuy;
	private BuildingType buildingType;
	public BuildingColumnData (string name, BiggerNumber amount, string costText, bool canBuy, BuildingType buildingType)
	{
		this.name = name;
		this.amount = amount;
		this.costText = costText;
		this.canBuy = canBuy;
		this.buildingType = buildingType;
	}
	public string Name {
		get {
			return this.name;
		}
	}

	public BiggerNumber Amount {
		get {
			return this.amount;
		}
	}

	public string CostText {
		get {
			return this.costText;
		}
	}

	public bool CanBuy {
		get {
			return this.canBuy;
		}
	}
	public BuildingType BuildingType {
		get {
			return this.buildingType;
		}
	}
}
