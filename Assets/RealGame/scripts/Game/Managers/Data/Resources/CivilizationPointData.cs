using UnityEngine;
using System.Collections;

public class CivilizationPointData : GenericResourceData
{
	private CivilizationPointType civlizationPointType;
	private BiggerNumber maxAmount;
	private BiggerNumber growthAmount;
	private BiggerNumber level;

	public CivilizationPointData (CivilizationPointType civlizationPointType, BiggerNumber currentAmount) : base (currentAmount)
	{
		this.civlizationPointType = civlizationPointType;
		this.maxAmount = civlizationPointType.BaseMaxValue;
		this.growthAmount = civlizationPointType.BaseGrowth;
		this.level = 0;
	}

	public CivilizationPointData (CivilizationPointType civlizationPointType) : base (0)
	{
		this.civlizationPointType = civlizationPointType;
		this.maxAmount = civlizationPointType.BaseMaxValue;
		this.growthAmount = civlizationPointType.BaseGrowth;
		this.level = 0;
	}

	public CivilizationPointType CivlizationPointType {
		get {
			return this.civlizationPointType;
		}
	}

	public override BiggerNumber CurrentAmount {
		get {
			return base.CurrentAmount;
		}
		set {
			base.CurrentAmount = value;
			stabalizeCivlizationPointData ();
		}
	}

	public BiggerNumber MaxAmount {
		get {
			return this.maxAmount;
		}
		set {
			maxAmount = value;
			stabalizeCivlizationPointData ();
		}
	}

	public BiggerNumber GrowthAmount {
		get {
			return this.growthAmount;
		}
		set {
			growthAmount = value;
		}
	}

	public BiggerNumber Level {
		get {
			return this.level;
		}
		set {
			level = value;
		}
	}

	private void stabalizeCivlizationPointData ()
	{
		while (CurrentAmount.CompareTo (maxAmount) >= 0) {
			CurrentAmount = CurrentAmount.SubNumber (maxAmount);
			level = level.AddNumber (1);
			maxAmount = maxAmount.MultNumber (growthAmount);
		}
		while (CurrentAmount.CompareTo (0) < 0) {
			if (level.CompareTo (0) == 0) {
				CurrentAmount = 0;
				break;
			}
			maxAmount = maxAmount.DivideNumber (growthAmount);
			level = level.SubNumber (1);
			CurrentAmount = CurrentAmount.AddNumber (maxAmount);
		}
	}
}
