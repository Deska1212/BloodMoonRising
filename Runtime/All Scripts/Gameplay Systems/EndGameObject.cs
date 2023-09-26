using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameObject : MonoBehaviour, IInteractable
{
    [SerializeField] private int sceneToLoadIndex;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        
    }

    public void Deselect()
    {
        
    }

    public void Interact()
    {
        // Ensure the player has killed all the bosses in the level before being able to interact with end game object
        if (GameManager.instance.activeBosses != 0)
        {
            // Play dun-dun jingle - player is trying to leave without killing all the Ghouls
            AudioManager.instance.Play("NoPotion");
            return;
        }
        

        SceneManage.instance.LoadScene(sceneToLoadIndex);
    }
}
