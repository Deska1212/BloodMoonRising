using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{

    public static SceneManage instance;
    
    [Tooltip("Assign me to fade image object in canvas")]
    [SerializeField] private Image fadeImage;
    [Tooltip("The time it takes to fade in and load the next scene")]
    [SerializeField] private float fadeInTime;
    
    [Tooltip("Time we wait when we are fully faded to exit the scene")]
    [SerializeField] private float exitSceneWhenFullyFadedDelay;
    
    [Tooltip("The time it takes to fade out after loading scene")]
    [SerializeField] private float fadeOutTime;
    [Tooltip("Delay before we start fading out")]
    [SerializeField] private float fadeOutDelay;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        fadeImage.color = Color.black;
        OnSceneLoad();
    }


    public void LoadScene(int idx)
    {
        StartCoroutine(ExitLevelRoutine(idx));
    }
    
    private void OnSceneLoad()
    {
        Debug.Log("On scene load called");
        StartCoroutine(EnterLevelRoutine());
    }

    /// <summary>
    /// For transitioning out of scenes
    /// </summary>
    private IEnumerator ExitLevelRoutine(int idx)
    {
        // Exiting scene to fade in the image with a tween
        LeanTween.alpha(fadeImage.GetComponent<RectTransform>(), 1f, fadeInTime);
        
        // Wait until fade is done
        yield return new WaitForSeconds(fadeInTime + exitSceneWhenFullyFadedDelay);
        
        // Exit the level and load level index
        SceneManager.LoadScene(idx);
    }
    
    /// <summary>
    /// For transitioning into scenes
    /// </summary>
    private IEnumerator EnterLevelRoutine()
    {
        fadeImage.color = Color.black;
        
        // Entering scene to fade out image alpha with a tween
        LeanTween.alpha(fadeImage.GetComponent<RectTransform>(), 0f, fadeOutTime).setDelay(fadeOutDelay);
        
        yield return null;
    }
    





    public void ResetScene()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
