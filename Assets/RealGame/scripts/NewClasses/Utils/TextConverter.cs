using UnityEngine;
using System.Collections;
using System;

public class TextConverter : MonoBehaviour
{
    private static TextConverter instance;
    public static TextConverter Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        CreateInstance();
    }

    void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public string GetAmountOfTimeToFinish(BiggerNumber currentAmount, BiggerNumber maxAmount, BiggerNumber income)
    {
		if (income.getValue () == 0) {
			return TextConverter.Instance.getTime ((24*3600*365)+1);
		}
        return TextConverter.Instance.getTime(maxAmount.SubNumber(currentAmount).DivideNumber(income).getValue());
    }
    public string getTime(double sec)
    {
		if (sec > (24 * 3600 * 365)) {
			return "Over a year";
		}
        var timeSpan = System.TimeSpan.FromSeconds(sec);
        int dd = timeSpan.Days;
        int hh = timeSpan.Hours;
        int mm = timeSpan.Minutes;
        int ss = timeSpan.Seconds;
        string toReturn = "";
        if (dd != 0)
        {
            toReturn += dd + "D ";
        }
        if (hh != 0)
        {
            toReturn += hh + "H ";
        }
        if (mm != 0)
        {
            toReturn += mm + "M ";
        }
        return toReturn + ss + "S";
    }
}
