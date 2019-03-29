﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
using System.Linq;
using System;

public class VoiceRecognition : MonoBehaviour
{
    private KeywordRecognizer keywordRecognizer;
    public AudioSource reloadSource, fireSound;
    public GameObject particles;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public GameObject cannonBall, initialBall;
    public Transform CBP;
    public float force, shakeForce, shakeLength;
    Vector3 originalpos, startPos;
    //Rigidbody rb;
    bool reloadable = false, firedFirst = false;

    private void Start()
    {
        //rb = initialBall.GetComponent<Rigidbody>();
        actions.Add("fire", Fire);
        actions.Add("reload", ReloadTest);
        actions.Add("restart", Restart);
        actions.Add("pause", Pause);
        actions.Add("play", Unpause);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        originalpos = this.transform.position;
        startPos = this.transform.position;
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
            fireSound.Play();
            StartCoroutine("FireCall");
        }
    }

    public void Reload()
    {
        if (reloadable)
        {
            reloadSource.Play();
            GameObject clone = Instantiate(cannonBall, CBP.position, Quaternion.identity);
            particles.SetActive(false);
            initialBall = null;
            initialBall = clone;
            clone.transform.parent = transform;
            //rb = clone.GetComponent<Rigidbody>();
            reloadable = false;
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RyanScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void Pause ()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }

    public void ReloadTest()
    {
        if (reloadable)
        {
            reloadSource.Play();
            GameObject clone = Instantiate(cannonBall, CBP.position, Quaternion.identity);
            particles.SetActive(false);
            initialBall = null;
            initialBall = clone;
            clone.transform.parent = transform;
            //rb = clone.GetComponent<Rigidbody>();
            reloadable = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "EnemyCannonBall")
        {
            Destroy(other.gameObject);
            shakeLength = .4f;
            print("Woah");
        }
    }

    private void Update()
    {
        if (shakeLength >= 0)
        {
            this.transform.localPosition = originalpos + UnityEngine.Random.insideUnitSphere * shakeForce * (Time.deltaTime * 1.5f);
            shakeLength -= Time.deltaTime;
        }
        else if (shakeLength <= 0)
        {
            this.transform.position = startPos;
        }
    }

    IEnumerator FireCall()
    {
        yield return new WaitForSeconds(0.5f);
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
#endif 