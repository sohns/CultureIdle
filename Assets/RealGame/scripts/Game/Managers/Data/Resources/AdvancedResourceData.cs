using UnityEngine;
using System.Collections;

public class AdvancedResourceData : GenericResourceData
{
	private AdvancedResourceType advancedResourceType;
	private BiggerNumber maxAmount;

	public AdvancedResourceData (AdvancedResourceType advancedResourceType) : base (0)
	{
		this.advancedResourceType = advancedResourceType;
		this.maxAmount = 0;
	}

	public AdvancedResourceData (AdvancedResourceType advancedResourceType, BiggerNumber currentAmount, BiggerNumber maxAmount) : base (currentAmount)
	{
		this.advancedResourceType = advancedResourceType;
		this.maxAmount = maxAmount;
	}

	public AdvancedResourceType AdvancedResourceType {
		get {
			return this.advancedResourceType;
		}
	}

	public BiggerNumber MaxAmount {
		set {
			this.maxAmount = value;
		}
		get {
			return this.maxAmount;
		}
	}
}
