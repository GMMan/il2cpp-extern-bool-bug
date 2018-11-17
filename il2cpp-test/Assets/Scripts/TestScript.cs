using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {
    Text target;

	// Use this for initialization
	void Start () {
        target = GetComponent<Text>();
		if (target != null)
        {
            target.text = NativeImport.TestFunc().ToString();
        }
	}
}
