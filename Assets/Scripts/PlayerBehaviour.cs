using UnityEngine;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour {

    public float playerSpeed = 4.0f;
    private float currentSpeed = 0.0f;
    private Vector3 lastMovement = new Vector3();
    public Transform laser;
    public float laserDistance = .2f;
    public float timeBetweenFires = .3f;
    private float timeTilNextFire = 0.0f;
    public List<KeyCode> shootButton;
    // What sound to play when we're shooting
    public AudioClip shootSound;
    // Reference to our AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenuBehaviour.isPaused)
        {

            Rotation();
            Movement();

            foreach (KeyCode element in shootButton)
            {
                if (Input.GetKey(element) && timeTilNextFire < 0)
                {
                    timeTilNextFire = timeBetweenFires;
                    ShootLaser();
                    break;
                }
            }

            timeTilNextFire -= Time.deltaTime;
        }
    }

    void Rotation()
    {
        Vector3 worldPosition = Input.mousePosition;
        worldPosition = Camera.main.ScreenToWorldPoint(worldPosition);

        float deltaX = this.transform.position.x - worldPosition.x;
        float deltaY = this.transform.position.y - worldPosition.y;

        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        this.transform.rotation = rot;
    }

    void Movement()
    {
        Vector3 movement = new Vector3();

        movement.x += Input.GetAxis("Horizontal");
        movement.y += Input.GetAxis("Vertical");

        movement.Normalize();

        if(movement.magnitude > 0)
        {
            currentSpeed = playerSpeed;
            this.transform.Translate(movement * Time.deltaTime * playerSpeed, Space.World);
            lastMovement = movement;
        }
        else
        {
            this.transform.Translate(movement * Time.deltaTime * playerSpeed, Space.World);
            currentSpeed *= .9f;
        }
    }

    void ShootLaser()
    {
        audioSource.PlayOneShot(shootSound);
        Vector3 laserPosition = this.transform.position;
        float rotationAngle = transform.localEulerAngles.z - 90;
        laserPosition.x += (Mathf.Cos((rotationAngle) * Mathf.Deg2Rad) * -laserDistance);
        laserPosition.y += (Mathf.Sin((rotationAngle) * Mathf.Deg2Rad) * -laserDistance);

        Instantiate(laser, laserPosition, this.transform.rotation);
    }
}
