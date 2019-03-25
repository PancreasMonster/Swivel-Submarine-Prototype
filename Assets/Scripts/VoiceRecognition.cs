using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    public GameObject particles;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public GameObject cannonBall, initialBall;
    public Transform CBP;
    public float force;
    //Rigidbody rb;
    bool reloadable = false, firedFirst = false;

    private void Start()
    {
        //rb = initialBall.GetComponent<Rigidbody>();
        actions.Add("fire", Fire);
        actions.Add("reload", ReloadTest);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void RecognizedSpeech (PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    public void Fire ()
    {
        if (!reloadable)
        {
            GameObject CB = initialBall;
            particles.SetActive(true);
            if (CB != null)
            {
                CB.GetComponent<Rigidbody>().AddForce(transform.forward * force);
                CB.GetComponent<Rigidbody>().useGravity = false;
                CB.transform.tag = "CannonBall";
                CB.transform.parent = null;
                Destroy(CB, 5);
                reloadable = true;
            }
        }
    }

    public void Reload()
    {
        if (reloadable)
        {
            GameObject clone = Instantiate(cannonBall, CBP.position, Quaternion.identity);
            particles.SetActive(false);
            initialBall = null;
            initialBall = clone;
            clone.transform.parent = transform;
            //rb = clone.GetComponent<Rigidbody>();
            reloadable = false;
        }
    }

    public void ReloadTest()
    {
        if (reloadable)
        {
            GameObject clone = Instantiate(cannonBall, CBP.position, Quaternion.identity);
            particles.SetActive(false);
            initialBall = null;
            initialBall = clone;
            clone.transform.parent = transform;
            //rb = clone.GetComponent<Rigidbody>();
            reloadable = false;
        }
    }
}
#endif 