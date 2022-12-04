using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Private Variables
    private Rigidbody playerRb;
    private float topBound = 18.0f;
    private float landingScore;
    private GameManager gameManager;
    private Color defaultFuelColor;
    private Vector3 offset = Vector3.zero;




    //Public Variables
    public float liftForce;
    public float downForce;
    public float speedBrake;
    public float currentTime;
    public float fuelBurn = 15.0f;
    public bool fuelAvailable;
    public bool engineStartNeeded;
    public bool engineCanDie;
    public bool applyingThrottle;
    public bool applyingBrake;
    public bool onRunway;
    public bool alreadyPlayed = false;
    public bool isGameOver;
    public bool stallingOut;
    public bool uncontrolledAircraft;
    public bool offScreen;
    public bool goodLanding;
    public AudioSource engineNoise;
    public AudioSource landingFriction;
    public AudioSource engineSputter;
    public TextMeshProUGUI fuelText;
    public Image fuelImage;
    public ParticleSystem deathExplosion;
    public ParticleSystem highscoreEffect;
    public ParticleSystem dirtCloud;
    public ParticleSystem rightContrail;
    public ParticleSystem leftContrail;
    public GameObject explosionPlaceholder;
    public GameObject confettiPlaceholder;

    // iOS variables
    public Slider joystick;
    private FlareButton flareButton;
    private CustomJoystick joystickScript;





    // Start is called before the first frame update
    void Start()
    {
        flareButton = GameObject.Find("Flare Button").GetComponent<FlareButton>();
        joystickScript = GameObject.Find("Joystick").GetComponent<CustomJoystick>();
        isGameOver = false;
        fuelAvailable = true;
        engineStartNeeded = true;
        fuelImage.fillAmount = 1;
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        defaultFuelColor = fuelImage.color;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        engineNoise.volume = 0.8f;
        confettiPlaceholder.transform.position = new Vector3(0, 10, 0);



    }

    private void FixedUpdate()
    {

        // Controls the Movement
        if (applyingThrottle == true && fuelAvailable == true && stallingOut == false)
        {
            playerRb.AddForce(Vector3.up * liftForce, ForceMode.Force);
            RotateObject(Vector3.left * .60f);
        } else if (applyingThrottle == true && stallingOut == true || fuelAvailable == false)
        {
            RotateObject(Vector3.left * .60f);
        } 
        if (applyingBrake == true)
        {
            playerRb.AddForce(Vector3.down * speedBrake, ForceMode.Force);
            RotateObject(Vector3.right * 0.45f);
        }
        if(flareButton.flaringOut == true && applyingBrake == false)
        {
            playerRb.AddForce(Vector3.up * .45f, ForceMode.Force);
            RotateObject(Vector3.left * 0.30f);
        }
        if (offset.x < 0 && joystick.value == 1)
        {
            RotateObject(Vector3.right * 0.55f);
        }

        //Controls the stalling 
        if (stallingOut == true)
        {
            playerRb.AddForce(Vector3.down * 6.5f, ForceMode.Acceleration);
        }
        if (uncontrolledAircraft == true)
        {
            playerRb.AddForce(Vector3.down * 5.0f, ForceMode.Acceleration);
        }
        if (offScreen == true)
        {
            playerRb.AddForce(Vector3.down * downForce, ForceMode.VelocityChange);
        }

        
        ScoringFeature();
    }

    // Update is called once per frame
    void Update()
    {
        LogicUpdate();
        CheckForStall();
        StayOnScreen();
        EngineIdleSpeed();
        ParticleManager();
        UpdateFuel();
        EngineNoise();
        DuplicateScore();
        explosionPlaceholder.transform.position = transform.position;
        
   
       
    }

    private void ScoringFeature()
    {
        //This section deals with adding the points for landing and controlling the UI
        if (onRunway == true && offset.x <= 3 && offset.x >= -3)
        {
            goodLanding = true;
            Invoke("ApplyScoreCountdown", .1f);
            gameManager.earnedScore.gameObject.SetActive(true);

        }
        else if (onRunway == true && (offset.x > 3 || offset.x < -3))
        {
            goodLanding = false;
        }
        if (goodLanding == true && gameManager.earnedScore.fontSize < 30)
        {
            gameManager.earnedScore.fontSize += .02f;
        }

        if (onRunway == true && (offset.x < -15 || offset.x > 15))
        {
            CrashLanding();
        }
    }
   
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && onRunway == false)
        {
            CrashLanding();
        }
        if (collision.gameObject.CompareTag("Runway"))
        {
            engineNoise.volume -= .10f * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Runway") && playerRb.velocity.y >= -9)
        {
            onRunway = true;
        }
        else if (collision.gameObject.CompareTag("Tanker"))
        {
            if (gameObject.transform.position.y > collision.gameObject.transform.position.y)
            {
                CrashLanding();
            }
        }
        else
        {
            CrashLanding();
        }
       //This prevents the audio queue for landing for playing infinitely
        if (alreadyPlayed == false && onRunway == true)
        {
            landingFriction.Play();
            alreadyPlayed = true;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("JerryCan"))
        {
            fuelImage.fillAmount += .2f;
            gameManager.refuelNoise.Play();
            other.gameObject.SetActive(false);
        }
        else
        {
            CrashLanding();
        }
        

        
        
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Runway"))
        {
            onRunway = false;
            goodLanding = false;
            
        }
    }

    private void LogicUpdate()
    {
        if(joystickScript.touchingJoystick == false)
        {
            joystick.value = 1;
        }
        if (joystick.value == 1)
        {
            applyingThrottle = false;
            applyingBrake = false;
        }
        if (joystick.value < 0.8f && MenuManager.controlsAreInverted == false)
        {
            applyingBrake = false;
            applyingThrottle = true;
            gameManager.inputDetected = true;
            engineNoise.pitch += .2f * Time.deltaTime;
        }else if (joystick.value < 0.8f && MenuManager.controlsAreInverted == true)
        {
            applyingThrottle = false;
            applyingBrake = true;
            gameManager.inputDetected = true;
        }
        if (joystick.value > 1.2f && MenuManager.controlsAreInverted == false)
        {
            applyingThrottle = false;
            applyingBrake = true;
            gameManager.inputDetected = true;
        }else if (joystick.value > 1.2f && MenuManager.controlsAreInverted == true)
        {
            applyingBrake = false;
            applyingThrottle = true;
            gameManager.inputDetected = true;
            engineNoise.pitch += .2f * Time.deltaTime;
        }
        if (flareButton.flaringOut == true)
        {
            gameManager.inputDetected = true;
        }
    }
   

    // If the user is not applying throttle, the engine noise will whirl down
    void EngineIdleSpeed()
    {
        if (engineNoise.pitch > 1.3f && applyingThrottle == false)
        {
            engineNoise.pitch -= .15f * Time.deltaTime;
        }
        if (engineNoise.volume < .8f && onRunway == false)
        {
            engineNoise.volume += .2f * Time.deltaTime;
        }

    }




    void StayOnScreen()
    {
        //This simulates a windshear
        if (transform.position.y > topBound)
        {
            offScreen = true;
        }
        else
        {
            offScreen = false;
        }
    }


    void CheckForStall()
    {
        if (offset.x <= -65)
        {
            stallingOut = true;
        }
        else
        {
            stallingOut = false;
        }
        if (offset.x >= 35)
        {
            uncontrolledAircraft = true;
        }
        else
        {
            uncontrolledAircraft = false;
        }
        
    }
    

    void ParticleManager()
    {
        if (fuelAvailable == false)
        {
            rightContrail.Stop();
            leftContrail.Stop();
        }


        //This deals with each particle during the landing/takeoff phase
        if (onRunway == true)
        {
            dirtCloud.Play();
            rightContrail.Stop();
            leftContrail.Stop();

        }
        if (onRunway == false && fuelAvailable == true)
        {

            rightContrail.Play();
            leftContrail.Play();


            if (onRunway == false)
            {
                dirtCloud.Stop();
                alreadyPlayed = false;
                landingFriction.Stop();
            }
            if (gameManager.inputDetected == false)
            {
                leftContrail.Stop();
                rightContrail.Stop();
            }
        }
    }
    void ApplyScoreCountdown()
    {
            gameManager.AddScore(landingScore);
    }
    public void UpdateFuel()
    {
        // This deals with how fuel interacts in the game
        if (fuelImage.fillAmount > 0)
        {
            fuelAvailable = true;
            engineCanDie = true;
        }
        else
        {
            fuelAvailable = false;
            engineNoise.Stop();
            engineStartNeeded = true;
        }



        // This is what controls how much fuel you burn per minute & how quickly you refuel on the runway
        fuelText.text = ("Current Fuel: " + (fuelImage.fillAmount * 100).ToString("0") + " %");
        if (onRunway == false)
        {
            fuelImage.fillAmount -= 0.8f / fuelBurn * Time.deltaTime;
        }
        else
        {
            fuelImage.fillAmount += 4.5f / fuelBurn * Time.deltaTime;

        }
        // This controls the color of the fuel bar
        if (fuelImage.fillAmount > .6)
        {
            fuelImage.color = defaultFuelColor;
        }
        if (fuelImage.fillAmount <= .6)
        {
            fuelImage.color = new Color(250.0f, 255.0f, 0.0f);
        }
        if (fuelImage.fillAmount <= .3)
        {
            fuelImage.color = new Color(255.0f, 0.0f, 44.0f);
        }
    }
    void EngineNoise()
    {
        if (fuelAvailable == true && engineStartNeeded == true)
        {
            engineNoise.Play();
            engineStartNeeded = false;
        }
        else if (engineCanDie == true && fuelAvailable == false)
        {
            engineSputter.Play();
            engineCanDie = false;
        }
        
    }
    void RotateObject (Vector3 amount)
    {
        offset += amount;
        transform.Rotate(amount, Space.Self);
    }
    void DuplicateScore()
    {
        if (goodLanding == false)
        {
            currentTime = 1f;
            landingScore = .88f;
        }
        else
        {
            currentTime -= 1 * Time.deltaTime;
           // Debug.Log("Current Time: " + currentTime);
        }
        if (currentTime <= 0)
        {
            landingScore = .99f;
        }
    }

    void CrashLanding()
    {
        deathExplosion.Play();
        gameManager.deathSound.Play();
        gameObject.SetActive(false);
        isGameOver = true;
    }
}