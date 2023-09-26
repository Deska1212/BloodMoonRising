using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Player exiting credits sequence early
            StopAllCoroutines();
            SceneManage.instance.LoadScene(1);
        }
    }

    public void EndCreditSequence()
    {
        SceneManager.LoadScene(1);
    }

}
