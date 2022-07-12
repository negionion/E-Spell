using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTS_AndroidLocal : MonoBehaviour
{
    public TextToSpeech tts;
    public void ttsPlayAudio(string text)
    {
        tts.Speak(text);
    }

    public void ttsSetLang(TextToSpeech.Locale langID)
    {
        tts.SetLanguage(langID);
    }
}
