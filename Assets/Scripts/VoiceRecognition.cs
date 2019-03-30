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
        /*actions.Add("fire", Fire);
        actions.Add("reload", ReloadTest);
        actions.Add("restart", Restart);
        actions.Add("pause", Pause);
        actions.Add("play", Unpause);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();*/

        originalpos = this.transform.position;
        startPos = this.transform.position;
    }

    /*private void RecognizedSpeech (PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }*/

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
        MicLoudness = LevelMax();
        testSound = MicLoudness;

        if(testSound > .5f && !reloadable)
        {
            Fire();
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

    public float testSound;
    public static float MicLoudness;
    private string _device;
    private AudioClip _clipRecord;
    private int _sampleWindow = 128;
    private bool _isInitialized;

    void InitMic()
    {
        if (_device == null)
        {
            _device = Microphone.devices[0];
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
            Debug.Log(_clipRecord);
        }
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1);
        if (micPosition < 0)
        {
            return 0;
        }
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestory()
    {
        StopMicrophone();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (!_isInitialized)
            {
                InitMic();
                _isInitialized = true;
            }
        }

        if (!focus)
        {
            StopMicrophone();
            _isInitialized = false;
        }
    }

}
#endif