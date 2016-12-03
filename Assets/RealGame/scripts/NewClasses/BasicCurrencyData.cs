using UnityEngine;
using System.Collections;

public class BasicCurrencyData
{

	private object currencyType;

	public BasicCurrencyData (object currencyType)
	{
		this.currencyType = currencyType;
	}

	public object CurrencyType {
		get {
			return this.currencyType;
		}
	}

	public bool isResourceType ()
	{
		return currencyType.GetType () == typeof(ResourceType);
	}

	public bool isBuildingType ()
	{
		return currencyType.GetType () == typeof(BuildingType);
	}

	public bool isAdvancedResourceType ()
	{
		return currencyType.GetType () == typeof(AdvancedResourceType);
	}

	public bool isTechnologyType ()
	{
		return currencyType.GetType () == typeof(TechnologyType);
	}

	public bool isCivilizationPointType ()
	{
		return currencyType.GetType () == typeof(CivilizationPointType);
	}
}
