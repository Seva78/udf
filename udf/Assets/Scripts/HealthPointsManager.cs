﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPointsManager : MonoBehaviour
{
    public GameObject hpUi;
    public TextMeshPro hpText;
    private int _healthPoints = 100;
    private int _healthPointsDelta;
    private float BarlogX => transform.position.x;
    private float BarlogY => transform.position.y;
    private int _healthPointsCooldownTrigger;
    private bool barlogAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void BoostHeal()
    {
        _healthPointsDelta = 5;
        _healthPoints += _healthPointsDelta;
        hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
        HealthPointsSpawn(_healthPointsDelta, new Color32(0, 255, 0, 255));
    }

    public void CollisionDamage()
    {
        _healthPointsDelta += Random.Range(1,3);
        Damage(_healthPointsDelta);
    }
    public void ProjectileDamage()
    {
        _healthPointsDelta += Random.Range(1,2);
        Damage(_healthPointsDelta);
    }
    
    public void Damage(int _healthPointsDelta)
    {
        _healthPoints -= _healthPointsDelta;
        if (_healthPointsCooldownTrigger == 0) {
            _healthPointsCooldownTrigger = 1;
            StartCoroutine(HealthPointsRedCoroutine(_healthPointsDelta));
        }
        hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
    }
    
    private IEnumerator HealthPointsRedCoroutine(int healthPointsDelta)
    {
        yield return new WaitForSeconds(0.1f);
        HealthPointsSpawn(healthPointsDelta * -1, new Color32(255, 0, 0, 255));
        _healthPointsCooldownTrigger = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (BarlogY > 800) _healthPoints -= 1;
        if (_healthPoints <= 0 && barlogAlive)
        {
            barlogAlive = false;
            _healthPoints = 0;            
            GetComponent<Barlog>().Death();
        }
    }
    private void HealthPointsSpawn(int healthPointsValue, Color32 healthPointsColor)
    {
        var hpTextI = Instantiate(hpText, new Vector3(BarlogX + 40, BarlogY, 0), Quaternion.identity);
        hpTextI.GetComponent<HealthPointsPopup>().Balrog = gameObject;
        hpTextI.GetComponent<TextMeshPro>().text = healthPointsValue.ToString();
        hpTextI.GetComponent<TextMeshPro>().color = healthPointsColor;
        _healthPointsDelta = 0;
    }
}
