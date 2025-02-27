﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ButtonHighlight : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GameObject selection = transform.FindChild ("SelectionHighlight").gameObject;
		EventSystem eventSystem = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();

		Button button = GetComponent<Button> ();
		button.onClick.AddListener (() => {
			if (selection != null) {
				foreach (Image image in selection.GetComponentsInChildren<Image>()) {
					image.enabled = false;
				}
			}
			eventSystem.SetSelectedGameObject (null);
		});
		EventTrigger trigger = GetComponent<EventTrigger> ();

		// http://answers.unity3d.com/questions/854251/how-do-you-add-an-ui-eventtrigger-by-script.html
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.UpdateSelected;
		entry.callback.AddListener ((eventData) => {
			Debug.Log ("Settings button selected");
			if (selection != null) {
				foreach (Image image in selection.GetComponentsInChildren<Image>()) {
					image.enabled = true;
				}
			}
		});
		trigger.triggers.Add (entry);
		
		entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.Deselect;
		entry.callback.AddListener ((eventData) => {
			Debug.Log ("Settings button deselected");
			if (selection != null) {
				foreach (Image image in selection.GetComponentsInChildren<Image>()) {
					image.enabled = false;
				}
			}
		});
		trigger.triggers.Add (entry);
		
		entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener ((eventData) => {
			eventSystem.SetSelectedGameObject (gameObject);
		});
		trigger.triggers.Add (entry);

	}
}
