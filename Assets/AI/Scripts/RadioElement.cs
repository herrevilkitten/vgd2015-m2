using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Core;
using RAIN.Serialization;

[RAINSerializableClass, RAINElement("Radio")]
public class RadioElement : CustomAIElement
{
	[RAINSerializableField(Visibility = FieldVisibility.Show, ToolTip = "")]
	private string
		sender = "";

	// the full set dialog that will be spoken between two NPCs (Assumes that conversation will go back and forth
	[RAINSerializableField(Visibility = FieldVisibility.Show, ToolTip="Navigation targets")]
	private List<GameObject>
		listOfTargets = new List<GameObject> ();

	public override void AIInit ()
	{
		base.AIInit ();
		//	InitializeTargets ();
		ListenTo ();	
	}
	/*
	private void InitializeTargets() {
		foreach (GameObject element in listOfTargets) {
			RadioManager.Singleton.AddTarget(element);
		}
	}
	*/

	public void ListenTo ()
	{
		RadioManager.Singleton.Listen (this);
	}

	public void ReceiveMessage (string sender, string variableName, object value)
	{
		Debug.Log ("Receive Message: " + sender + " " + variableName + " " + value);
		if (!this.sender.Equals (sender)) {
			AI.WorkingMemory.SetItem<object> (variableName, value);
		}
	}

	public void RadioMessage (string variableName, object value)
	{
		//TODO: Add timestamp
		RadioManager.Singleton.RadioMessage (sender, variableName, value);
	}

	public void RadioReduceDetection (AI cop)
	{
		//TODO: Add timestamp
		RadioManager.Singleton.RadioReduceDetection (cop);
	}

	public void RadioAddDetection (AI cop)
	{
		//TODO: Add timestamp
		RadioManager.Singleton.RadioAddDetection (cop);
	}

	public string RadioDispatcher (AI cop, Transform player, float currentTime)
	{
		float firstObservedTime = cop.WorkingMemory.GetItem <float> ("FirstObservedTime");

		//Debug.Log (cop.Body.name + ": firstObservedTime = " + firstObservedTime);
		//Debug.Log (cop.Body.name + ": currentTime - firstObservedTime = " + (currentTime - firstObservedTime));
		if ((firstObservedTime != 0) && ((currentTime - firstObservedTime) > 1.0f)) {
			if (HasPlayerNotMovedFromLastObjectDetection (cop, player)) {
				//		Debug.Log (cop.Body.name + "Player is linger enough to cause suspicion. Calling dispatcher");
				RadioManager.Singleton.RadioDispatcher (cop, player, currentTime);

				//		Debug.Log ("Suspicion Level = " + StateManager.GetSuspicion());
				if (StateManager.instance.GetSuspicion () >= StateManager.MAXIMUM_SUSPICION_LEVEL) {
					return "arrest";
				} else {
					return "observe";
				}

			}
		} 

		//Debug.Log (cop.Body.name + "did not radio dispatcher");

		return "observe";
	}

	private bool HasPlayerMovedFromLastObjectDetection (AI cop, Transform player)
	{
		return !HasPlayerMovedFromLastObjectDetection (cop, player);
	}
	
	private bool HasPlayerNotMovedFromLastObjectDetection (AI cop, Transform player)
	{
		Vector3 latestPosition = player.position;
		Vector3 lastPosition = cop.WorkingMemory.GetItem <Vector3> ("LastDetectedPosition");

		if (lastPosition != null) {

			Vector3 difference = latestPosition - lastPosition;
			
			bool xNear = ((-1) * 5.0f) <= difference.x && difference.x <= 5.0f;
			bool zNear = ((-1) * 5.0f) <= difference.z && difference.z <= 5.0f;
		
			Debug.Log ("xNear && zNear= " + (xNear && zNear));
			return xNear && zNear;
		}
		Debug.Log ("no last position found");
		return false;
	}
}


