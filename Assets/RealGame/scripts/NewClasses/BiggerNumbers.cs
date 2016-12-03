using System;

public class BiggerNumber: IComparable<BiggerNumber>
{
	public static implicit operator BiggerNumber (int value)
	{
		return new BiggerNumber (value);
	}

	public static implicit operator BiggerNumber (float value)
	{
		return new BiggerNumber (value);
	}

	public static implicit operator BiggerNumber (double value)
	{
		return new BiggerNumber (value);
	}

	public static readonly BiggerNumber ZERO = 0;
	public static readonly BiggerNumber ONE = 1;
	//TODO:Start off just using doubles. Let us see if that is enough!
	private double valueSmall = 0;
	private int exponent = 0;
	//Unused atm
	private Object thisLock = new Object ();
	//TODO:should be adjusted/set by the boss;

	public BiggerNumber ()
	{
	}

	public BiggerNumber (float value)
	{
		valueSmall = value;
		cleanUp ();
	}

	public BiggerNumber (double value)
	{
		valueSmall = value;
		cleanUp ();
	}

	public BiggerNumber (int value)
	{
		valueSmall = value;
		cleanUp ();
	}

	public BiggerNumber (BiggerNumber value)
	{
		this.valueSmall = value.valueSmall;
		this.exponent = value.exponent;
	}


	public BiggerNumber getMin (BiggerNumber other)
	{
		if (other.getValue () <= this.getValue ()) {
			return other;
		}
		return this;
	}

	public double getValue ()
	{
		return valueSmall;
	}

	public string ToDisplayString ()
	{
		return ToDisplayString (3, false);
	}

	public string ToDisplayString (int digitsToShow, bool isRounded)
	{
		if (valueSmall == 0) {
			if (isRounded) {
				return valueSmall.ToString ("F0");
			} else {
				return valueSmall.ToString ("f" + digitsToShow);
			}
		}
		int exponent;

		int groupOfThreeExponent;
		double valueReduced;

		exponent = (int)Math.Floor (Math.Log10 (Math.Abs (valueSmall)));
		groupOfThreeExponent = exponent - (exponent % 3);
		valueReduced = valueSmall / (Math.Pow (10, groupOfThreeExponent));
		string converted;
		if (groupOfThreeExponent == 0) {
			if (isRounded) {
				converted = valueSmall.ToString ("F0");
			} else {
				converted = valueSmall.ToString ("f" + digitsToShow);
			}
		} else {
			converted = valueReduced.ToString ("F" + digitsToShow) + "E" + groupOfThreeExponent;
		}
		return converted;
	}

	private void cleanUp ()
	{
		//while (valueSmall >= 1000)
		//{
		//    valueSmall = valueSmall / 1000;
		//    exponent += 3;
		//}
		//while (valueSmall <= 1 && exponent>=3)
		//{
		//    valueSmall = valueSmall * 1000;
		//    exponent -= 3;
		//}
	}

	public int CompareTo (BiggerNumber otherNumber)
	{
		return getValue ().CompareTo (otherNumber.getValue ());
	}

	public BiggerNumber AddNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (valueSmall + otherNumber.valueSmall);
		}
	}

	public BiggerNumber SubNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (valueSmall - otherNumber.valueSmall);
		}
	}

	public BiggerNumber MultNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (valueSmall * otherNumber.valueSmall);
		}
	}

	public BiggerNumber PowerNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (Math.Pow (valueSmall, otherNumber.valueSmall));
		}
	}

	public BiggerNumber DivideNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (valueSmall / otherNumber.valueSmall);
		}
	}

	public BiggerNumber InvertDivideNumber (BiggerNumber otherNumber)
	{
		lock (thisLock) {
			return new BiggerNumber (otherNumber.valueSmall / valueSmall);
		}
	}

	public BiggerNumber AbsoluteValue {
		get {
			lock (thisLock) {
				return new BiggerNumber (Math.Abs (valueSmall));
			}
		}
	}
      
}
