using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerAnimation : MonoBehaviour {
    bool yes;
    private Renderer rend;
    public Material[] mats;
    void Start () {
        rend = GetComponent<Renderer>();
        InvokeRepeating("Change", 0, 0.05f);
	}
	void Change () {
        yes = !yes;
        rend.material = mats[(yes ? 0 : 1)];
	}
}
