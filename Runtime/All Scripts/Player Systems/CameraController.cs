using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float maxCameraVerticalViewAngle;
    [SerializeField] private bool hideCursor;
    private Vector2 input;

    [Header("Camera Shake")]
    [SerializeField] private float cameraShakeMultiplier;
    [SerializeField] private float crossbowFireShakeMultiplier;
    
    
    [Header("FOV Settings")]
    [SerializeField] private float sprintFOV;
    [SerializeField] private float defaultFOV;
    [SerializeField] private float fovSmoothing;

    [Header("Movement Sway")]
    [SerializeField] private float walkSwayAmplitude;
    [SerializeField] private float sprintSwayAmplitude;
    [SerializeField] private float walkSwayFrequency;
    [SerializeField] private float sprintSwayFrequency;
    [SerializeField] private float swayNoise;
    [SerializeField] private float lissajSmooth;
    private float ratio = 0.5f;
    private Vector3 lissajVelocity;



    private float currentFOV = 65f;
    private float desiredFOV = 65f;
    private float fovVelocity;
    private float cameraRotationX;
    private bool isSprinting;

    private float currentMovementSway;
    [SerializeField] private Vector3 cameraDefaultPosition;
    
    //References
    private Camera mainCamera;
    private PlayerMovementController playerMovementController;



    private void Awake()
    {
        // Lock the cursor
        if (hideCursor)
            Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        playerMovementController = GetComponent<PlayerMovementController>();
        cameraDefaultPosition = mainCamera.transform.localPosition;
    }
    

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleRotations();
        HandleFovFx();
        HandleCameraMovementSway();
        
        // TODO: TEMP TEMP TEMP TEMP TEMP TEMP TEMP
        
        // Hide the cursor if hideCursor checked
        // Cursor.lockState = hideCursor ? CursorLockMode.Locked : CursorLockMode.None;

        
    }

    private void HandleRotations()
    {
        // Remember camera up down rotation is changed via X-axis
        cameraRotationX -= input.y; // Subtract movement of mouse y from camera rotation, this is up down movement
        cameraRotationX = Mathf.Clamp(cameraRotationX, -maxCameraVerticalViewAngle, maxCameraVerticalViewAngle);

        
        // Perform rotations
        mainCamera.transform.localRotation = Quaternion.Euler(cameraRotationX, 0, 0); // Perform Up down mouse look
        transform.Rotate(Vector3.up * input.x); // Remember this script is attached to player object, mouse x just rotates player
    }

    private void HandleInput()
    {
        // TODO: Move this to input manager

        input.x = InputManager.Instance.mouseX;
        input.y = InputManager.Instance.mouseY;
    }

    private void HandleFovFx()
    {
        desiredFOV = playerMovementController.IsSprinting ? sprintFOV : defaultFOV;
        currentFOV = Mathf.SmoothDamp(currentFOV, desiredFOV, ref fovVelocity, fovSmoothing);
        mainCamera.fieldOfView = currentFOV;
    }

    private void HandleCameraMovementSway()
    {
        // If the player is moving then calculate a lissaj and apply it with smooth damp function
        // If the player isn't moving then damp back to cameras efault position

        float amp = playerMovementController.IsSprinting ? sprintSwayAmplitude : walkSwayAmplitude;
        float frq = playerMovementController.IsSprinting ? sprintSwayFrequency : walkSwayFrequency;

        var x = amp * Mathf.Sin((frq * ratio) * Time.timeSinceLevelLoad); // <Might need to offset
        var y = amp * Mathf.Sin(frq * Time.timeSinceLevelLoad);

        if (playerMovementController.Velocity.magnitude > 0)
        {
            Vector3 lissaj = new Vector3(x, y, 0);
            lissaj.x += Random.Range(-swayNoise, swayNoise);
            lissaj.y += Random.Range(-swayNoise, swayNoise);
            mainCamera.transform.localPosition = Vector3.SmoothDamp(mainCamera.transform.localPosition, cameraDefaultPosition + lissaj,
                ref lissajVelocity, lissajSmooth);
        }
        else
        {
            // We are not moving so smooth damp camera position back to default
            mainCamera.transform.localPosition = Vector3.SmoothDamp(mainCamera.transform.localPosition, cameraDefaultPosition,
                ref lissajVelocity, lissajSmooth);
        }
    }

    private void CrossbowFireShake()
    {
        DamageShake(crossbowFireShakeMultiplier);
    }

    public void DamageShake(float multiplier)
    {
        // Set duration and magnutude values based off multiplier, which is based off damage
        float duration = (multiplier / 25) * cameraShakeMultiplier;
        float magnitude = (multiplier / 50) * cameraShakeMultiplier;

        StartCoroutine(Shake(duration, magnitude));

    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            // TODO: Lower the magnitude each itteration

            float magMultiplier = Mathf.InverseLerp(duration, 0, elapsed);
            
            float x = Random.Range(-1f, 1f) * magnitude * magMultiplier;
            float y = Random.Range(-1f, 1f) * magnitude * magMultiplier;

            Camera.main.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;

    }

    private void OnEnable()
    {
        Crossbow.OnCrossbowFire += CrossbowFireShake;
    }

    private void OnDisable()
    {
        Crossbow.OnCrossbowFire -= CrossbowFireShake;
    }
}
