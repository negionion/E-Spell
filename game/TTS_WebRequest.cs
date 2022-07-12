using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TTS_WebRequest : MonoBehaviour
{

    public IEnumerator ttsGoogleTranslate(string text, System.Action<AudioClip> speechAction, System.Action<string> logAction = null)
    {
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&tl=en&q=" + text + "&client=tw-ob";        
        WWW www = new WWW(url);     
        yield return www;
        if(logAction == null)
            Debug.Log("tts-error:" + www.error);
        else
            logAction(www.error);
        speechAction(www.GetAudioClip(false, false, AudioType.MPEG));
    }
    
}
