using UnityEngine;
using System.Collections;

[System.Serializable]
public class ResourceSave : BaseSave
{

	private double valueSmallTotal;

	public ResourceSave (string name, double valueSmallCurrent, double valueSmallTotal) : base (name, valueSmallCurrent)
	{
		this.valueSmallTotal = valueSmallTotal;
	}

	public double ValueSmallTotal {
		get {
			return this.valueSmallTotal;
		}
	}
	
}
