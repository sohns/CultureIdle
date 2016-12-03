using UnityEngine;
using System.Collections;

[System.Serializable]
public class TechnologySave : BaseSave
{

	private bool isResearched;
	private bool isResearching;

	public TechnologySave (string name, double valueSmallCurrent, bool isResearched, bool isResearching) : base (name, valueSmallCurrent)
	{
		this.isResearched = isResearched;
		this.isResearching = isResearching;
	}

	public bool IsResearched {
		get {
			return this.isResearched;
		}
	}

	public bool IsResearching {
		get {
			return this.isResearching;
		}
	}

}
