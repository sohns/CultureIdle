using UnityEngine;
using System.Collections;

public class TechnologyData {
	private TechnologyType technologyType;
	private CurrencyData[] cost;
	private bool isResearched;
	private bool isResearching;

	public TechnologyData (TechnologyType technologyType)
	{
		this.technologyType = technologyType;
		this.cost = technologyType.Costs;
		this.isResearched = false;
		this.isResearching = false;
	}
	public TechnologyType TechnologyType {
		get {
			return this.technologyType;
		}
	}

	public CurrencyData[] Cost {
		get {
			return this.cost;
		}
	}

	public bool IsResearched {
		set{
			this.isResearched = value;
		}
		get {
			return this.isResearched;
		}
	}

	public bool IsResearching {
		set{
			this.isResearching = value;
		}
		get {
			return this.isResearching;
		}
	}

	public bool IsAvailableToStartResearching{
		get {
			return (!this.IsResearching && !this.isResearched);
		}
	}

}
