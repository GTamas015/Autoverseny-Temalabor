using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{

    public GameObject CP;
    public GameObject CheckPointHolder;

    public GameObject[] cars;
    public Transform[] CheckPointPosition;
    public GameObject[] CheckPointForEachCar;

    private int numberOfCars;
    private int numberofCheckpoints;

    public TextMeshProUGUI rankText;
    public static String rank;

    // Start is called before the first frame update
    void Start()
    {
        numberOfCars = cars.Length;
        numberofCheckpoints = CheckPointHolder.transform.childCount;

        setCheckPoints();
        setCarPosition();
    }

    void setCheckPoints()
    {
        CheckPointPosition = new Transform[numberofCheckpoints];
        for (int i = 0; i < numberofCheckpoints; i++)
        {
            CheckPointPosition[i] = CheckPointHolder.transform.GetChild(i).transform;
        }

        CheckPointForEachCar = new GameObject[numberOfCars];
        for (int i = 0; i < numberOfCars; i++)
        {
            CheckPointForEachCar[i] = Instantiate(CP, CheckPointPosition[0].position, CheckPointPosition[0].rotation);
            CheckPointForEachCar[i].name = "CP" + i;
            CheckPointForEachCar[i].layer = 6 + i;
        }
    }


    public void setCarPosition() 
    {
        for (int i = 0; i < numberOfCars; i++) {
            cars[i].GetComponent<CarCPManager>().carPosition = i + 1;
            cars[i].GetComponent<CarCPManager>().carNumber = i;
        }

        rankText.text = cars[0].GetComponent<CarCPManager>().carPosition.ToString();
    }

    public void CarCollerCollectedCP(int carNumber, int numberofCP) 
    {
        CheckPointForEachCar[carNumber].transform.position = CheckPointPosition[numberofCP].transform.position;
        CheckPointForEachCar[carNumber].transform.rotation = CheckPointPosition[numberofCP].transform.rotation;

        comparePositions(carNumber);
    }


    public void comparePositions(int carNumber)
    {
        if (cars[carNumber].GetComponent<CarCPManager>().carPosition > 1) 
        {
            GameObject currentCar = cars[carNumber];
            int currentCarPosition = currentCar.GetComponent<CarCPManager>().carPosition;
            int currentCarCP = currentCar.GetComponent<CarCPManager>().cpCrossed;

            GameObject carInFront = null;
            int carInFrontPos = 0;
            int carInfrontCp = 0;

            for (int i = 0; i < numberOfCars; i++) 
            {
                if (cars[i].GetComponent<CarCPManager>().carPosition == currentCarPosition - 1)
                {
                    carInFront = cars[i];
                    carInFrontPos = carInFront.GetComponent<CarCPManager>().carPosition;
                    carInfrontCp = carInFront.GetComponent<CarCPManager>().cpCrossed;
                    break;

                }
            }

            if (currentCarCP > carInfrontCp)
            {
                currentCar.GetComponent<CarCPManager>().carPosition = currentCarPosition - 1;
                carInFront.GetComponent<CarCPManager>().carPosition = carInFrontPos + 1;
            }
            rankText.text = cars[0].GetComponent<CarCPManager>().carPosition.ToString();
            rank = rankText.text;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
