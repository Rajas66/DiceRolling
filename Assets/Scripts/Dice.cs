using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    float indexFloatVal = 0;

    [SerializeField]
    float calcSpeed = 1;

    public float speed = 2;

    public float minSpeed = 30, maxSpeed = 40;

    public float rotationSpeedMultiplier = 1;

    public float toHighPowerSpeed = 10;

    float timerToReduce = 0;
    public float timeAfterReducingSpeed = 1.75f;

    float highForceFactor = 0;
    public float highForceFactorSpeed = 1.25f;

    public float reduceFactorSpeed = 1.25f;

    float calcFactor = 0;

    int resultIndex = -1;
    public int resultNum = -1;

    public bool resultNumberReceived = false;

    [SerializeField]
    int index = 0;
    int oldIndex = -1;

    [SerializeField]
    int rollStartFlag = 0;

    [SerializeField]
    Transform diceTrans;

    [SerializeField]
    Vector3 rotateVector = Vector3.forward;

    [SerializeField]
    AnimationCurve reduceCurve;

    [SerializeField]
    float angle = 5;
    int i = 0, faceCount = 6;
    [SerializeField]
    SpriteRenderer crntSpriteRend;
    public List<Sprite> diceFaces = new List<Sprite>();    
    void Awake()
    {
        resultIndex = Random.Range(0, faceCount);
        crntSpriteRend.sprite = diceFaces[resultIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if(rollStartFlag == 1)
        {
            highForceFactor += Time.deltaTime * toHighPowerSpeed;
            highForceFactor = Mathf.Clamp(highForceFactor, 0, 1);

            if (highForceFactor >= 1)
            {
                timerToReduce = 0;
                rollStartFlag = 2;
            }
            calcFactor = highForceFactor;
        }
        else
        if(rollStartFlag == 2) 
        {
            timerToReduce += Time.deltaTime;

            if (timerToReduce > timeAfterReducingSpeed)
            {
                rollStartFlag = 3;
            }
        }
        else
        if (rollStartFlag == 3)
        {
            highForceFactor -= Time.deltaTime * reduceFactorSpeed;
            highForceFactor = Mathf.Clamp(highForceFactor, 0, 1);

            if (highForceFactor < .1f)
            {
                highForceFactor = 0;
                CalculateAndRotateToSnap();
                calcFactor = 0;
            }

            calcFactor = reduceCurve.Evaluate(highForceFactor);            
        }

        if (rollStartFlag != 0)
            RollingTheDice(calcFactor * speed);

        // Had put this condition at last for RollingTheDice function to Complete
        if (calcFactor <= 0 && rollStartFlag == 3)
        {
            //Debug.Log("Calc Angle:  " + Vector3.Angle(Vector3.right, this.transform.right));
            //CalculateAndRotateToSnap();

            SetResultNumber();

            rollStartFlag = 0;
            highForceFactor = 0;
        }
    }
    void CalculateAndRotateToSnap()
    {
        resultIndex = Random.Range(0, faceCount);
        crntSpriteRend.sprite = diceFaces[resultIndex];

        float tmpAngle = 0, minAngle = 0, targetAngle = 0;
        float angle = Vector3.SignedAngle(this.transform.right, Vector3.right, Vector3.forward);
        
        angle = Vector3.SignedAngle(this.transform.right, Vector3.right, Vector3.forward);

        minAngle = angle;

        angle = Vector3.SignedAngle(this.transform.right, Vector3.down, Vector3.forward);

        if(minAngle > angle && angle > 0)
            minAngle = angle;

        angle = Vector3.SignedAngle(this.transform.right, Vector3.left, Vector3.forward);

        if (minAngle > angle && angle > 0)
            minAngle = angle;

        angle = Vector3.SignedAngle(this.transform.right, Vector3.up, Vector3.forward);

        if (minAngle > angle && angle > 0)
            minAngle = angle;

        this.transform.Rotate(0, 0, minAngle);
    }
    void SetResultNumber()
    {
        resultNum = resultIndex + 1;

        resultNumberReceived = true;
    }
    void RollingTheDice(float crntSpeed)
    {
        indexFloatVal += Time.deltaTime * crntSpeed;

        indexFloatVal = (float)indexFloatVal % faceCount;

        index = (int)indexFloatVal;

        diceTrans.Rotate(rotateVector, angle * crntSpeed * rotationSpeedMultiplier * Time.deltaTime);

        if (index != oldIndex)
        {
            resultIndex = Random.Range(0, faceCount);
            crntSpriteRend.sprite = diceFaces[resultIndex];
        }
        oldIndex = index;
    }
    public bool IsItRolling()
    {
        return (rollStartFlag != 0);
    }
    public void StartRollingDice()
    {
        resultNumberReceived = false;

        rollStartFlag = 1;
        highForceFactor = 0;

        speed = Random.Range(minSpeed, maxSpeed);
    }
    private void OnEnable()
    {
        highForceFactor = 0;
    }
}