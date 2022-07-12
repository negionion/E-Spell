using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTS_GoogleTest : MonoBehaviour
{
    public TTS_WebRequest tts;
    public Text ttsLog;
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        ttsLog.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ttsTest(string str)
    {
        StartCoroutine(tts.ttsGoogleTranslate(str,
        (voice) => {
            audio.clip = voice;
            audio.Play();
        },
        (log) => 
        {
            ttsLog.text += log + "\n";
            ttsLog.text += "\nEND\n";
        }));
    }
}
