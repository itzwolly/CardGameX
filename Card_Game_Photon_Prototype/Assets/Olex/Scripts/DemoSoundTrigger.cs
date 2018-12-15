using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSoundTrigger : MonoBehaviour {
    public int _dummyID;
	void Start () {
		
	}
	
	//Demo: pressing Q, W, or E will trigger a sound coming from a respective direction
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q) && _dummyID==1)
            SoundManager.instance.PlaySound(SoundManager.instance.m_Damage_EventPath, 50.0f, 10.0f, 0f, transform.position);
        if (Input.GetKeyDown(KeyCode.W) && _dummyID == 2)
            SoundManager.instance.PlaySound(SoundManager.instance.m_Damage_EventPath, 50.0f, 10.0f, 0f, transform.position);
        if (Input.GetKeyDown(KeyCode.E) && _dummyID == 3)
            SoundManager.instance.PlaySound(SoundManager.instance.m_Damage_EventPath, 50.0f, 10.0f, 0f, transform.position);
    }

    //The generic request for an object to play a sound looks like this:
    //SoundManager.instance.PlaySound(SoundManager.instance.m_SOUND EFFECT EVENT NAME_EventPath, REVERB (0f-100f), DAMAGE (0f-10f) (obv keep zero if it's not combat related), FIRE INTENSITY (0f-100f), transform.position);
    //The list of the sound effect event names is found in the Sound Manager.
    //Music and Fire for now are not functional.
}
