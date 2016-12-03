using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildingSave : BaseSave
{
	private double valueSmallBuilt;

	public BuildingSave (string name, double valueSmallCurrent, double valueSmallBuilt) : base (name, valueSmallCurrent)
	{
		this.valueSmallBuilt = valueSmallBuilt;
	}

	public double ValueSmallBuilt {
		get {
			return this.valueSmallBuilt;
		}
	}

}
