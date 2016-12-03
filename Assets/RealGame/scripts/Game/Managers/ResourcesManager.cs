using UnityEngine;
using System.Collections.Generic;

public class ResourcesManager : MonoBehaviour, Incremental
{

	private BuildingManager buildingManager;
	//Resources
	private Dictionary<ResourceType,ResourceData> resources = new Dictionary<ResourceType,ResourceData> ();
	private Dictionary<ResourceType,BiggerNumber> totalResources = new Dictionary<ResourceType,BiggerNumber> ();
	private Dictionary<ResourceType,BiggerNumber> currentRateResources = new Dictionary<ResourceType,BiggerNumber> ();
	//AdvancedResources
	private Dictionary<AdvancedResourceType,AdvancedResourceData> advancedResources = new Dictionary<AdvancedResourceType,AdvancedResourceData> ();
	private Dictionary<AdvancedResourceType,BiggerNumber> totalAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
	private Dictionary<AdvancedResourceType,BiggerNumber> currentRateAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
	//CivilizationPoints
	private Dictionary<CivilizationPointType,CivilizationPointData> civilizationPoints = new Dictionary<CivilizationPointType,CivilizationPointData> ();
	private Dictionary<CivilizationPointType,BiggerNumber> totalCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();
	private Dictionary<CivilizationPointType,BiggerNumber> currentRateCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();

	// Use this for initialization
	void Start ()
	{
		buildingManager = (BuildingManager)GameObject.Find ("Buildings").GetComponent ("BuildingManager");
	}

	public void reset ()
	{
		//Resources
		resources = new Dictionary<ResourceType,ResourceData> ();
		totalResources = new Dictionary<ResourceType,BiggerNumber> ();
		currentRateResources = new Dictionary<ResourceType,BiggerNumber> ();
		//AdvancedResources
		advancedResources = new Dictionary<AdvancedResourceType,AdvancedResourceData> ();
		totalAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
		currentRateAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
		//CivilizationPoints
		civilizationPoints = new Dictionary<CivilizationPointType,CivilizationPointData> ();
		totalCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();
		currentRateCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();
	}

	//Update resources from projects/challenges
	public void OnUpdate (float rate)
	{
		//Resources from buildings
		Dictionary<BuildingType,BuildingData> buildings = buildingManager.Buildings;
		//ToDo make this generic somehow??
		currentRateResources = new Dictionary<ResourceType,BiggerNumber> ();
		currentRateAdvancedResources = new Dictionary<AdvancedResourceType,BiggerNumber> ();
		currentRateCivilizationPoints = new Dictionary<CivilizationPointType,BiggerNumber> ();
		foreach (BuildingData buildingData in buildings.Values) {
			BiggerNumber amountOwned = buildingData.AmountOwned;
			CurrencyData[] currentGenerates = buildingData.CurrentGenerates;
			//TODO refactor negatives
			foreach (CurrencyData currencyData in currentGenerates) {
				BiggerNumber amountToIncrease = currencyData.BaseAmount.MultNumber (amountOwned);
				if (amountToIncrease.CompareTo (0) == 0) {
					continue;
				}
				BiggerNumber amountToIncreaseByRate = amountToIncrease.MultNumber (rate);
				if (currencyData.isResourceType ()) {
					BiggerNumber amountGained = increaseCurrentAmount (resources, currencyData, amountToIncreaseByRate);
					calculateCurrentRate (currentRateResources, currencyData, amountToIncrease);
				} else if (currencyData.isAdvancedResourceType ()) {
					BiggerNumber amountGained = increaseCurrentAmount (advancedResources, currencyData, amountToIncreaseByRate);
					calculateCurrentRate (currentRateAdvancedResources, currencyData, amountToIncrease);
				} else if (currencyData.isCivilizationPointType ()) {
					//TODO this better:P
					CivilizationPointData data = civilizationPoints [currencyData.CurrencyType as CivilizationPointType];
					if (data.CurrentAmount.CompareTo (0) < 1) {
						data.CurrentAmount = 0;
						continue;
					}
					BiggerNumber amountGained = increaseCurrentAmount (civilizationPoints, currencyData, amountToIncreaseByRate);
					calculateCurrentRate (currentRateCivilizationPoints, currencyData, amountToIncrease);
				} else {
					Debug.LogError ("uncharted resource being generated" + currencyData.ToString ());
				}
			}
		}
		updateTotalAmounts (currentRateResources, totalResources);
		updateTotalAmounts (currentRateAdvancedResources, totalAdvancedResources);
		updateTotalAmounts (currentRateCivilizationPoints, totalCivilizationPoints);
	}

	private void updateTotalAmounts<T> (Dictionary<T,BiggerNumber> currentRates, Dictionary<T,BiggerNumber> totals)
	{
		foreach (T currentRate in currentRates.Keys) {
			BiggerNumber value = currentRates [currentRate];
			if (value.CompareTo (0) > 0) {
				calculateTotalAmount (totals, currentRate, value);
			}
		}
	}

	private void calculateTotalAmount<A> (Dictionary<A,BiggerNumber> listToIncrease, A objectType, BiggerNumber amountToIncrease)
	{
		if (listToIncrease.ContainsKey (objectType)) {
			listToIncrease [objectType] = listToIncrease [objectType].AddNumber (amountToIncrease);
		} else {
			listToIncrease.Add (objectType, amountToIncrease);
		}
	}

	private void calculateCurrentRate<A> (Dictionary<A,BiggerNumber> listToIncrease, CurrencyData currencyData, BiggerNumber amountToIncrease)
	{
		A objectType = (A)currencyData.CurrencyType;
		if (listToIncrease.ContainsKey (objectType)) {
			listToIncrease [objectType] = listToIncrease [objectType].AddNumber (amountToIncrease);
		} else {
			listToIncrease.Add (objectType, amountToIncrease);
		}
	}

	private BiggerNumber increaseCurrentAmount<A,B> (Dictionary<A,B> listToIncrease, CurrencyData currencyData, BiggerNumber amountToIncrease)
	{
		A objectType = (A)currencyData.CurrencyType;
		if (listToIncrease.ContainsKey (objectType)) {
			B thisObject = listToIncrease [objectType];
			if (currencyData.isAdvancedResourceType ()) {
				AdvancedResourceData data = thisObject as AdvancedResourceData;
				BiggerNumber temp = new BiggerNumber (data.CurrentAmount);
				BiggerNumber newValue = data.CurrentAmount.AddNumber (amountToIncrease);
				data.CurrentAmount = newValue.getMin (data.MaxAmount);
				return data.CurrentAmount.SubNumber (temp);
			} else if (currencyData.isResourceType ()) {
				ResourceData data = thisObject as ResourceData;
				BiggerNumber temp = new BiggerNumber (data.CurrentAmount);
				BiggerNumber newValue = data.CurrentAmount.AddNumber (amountToIncrease);
				data.CurrentAmount = newValue.getMin (data.MaxAmount);
				return data.CurrentAmount.SubNumber (temp);
			} else if (currencyData.isCivilizationPointType ()) {
				CivilizationPointData data = thisObject as CivilizationPointData;
				data.CurrentAmount = data.CurrentAmount.AddNumber (amountToIncrease);
				return amountToIncrease;
			} else {
				Debug.LogError ("unhandledType");
				return 0;
			}
		} else {
			Debug.LogError ("We should never be able to add to a resource we dont have yet:" + typeof(A));
			return 0;
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	public bool addNewResource<A> (A toAdd)
	{
		return addNewResource (toAdd, 0);
	}

	public bool addNewResource<A> (A toAdd, BiggerNumber startValue)
	{
		if (typeof(A) == typeof(AdvancedResourceType)) {
			
		} else if (typeof(A) == typeof(ResourceType)) {
			ResourceData data = new ResourceData (toAdd as ResourceType, startValue);
			if (!(
			        addNewResourceHelper<ResourceType,ResourceData> (resources, toAdd as ResourceType, data)
			        && addNewResourceHelper<ResourceType,BiggerNumber> (totalResources, toAdd as ResourceType, 0)
			        && addNewResourceHelper<ResourceType,BiggerNumber> (currentRateResources, toAdd as ResourceType, 0)
			    )) {
				return false;
			}
		} else if (typeof(A) == typeof(CivilizationPointType)) {
			CivilizationPointData data = new CivilizationPointData (toAdd as CivilizationPointType, startValue);
			if (!(
			        addNewResourceHelper<CivilizationPointType,CivilizationPointData> (civilizationPoints, toAdd as CivilizationPointType, data)
			        && addNewResourceHelper<CivilizationPointType,BiggerNumber> (totalCivilizationPoints, toAdd as CivilizationPointType, 0)
			        && addNewResourceHelper<CivilizationPointType,BiggerNumber> (currentRateCivilizationPoints, toAdd as CivilizationPointType, 0)
			    )) {
				return false;
			}
		} else {
			Debug.LogError ("unhandledType");
		}
		return false;
	}

	private bool addNewResourceHelper<A,B> (Dictionary<A,B> dictionaryToAdd, A keyToAdd, B valueToAdd)
	{
		if (!dictionaryToAdd.ContainsKey (keyToAdd)) {
			dictionaryToAdd.Add (keyToAdd, valueToAdd);
			return true;
		} 
		return false;
	}

	public void resetResources (Dictionary<ResourceType, ResourceData> resources, Dictionary<ResourceType, BiggerNumber> totalResources, Dictionary<AdvancedResourceType, AdvancedResourceData> advancedResources, Dictionary<AdvancedResourceType, BiggerNumber> totalAdvancedResources, Dictionary<CivilizationPointType, CivilizationPointData> civilizationPoints, Dictionary<CivilizationPointType, BiggerNumber> totalCivilizationPoints)
	{
		this.resources = resources;
		this.totalResources = totalResources;
		this.advancedResources = advancedResources;
		this.totalAdvancedResources = totalAdvancedResources;
		this.civilizationPoints = civilizationPoints;
		this.totalCivilizationPoints = totalCivilizationPoints;
	}

	public Dictionary<ResourceType, ResourceData> Resources {
		get {
			return this.resources;
		}
	}

	public Dictionary<ResourceType, BiggerNumber> TotalResources {
		get {
			return this.totalResources;
		}
	}

	public Dictionary<ResourceType, BiggerNumber> CurrentRateResources {
		get {
			return this.currentRateResources;
		}
	}

	public Dictionary<AdvancedResourceType, AdvancedResourceData> AdvancedResources {
		get {
			return this.advancedResources;
		}
	}

	public Dictionary<AdvancedResourceType, BiggerNumber> TotalAdvancedResources {
		get {
			return this.totalAdvancedResources;
		}
	}

	public Dictionary<AdvancedResourceType, BiggerNumber> CurrentRateAdvancedResources {
		get {
			return this.currentRateAdvancedResources;
		}
	}

	public Dictionary<CivilizationPointType, CivilizationPointData> CivilizationPoints {
		get {
			return this.civilizationPoints;
		}
	}

	public Dictionary<CivilizationPointType, BiggerNumber> TotalCivilizationPoints {
		get {
			return this.totalCivilizationPoints;
		}
	}

	public Dictionary<CivilizationPointType, BiggerNumber> CurrentRateCivilizationPoints {
		get {
			return this.currentRateCivilizationPoints;
		}
	}
}
