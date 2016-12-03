using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActiveProjectBaseData {

	private object activeProjectType;
	private List<ActiveProjectCost> costs=new List<ActiveProjectCost>();
	private BiggerNumber timeLeft;//in sec
	private bool isPaused;
	private Guid uniqueId; 

	public ActiveProjectBaseData (object activeProjectType, List<ActiveProjectCost> costs, BiggerNumber timeLeft, bool isPaused)
	{
		this.activeProjectType = activeProjectType;
		this.costs = costs;
		this.timeLeft = timeLeft;
		this.isPaused = isPaused;
		uniqueId = Guid.NewGuid ();
	}
	
	public ActiveProjectCost getSpecificCost(object specificType){
		foreach (ActiveProjectCost activeProjectCost in costs) {
			if (activeProjectCost.ActiveProjectCostType == specificType) {
				return activeProjectCost;
			}
		}
		Debug.LogError ("could not find type");
		return null;
	}
	public Guid UniqueId {
		get {
			return this.uniqueId;
		}
	}
	public object ActiveProjectType {
		get {
			return this.activeProjectType;
		}
	}

	public List<ActiveProjectCost> Costs {
		get {
			return this.costs;
		}
	}

	public BiggerNumber TimeLeft {
		get {
			return this.timeLeft;
		}
		set {
			this.timeLeft = value;
		}
	}

	public bool IsPaused {
		get {
			return this.isPaused;
		}
	}

	public bool isBuildingType(){
		return activeProjectType.GetType () == typeof(BuildingType);
	}
	public bool isTechnologyType(){
		return activeProjectType.GetType () == typeof(TechnologyType);
	}
}
