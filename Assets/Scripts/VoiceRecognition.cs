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
    public GameObject cannonBall, initialBall;
    public Transform CBP;
    public float force;
    Rigidbody rb;
    bool reloadable = false, firedFirst = false;

    private void Start()
    {
        rb = initialBall.GetComponent<Rigidbody>();
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
        if (!reloadable)
        {
            rb.AddForce(transform.forward * force);
            rb.useGravity = false;
            if (!firedFirst)
            {
                Destroy(initialBall, 5);
                firedFirst = true;
            }
            else
                Destroy(cannonBall, 5);
            reloadable = true;
        }
    }

    public void Reload()
    {
        if (reloadable)
        {
            GameObject clone = Instantiate(cannonBall, CBP.position, Quaternion.identity);
            cannonBall = clone;
            clone.transform.parent = transform;
            rb = clone.GetComponent<Rigidbody>();
            reloadable = false;
        }
    }
}
