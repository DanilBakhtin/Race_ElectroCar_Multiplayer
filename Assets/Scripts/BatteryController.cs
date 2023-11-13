using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using Photon.Pun;

public class BatteryController : MonoBehaviour
{
    [SerializeField] private float maxEnergy;
    [SerializeField] private float Energyonsumption = 1f;
    [SerializeField] private UI_BatteryController ui_BatteryController;
    private float currentEnergy;
    public ParticleSystem electrocity;

    public Light headLight;
    private bool isLight;
    [SerializeField] private float costLight;

    [SerializeField] private float jumpForce;
    private bool isGround;

    [SerializeField] private Generator generator;

    private PrometeoCarController CarController;
    private Rigidbody rb;

    private bool isLossEnergy;

    private PhotonView view;


    private Trap trap;

    private bool isPause;
    private bool isFinish;
    private Vector3 startPos;
    private UI_Controller ui_Controller;

    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.Owner.IsLocal)
        {
            Camera.main.GetComponent<CameraController>().car = gameObject.transform;

            if (FindObjectOfType<UI_BatteryController>())
                ui_BatteryController = FindObjectOfType<UI_BatteryController>();

            ui_Controller = FindObjectOfType<UI_Controller>();
        }

        startPos = transform.position;
        CarController = GetComponent<PrometeoCarController>();
        rb = GetComponent<Rigidbody>();

        actionOff();
        currentEnergy = maxEnergy;
        electrocity.Stop();

        LightHeadOff();

        isLossEnergy = false;


        isPause = false;
        CarController.isPause = isPause;
        isFinish = false;
    }

    void FixedUpdate()
    {
        if (view.IsMine && !isFinish)
        {
            int absoluteCarSpeed = Mathf.RoundToInt(Mathf.Abs(CarController.carSpeed));

            if (absoluteCarSpeed > 0 && !CarController.deceleratingCar && !generator.isCharge)
            {
                addEnergy(Time.deltaTime * Energyonsumption * -1f);
            }

            if (absoluteCarSpeed > 0 && CarController.deceleratingCar && !generator.isCharge)
            {
                addEnergy(Time.deltaTime * Energyonsumption / 5f);
            }

            if (isLight)
            {
                addEnergy(Time.deltaTime * costLight * -1f);
            }

            if (currentEnergy <= 0)
            {
                LightHeadOff();
                rb.isKinematic = true;
                isLossEnergy = true;

            }

            if (isLossEnergy && !generator.isCharge && currentEnergy > 0f)
            {
                rb.isKinematic = false;
                isLossEnergy = false;
            }

            ui_BatteryController.updateEnergyText(currentEnergy);
        }

    }
    private void Update()
    {

        if (view.IsMine && !isPause && !isFinish)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                activateTrap();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isLight)
                    LightHeadOff();
                else LightHeadOn();
            }

            if (Input.GetKeyDown(KeyCode.Q) && isGround)
            {
                jump();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                activateGenerator();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && view.IsMine && !isFinish)
        {
            isPause = !isPause;
            CarController.isPause = isPause;
        }

    }
    private void actionOn()
    {   
        if(ui_BatteryController)
            ui_BatteryController.actionOn();
    }
    private void actionOff()
    {
        if (ui_BatteryController)
            ui_BatteryController.actionOff();
    }
    private void updateIconBattery()
    {
        if (ui_BatteryController)
            if (currentEnergy >= 0)
                ui_BatteryController.updateIconBattery(currentEnergy, maxEnergy);
    }
    public void addEnergy(float energyInput)
    {
        currentEnergy += energyInput;

        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;

        if (currentEnergy < 0)
            currentEnergy = 0;

        updateIconBattery();
    }
    private void activateTrap()
    {
        if (trap != null)
        {
            if (currentEnergy - trap.costEnergy >= 0)
            {
                currentEnergy -= trap.costEnergy;
                trap.activate();
                actionOff();
                StartCoroutine(electrocityCoroutine());

            }
        }

    }
    public void setTrap(Trap input)
    {
        if (input != null)
        {
            trap = input;
            ui_BatteryController.setCostTrapText(trap.costEnergy);
            actionOn();
        }
        else
        {
            trap = null;
            actionOff();
        }
    }
    IEnumerator electrocityCoroutine()
    {
        electrocity.Play();

        yield return new WaitForSeconds(2);

        electrocity.Stop();
    }
    private void LightHeadOn()
    {
        headLight.enabled = true;
        isLight = true;
    }
    private void LightHeadOff()
    {
        headLight.enabled = false;
        isLight = false;
    }
    private void jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    private void activateGenerator()
    {

        if (!generator.isCharge)
        {
            rb.isKinematic = true;
            generator.startCharging(this);
        }
        else
        {
            rb.isKinematic = false;
            generator.stopCharging();
        }
    }

    public void goToCheckPoint()
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillZone"))
        {
            goToCheckPoint();
        }

        if (other.gameObject.CompareTag("Finish"))
        {
            isFinish = true;
            CarController.isPause = true;
            ui_Controller.finishMultyplayer(other.gameObject.GetComponent<FinishRace>().place);
        }
    }
}
