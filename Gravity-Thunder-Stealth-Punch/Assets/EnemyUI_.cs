using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class EnemyUI_ : MonoBehaviour
{
    [Header("Detection Sliders")]
    //references
    [SerializeField] Image searchMeter = null;
    [SerializeField] Image chaseMeter = null;

    [Header("Detection Values")]
    [Tooltip("This value controls enemy behaviour. Search Value = 100: AI searches for player. Chase Value = 100: AI will chase and attack player")]
    [SerializeField] [Range(0, 100)] public float searchMeterValue, chaseMeterValue;

    //Max values for each Detection Meter, do not change these, only used for comparions
    [HideInInspector] public float maxSearchMeterValue = 100, maxChaseMeterValue = 100;

    [Tooltip("Makes it so the UI always faces the camera. This prevents the UI from rotating weirdly.")]
    public Transform cameraToLookAt;

    void Update()
    {
        GetCurrentDetection();
        transform.LookAt(cameraToLookAt);
    }

    private void GetCurrentDetection()
    {
        //Search Meter Values
        float searchAmount = searchMeterValue / maxSearchMeterValue;
        searchMeter.fillAmount = searchAmount;

        //Chase Meter Values
        float chaseAmount = chaseMeterValue / maxChaseMeterValue;
        chaseMeter.fillAmount = chaseAmount;

        if (chaseMeterValue > maxChaseMeterValue)
            chaseMeterValue = maxChaseMeterValue; 
        
        if (searchMeterValue > maxSearchMeterValue)
            searchMeterValue = maxSearchMeterValue;
    }
}
