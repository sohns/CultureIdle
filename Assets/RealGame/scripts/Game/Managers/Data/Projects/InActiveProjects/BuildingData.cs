using System.Collections;
using UnityEngine;

public class BuildingData
{
	private BuildingType buildingType;
	private BiggerNumber amountOwned;
	private BiggerNumber amountBuilt;
	private CurrencyData[] currentCost;
	private CurrencyData[] currentGenerates;

	public BuildingData (BuildingType buildingType)
	{
		this.buildingType = buildingType;
		this.amountOwned = new BiggerNumber (0);
		this.currentCost = buildingType.Costs;
		this.currentGenerates = buildingType.Generates;
	}

	public BuildingData (BuildingType buildingType, BiggerNumber amount)
	{
		this.buildingType = buildingType;
		this.amountOwned = amount;
		this.currentCost = buildingType.Costs;
		this.currentGenerates = buildingType.Generates;
		this.amountBuilt = 0;
	}

	public BuildingData (BuildingType buildingType, BiggerNumber amount, BiggerNumber amountBuilt)
	{
		this.buildingType = buildingType;
		this.amountOwned = amount;
		this.currentCost = buildingType.Costs;
		this.currentGenerates = buildingType.Generates;
		increaseCosts (amountBuilt);
		this.amountBuilt = amountBuilt;
	}

	public BuildingType BuildingType {
		
		get {
			return this.buildingType;
		}
	}

	public BiggerNumber AmountOwned {
		set {
			this.amountOwned = value;
		}
		get {
			return this.amountOwned;
		}
	}

	public BiggerNumber AmountBuilt {
		get {
			return this.amountBuilt;
		}
	}

	public CurrencyData[] CurrentCost {
		get {
			return this.currentCost;
		}
	}

	public CurrencyData[] CurrentGenerates {
		get {
			return this.currentGenerates;
		}
	}

	public void increaseCosts (BiggerNumber amountOfTimes)
	{
		foreach (CurrencyData currencyData in currentCost) {
			currencyData.incrementCurrency (amountOfTimes);
		}
	}

	public void increaseCosts ()
	{
		foreach (CurrencyData currencyData in currentCost) {
			currencyData.incrementCurrency ();
		}
	}

	public void increaseAmountOwned ()
	{
		amountOwned = amountOwned.AddNumber (1);
	}

	public void increaseAmountBuilt ()
	{
		amountBuilt = amountBuilt.AddNumber (1);
	}
}
