using UnityEngine;
using System.Collections;

public class ResourceData : GenericResourceData
{
	private ResourceType resourceType;

	private BiggerNumber maxAmount;

	public ResourceData (ResourceType resourceType, BiggerNumber currentAmount) : base (currentAmount)
	{
		this.resourceType = resourceType;
		this.maxAmount = resourceType.BaseMaxValue;
	}


	public ResourceData (ResourceType resourceType) : base (0)
	{
		this.resourceType = resourceType;
		this.CurrentAmount = 0;
		this.maxAmount = resourceType.BaseMaxValue;
	}

	public ResourceType ResourceType {
		get {
			return this.resourceType;
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
