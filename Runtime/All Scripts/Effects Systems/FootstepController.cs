using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [SerializeField] private GameObject footstepObject; // Hold particle fx and sound for footstep
    [SerializeField] private Transform leftStepPoint;
    [SerializeField] private Transform rightStepPoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// Called via animation event - or in case of player - player movement script
    /// </summary>
    public void LeftFootstep()
    {
        Instantiate(footstepObject, leftStepPoint.position, Quaternion.identity);
    }
    
    /// <summary>
    /// Called via animation event
    /// </summary>
    public void RightFootstep()
    {
        Instantiate(footstepObject, rightStepPoint.position, Quaternion.identity);
    }
}
