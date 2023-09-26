using UnityEngine;
using UnityEngine.SceneManagement;

public class Test_PlaySounds : MonoBehaviour
{
    public GameObject pot1;
    public bool potion1 = true;

    public GameObject pot2;
    public bool potion2 = false;

    public GameObject pot3;
    public bool potion3 = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && potion3 == true)
        {
            FindObjectOfType<AudioManager>().Play("Potion");

            pot3.gameObject.SetActive(false);
            potion3 = false;

        }


        if (Input.GetKeyDown(KeyCode.F) && potion2 == true)
        {
            FindObjectOfType<AudioManager>().Play("Potion");

            pot2.gameObject.SetActive(false);
            potion2 = false;
            potion3 = true;
        }

        if (Input.GetKeyDown(KeyCode.F) && potion1 == true) 
        {
            FindObjectOfType<AudioManager>().Play("Potion");

            pot1.gameObject.SetActive(false);
            potion1 = false;
            potion2 = true;

        }

    }
}