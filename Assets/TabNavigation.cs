﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TabNavigation : MonoBehaviour
{
	EventSystem system;

	// Use this for initialization
	void Start ()
	{
		system = EventSystem.current;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Tab)) {
			Selectable next = system.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();

			Debug.Log ("Next selectable is " + next);
			if (next != null) {
				
				InputField inputfield = next.GetComponent<InputField> ();
				if (inputfield != null)
					inputfield.OnPointerClick (new PointerEventData (system));  //if it's an input field, also set the text caret
				
				system.SetSelectedGameObject (next.gameObject, new BaseEventData (system));
			}
			//else Debug.Log("next nagivation element not found");
			
		}	
	}
}
