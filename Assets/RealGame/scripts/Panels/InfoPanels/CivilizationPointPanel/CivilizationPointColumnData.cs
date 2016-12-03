using UnityEngine;
using System.Collections;

public class CivilizationPointColumnData
{

	private string name;
	private BiggerNumber level = new BiggerNumber ();
	private BiggerNumber basePerSec = new BiggerNumber ();
	private BiggerNumber totalEarned = new BiggerNumber ();
	private BiggerNumber currentAmount = new BiggerNumber ();
	private BiggerNumber maxAmount = new BiggerNumber ();
	private CivilizationPointType type;

	public CivilizationPointColumnData (string name, BiggerNumber level, BiggerNumber basePerSec, BiggerNumber totalEarned, BiggerNumber currentAmount, BiggerNumber maxAmount, CivilizationPointType type)
	{
		this.name = name;
		this.level = level;
		this.basePerSec = basePerSec;
		this.totalEarned = totalEarned;
		this.currentAmount = currentAmount;
		this.maxAmount = maxAmount;
		this.type = type;
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public BiggerNumber Level {
		get {
			return this.level;
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

	public CivilizationPointType Type {
		get {
			return this.type;
		}
	}
}
