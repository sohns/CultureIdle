using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if !UNITY_WEBGL
using System.Threading;
#endif
public class IncrementalManager : MonoBehaviour
{

	private List<Incremental> toIncrement = new List<Incremental> ();
	private float rate = .1f;
	private static bool isPaused = false;

	#if !UNITY_WEBGL
	private Thread gameThread;
	#endif
	public List<Incremental> ToIncrement {
		get {
			return this.toIncrement;
		}
	}

	public static bool IsPaused {
		get {
			return isPaused;
		}
		set {
			isPaused = value;
		}
	}

	void Start ()
	{
		StartCoroutine (AutoTick ());
	}

	void Update ()
	{
	}

	IEnumerator AutoTick ()
	{
		#if !UNITY_WEBGL
		Debug.LogWarning ("Not WEBGL");
		#endif
		#if UNITY_WEBGL
		Debug.LogWarning("WebGL");
		#endif
		while (true) {
			while (isPaused) {
			}
			#if !UNITY_WEBGL
			GameAdvancement gameAdvancement = new GameAdvancement (toIncrement, rate);
			bool isSlow = false;
			while (gameThread != null && gameThread.IsAlive) {
				isSlow = true;
			}
			if (isSlow) {
				Debug.LogWarning ("Thread took over the rate in time. CurrentRate:" + rate);
				rate = rate * 2;
			}
			gameThread = new Thread (new ThreadStart (gameAdvancement.runGameAdvancement));
			gameThread.Start ();
			#endif
			#if UNITY_WEBGL
			IncrementIncrementals ();
			#endif
			yield return new WaitForSeconds (rate);
		}
	}

	private void IncrementIncrementals ()
	{
		//For testing slow threads!
		//System.Threading.Thread.Sleep(1000);
		if (toIncrement.Count != 0) {
			foreach (Incremental incremental in toIncrement) {
				incremental.OnUpdate (rate);
			}
		}
	}
}
#if !UNITY_WEBGL
public class GameAdvancement
{
	private List<Incremental> toIncrement;
	private float rate;

	public GameAdvancement (List<Incremental> toIncrement, float rate)
	{
		this.toIncrement = toIncrement;
		this.rate = rate;
	}

	public void runGameAdvancement ()
	{
		IncrementIncrementals ();
	}

	private void IncrementIncrementals ()
	{
		//For testing slow threads!
		//System.Threading.Thread.Sleep(1000);
		if (toIncrement.Count != 0) {
			foreach (Incremental incremental in toIncrement) {
				incremental.OnUpdate (rate);
			}
		}
	}
}
#endif
