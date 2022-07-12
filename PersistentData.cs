using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PersistentData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        copyDataToPersistent("mhcilab.db");
        copyDataToPersistent("vocabulary_voice.assetbundle");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void copyDataToPersistent(string fileName)
    {
        string dataPath = Application.persistentDataPath + "/" + fileName;
        if(!File.Exists(dataPath))
        {
            WWW fileLoder = new WWW(Application.streamingAssetsPath + "/" + fileName);
            while(!fileLoder.isDone){}
            //data = WWW.LoadFromCacheOrDownload(Application.streamingAssetsPath + "/mhcilab.db", 0);
            File.WriteAllBytes(dataPath, fileLoder.bytes);
        }
    }
}
