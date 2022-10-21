using UnityEngine;
using System.Collections;

public class JSUIHelper
{
	// This is the version that Jump Start supports.  If it is lower than your version of NGUI, it may not function properly.
	// An update should be available to bring the version in line with NGUI.
	public static string GetNGUIVersion()
	{
		return "3.7.4";
	}
	
	// Get the scene's Preferences Controller.
	public static JSPreferencesController GetPreferencesControllerClass()
	{
		GameObject gco = GameObject.Find("JSPreferencesController");
		
		if(gco == null)
		{
			return null;
		}
		
		return gco.GetComponent<JSPreferencesController>();
	}

	public static JSAudioSequenceController GetAudioSequencerControllerClass()
	{
		return null;
	}

	// Spit out a log of an object's components (for testing prefabs when they're being converted to JS from NGUI and back).
	public static void JSUILogComponents(GameObject obj)
	{
		MonoBehaviour[] monos = obj.GetComponents<MonoBehaviour>();
		
		foreach(MonoBehaviour mono in monos)
		{
			Debug.Log("obj: " + obj.name + " component: " + mono.GetType().ToString());
		}
	}
	
	public static void BroadcastToAll(string function)
	{
		GameObject[] gameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		
		
		foreach(GameObject g in gameObjects)
		{
			if(g != null && g.transform.parent == null)
			{
				g.BroadcastMessage(function, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
