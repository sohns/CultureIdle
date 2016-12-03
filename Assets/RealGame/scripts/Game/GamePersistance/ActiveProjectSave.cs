using UnityEngine;
using System.Collections;

[System.Serializable]
public class ActiveProjectSave : BaseSave
{

	private double valueSmallTime;

	public ActiveProjectSave (string name, double valueSmallCurrent, double valueSmallTime) : base (name, valueSmallCurrent)
	{
		this.valueSmallTime = valueSmallTime;
	}


	public double ValueSmallTime {
		get {
			return this.valueSmallTime;
		}
	}
}
