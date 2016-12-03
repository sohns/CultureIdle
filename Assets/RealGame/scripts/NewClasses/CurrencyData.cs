using System.Collections;
using UnityEngine;

public class CurrencyData : BasicCurrencyData
{
	private BiggerNumber baseAmount;
	private BiggerNumber growthAmount = new BiggerNumber (0);

	public CurrencyData (object currencyType, BiggerNumber baseAmount, BiggerNumber growthAmount) : base (currencyType)
	{
		this.baseAmount = baseAmount;
		this.growthAmount = growthAmount;
	}

	public CurrencyData (object currencyType, BiggerNumber baseAmount) : base (currencyType)
	{
		this.baseAmount = baseAmount;
	}

	public BiggerNumber BaseAmount {
		get {
			return this.baseAmount;
		}
	}

	public BiggerNumber GrowthAmount {
		get {
			return this.growthAmount;
		}
	}

	public void incrementCurrency ()
	{
		baseAmount = baseAmount.MultNumber (growthAmount);
	}

	public void incrementCurrency (BiggerNumber amountOfTimes)
	{
		BiggerNumber massGrowth = growthAmount.PowerNumber (amountOfTimes);
		baseAmount = baseAmount.MultNumber (massGrowth);
	}

}
