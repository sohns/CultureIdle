using System.Collections;

public class SimpleCurrencyData : BasicCurrencyData
{
	private BiggerNumber baseAmount;

	public SimpleCurrencyData (object currencyType, BiggerNumber baseAmount) : base (currencyType)
	{
		this.baseAmount = baseAmount;
	}

	public SimpleCurrencyData (object currencyType) : base (currencyType)
	{
	}

	public BiggerNumber BaseAmount {
		get {
			return this.baseAmount;
		}
	}
}
