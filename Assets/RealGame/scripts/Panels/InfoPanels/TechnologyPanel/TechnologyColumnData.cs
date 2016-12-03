using UnityEngine;
using System.Collections;

public class TechnologyColumnData {

	private string name;
	private string costText;
	private bool canBuy;
	private bool isResearching;
	private bool isResearched;
	private TechnologyType technologyType;
	public TechnologyColumnData (string name, string costText, bool canBuy, bool isResearching, bool isResearched, TechnologyType technologyType)
	{
		this.name = name;
		this.costText = costText;
		this.canBuy = canBuy;
		this.isResearching = isResearching;
		this.isResearched = isResearched;
		this.technologyType = technologyType;
	}
	public string Name {
		get {
			return this.name;
		}
	}

	public string CostText {
		get {
			return this.costText;
		}
	}

	public bool CanBuy {
		get {
			return this.canBuy;
		}
	}

	public bool IsResearching {
		get {
			return this.isResearching;
		}
	}

	public bool IsResearched {
		get {
			return this.isResearched;
		}
	}

	public TechnologyType TechnologyType {
		get {
			return this.technologyType;
		}
	}
}
