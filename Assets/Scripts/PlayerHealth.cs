using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider sl;
    public float currentHealth, maxHealth, healthRegen;
    float regenDelay;
    public Animator anim, anim2;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sl.value = currentHealth / maxHealth;

        if (regenDelay > 0)
            regenDelay -= Time.deltaTime;

        if (currentHealth < maxHealth && regenDelay <= 0)
        {
            currentHealth += healthRegen * Time.deltaTime;
        }
        
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (currentHealth <= 0)
        {
            anim.SetBool("Fade", true);
            text.text = "Alas, we could only hold on for so long";
            anim2.SetBool("Fade", true);
            StartCoroutine(ReloadGame());
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "EnemyCannonBall")
        {
            currentHealth -= 15;
            regenDelay = 2;
            Destroy(other.gameObject);
        }
    }

    IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(5);
        Time.timeScale = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("RyanScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
