using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public GameObject cannonBall;
    public float force;

    private void Start()
    {
        actions.Add("fire", Fire);

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
        GameObject clone = Instantiate(cannonBall, transform.position, Quaternion.identity);
        Rigidbody rb = clone.AddComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
        rb.useGravity = false;
        Destroy(clone, 5);
    }
}
