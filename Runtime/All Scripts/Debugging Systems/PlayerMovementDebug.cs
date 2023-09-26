using System;
using UnityEngine;
using TMPro;

/// <summary>
/// This whole class is yucky yucky bad bad and just for debug info while
/// implementing/balancing player movement controller
/// </summary>
public class PlayerMovementDebug : MonoBehaviour
{
    
    
    // Very tight reference to player movement controller
    [SerializeField] private PlayerMovementController controller;
    [SerializeField] private Player player;

    public TextMeshProUGUI speedUI;
    public TextMeshProUGUI groundedUI;
    public TextMeshProUGUI sprintingUI;
    public TextMeshProUGUI currGravUI;
    public TextMeshProUGUI staminaUI;
    public TextMeshProUGUI healthUI;

    private void Start()
    {
        var playerGameObject = GameObject.FindWithTag("Player");
        controller = playerGameObject.GetComponent<PlayerMovementController>();
        player = playerGameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        speedUI.text = "Speed: " + controller.playerSpeed.ToString("F1");
        currGravUI.text = "Gravity: " + controller.currGravity.ToString("F1");
        groundedUI.text = "Grounded?: " + controller.IsGrounded.ToString();
        sprintingUI.text = "Sprinting?: " + controller.IsSprinting.ToString();
        staminaUI.text = "Stamina: " + player.Stamina.ToString("F1") + "/100";
        healthUI.text = "Health: " + player.Health.ToString("F1") + "/100";
    }
}
