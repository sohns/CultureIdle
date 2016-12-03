using UnityEngine;
using System.Collections;

public class ActiveProjectCost {
	private object activeProjectCostType;
	private BiggerNumber maxValue, currentValue, incrementValue;

	public ActiveProjectCost (object activeProjectCostType, BiggerNumber maxValue, BiggerNumber currentValue, BiggerNumber incrementValue)
	{
		this.activeProjectCostType = activeProjectCostType;
		this.maxValue = maxValue;
		this.currentValue = currentValue;
		this.incrementValue = incrementValue;
	}
	public object ActiveProjectCostType {
		get {
			return this.activeProjectCostType;
		}
	}

	public BiggerNumber MaxValue {
		get {
			return this.maxValue;
		}
	}

	public BiggerNumber CurrentValue {
		get {
			return this.currentValue;
		}
		set {
			this.currentValue = value;
		}
	}

	public BiggerNumber IncrementValue {
		get {
			return this.incrementValue;
		}
	}
	public bool isAdvancedResourceType(){
		return activeProjectCostType.GetType () == typeof(AdvancedResourceType);
	}
}
