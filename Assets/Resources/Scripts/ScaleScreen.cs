﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Change
{


    public class ScaleScreen : MonoBehaviour
    {

        [System.Serializable]
        private enum Infotype { water, gas, area, calories, weight };

        [Header("Value Display")]
        [SerializeField] private Infotype _infoType;
        [SerializeField] private float _countDuration = 1f;
        [SerializeField] private TMPro.TextMeshPro _valueText;
        [SerializeField] private TMPro.TextMeshPro _subText;
     

        [Header("Subtext")]
        [SerializeField] private string _waterString = "liters of water";
        [SerializeField] private string _gasString = "grams of CO<sub>2</sub>";
        [SerializeField] private string _areaString = "m<sup>2</sup> of acreage";
        [SerializeField] private string _caloriesString = "kcal";
        [SerializeField] private string _weightString = "kg";


        private float currentValue;
        private float newValue;
        private Coroutine _countCoroutine = null;

        private void Start()
        {
            Scale.onScaleEvent.AddListener(OnScaleEvent);

            // set subline text
            switch(_infoType){
                case Infotype.water:
                    _subText.text = _waterString;
                    break;
                case Infotype.gas:
                    _subText.text = _gasString;
                    break;
                case Infotype.area:
                    _subText.text = _areaString;
                    break;
                case Infotype.calories:
                    _subText.text = _caloriesString;
                    break;
            }
           

        }
        private void OnScaleEvent(Food.Emission emission)
        {
            currentValue = newValue;
            switch (_infoType)
            {
                case Infotype.water:
                    newValue = emission.waterLiters;
                    break;

                case Infotype.area:
                    newValue = emission.areaSqrMeters;
                    break;

                case Infotype.gas:
                    newValue = emission.gasGrams;
                    break;

                case Infotype.calories:
                    newValue = emission.kcal;
                    break;

                case Infotype.weight:
                    newValue = emission.weight;
                    break;

                default:
                    Debug.Log("Choose info type for the " + name);
                    break;
            }

            if (_countCoroutine != null)
            {
                StopCoroutine(_countCoroutine);
                _countCoroutine = null;
            }

            _countCoroutine = StartCoroutine(C_CountingValue( _countDuration));
        }


        IEnumerator C_CountingValue(float duration)
        {
           
            float t = 0f;
            float lerpT = 0f;
            float value;
            while (t < duration)
            {
                t += Time.deltaTime;
                lerpT = Mathf.SmoothStep(0, 1, t / duration);

                value = Mathf.Lerp(currentValue, newValue, lerpT);


                _valueText.text = String.Format("{0:#,0}", (int)value);

                if (_infoType == Infotype.weight)
                {
                    _valueText.text = String.Format("{0:0,000,0}", (int)value);
                }
                    

                yield return null;
            }
            yield return null;
        }
    }

}
