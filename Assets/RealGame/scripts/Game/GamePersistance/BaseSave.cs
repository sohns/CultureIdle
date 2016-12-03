using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseSave
{
	private string name;
	private double valueSmallCurrent;

	public BaseSave (string name, double valueSmallCurrent)
	{
		this.name = name;
		this.valueSmallCurrent = valueSmallCurrent;
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public double ValueSmallCurrent {
		get {
			return this.valueSmallCurrent;
		}
	}
}
