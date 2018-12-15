using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager instance = null;
    [FMODUnity.EventRef] public string m_ArmorBuff_EventPath;
    [FMODUnity.EventRef] public string m_ArmorDebuff_EventPath;
    [FMODUnity.EventRef] public string m_AttackBuff_EventPath;
    [FMODUnity.EventRef] public string m_AttackDebuff_EventPath;
    [FMODUnity.EventRef] public string m_BeginTurn_EventPath;
    [FMODUnity.EventRef] public string m_CardDiscard_EventPath;
    [FMODUnity.EventRef] public string m_CardDraw_EventPath;
    [FMODUnity.EventRef] public string m_CardPlay_EventPath;
    [FMODUnity.EventRef] public string m_CardSelect_EventPath;
    [FMODUnity.EventRef] public string m_Connected_EventPath;
    [FMODUnity.EventRef] public string m_Damage_EventPath;
    [FMODUnity.EventRef] public string m_Disconnected_EventPath;
    [FMODUnity.EventRef] public string m_EndTurn_EventPath;
    [FMODUnity.EventRef] public string m_Fire_EventPath;
    [FMODUnity.EventRef] public string m_Heal_EventPath;
    [FMODUnity.EventRef] public string m_Music_EventPath;
    [FMODUnity.EventRef] public string m_Shuffle_EventPath;
    [FMODUnity.EventRef] public string m_TurboDiscard_EventPath;
    [FMODUnity.EventRef] public string m_TurboPlay_EventPath;
    [FMODUnity.EventRef] public string m_TurboSelect_EventPath;
    [FMODUnity.EventRef] public string m_UIClick_EventPath;
    [FMODUnity.EventRef] public string m_UIForbidden_EventPath;
    [FMODUnity.EventRef] public string m_UINotification_EventPath;
    [FMODUnity.EventRef] public string m_UISelect_EventPath;
    public bool m_Debug;


    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetParameter(FMOD.Studio.EventInstance e, string name, float value)
    {
        FMOD.Studio.ParameterInstance parameter;
        e.getParameter(name, out parameter);
        if (parameter.Equals(null))
        {
            if (m_Debug)
                Debug.Log("Parameter named: " + name + " does not exist");
            return;
        }
        parameter.setValue(value);
    }

    public void PlaySound(string m_EventPath, float m_Spaciousness, float m_Damage, float m_FireIntensity, Vector3 m_Position)
    {
        if (m_EventPath != null)
        {
            FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_EventPath);
            e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(m_Position));

            SetParameter(e, "Spaciousness", m_Spaciousness);
            SetParameter(e, "Damage", m_Damage);
            SetParameter(e, "Fire Intensity", m_FireIntensity);

            e.start();
            e.release(); 
        }
    }
    //public void PlaySoundFire(gameObject SoundSource)
    //{
    //    if (m_Fire_EventPath != null)
    //    {
    //        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_Fire_EventPath);
    //        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(SoundSource.position));

    //        SetParameter(e, "Spaciousness", m_Spaciousness);
    //        SetParameter(e, "Damage", m_Damage);
    //        SetParameter(e, "Fire Intensity", m_FireIntensity);

    //        e.start();
    //        e.release(); //add fire specifics
    //    }
    //}

    //public void PlayMusic(gameObject SoundSource)
    //{
    //    if (m_Music_EventPath != null)
    //    {
    //        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(m_Music_EventPath);
    //        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(SoundSource.position));

    //        SetParameter(e, "Spaciousness", m_Spaciousness);
    //        SetParameter(e, "Damage", m_Damage);
    //        SetParameter(e, "Fire Intensity", m_FireIntensity);

    //        e.start();
    //        e.release(); //add music specifics
    //    }
    //} //figure out fire and music later


}
