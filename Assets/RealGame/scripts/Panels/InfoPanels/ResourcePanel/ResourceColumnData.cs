using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceColumnData
{

	private string resourceName;
	private BiggerNumber basePerSec = new BiggerNumber ();
	private BiggerNumber totalEarned = new BiggerNumber ();
	private BiggerNumber currentAmount = new BiggerNumber ();
	private BiggerNumber maxAmount = new BiggerNumber ();

	public ResourceColumnData (string resourceName, BiggerNumber basePerSec, BiggerNumber totalEarned, BiggerNumber currentAmount, BiggerNumber maxAmount)
	{
		this.resourceName = resourceName;
		this.basePerSec = basePerSec;
		this.totalEarned = totalEarned;
		this.currentAmount = currentAmount;
		this.maxAmount = maxAmount;
	}

	public string ResourceName {
		get {
			return this.resourceName;
		}
	}

	public BiggerNumber BasePerSec {
		get {
			return this.basePerSec;
		}
	}

	public BiggerNumber TotalEarned {
		get {
			return this.totalEarned;
		}
	}

	public BiggerNumber CurrentAmount {
		get {
			return this.currentAmount;
		}
	}

	public BiggerNumber MaxAmount {
		get {
			return this.maxAmount;
		}
	}
}
