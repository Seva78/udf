using System.Collections;
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
    private bool _barlogAlive = true;

    public void BoostHeal()
    {
        _healthPointsDelta = 5;
        _healthPoints += _healthPointsDelta;
        hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints;
        HealthPointsSpawn(_healthPointsDelta, new Color32(0, 255, 0, 255));
    }

    public void CollisionDamage()
    {
        _healthPointsDelta += Random.Range(0,0);
        Damage(_healthPointsDelta);
    }
    public void ProjectileDamage()
    {
        _healthPointsDelta += Random.Range(0,0);
        Damage(_healthPointsDelta);
    }
    
    public void Damage(int _healthPointsDelta)
    {
        _healthPoints -= _healthPointsDelta;
        if (_healthPointsCooldownTrigger == 0) {
            _healthPointsCooldownTrigger = 1;
            StartCoroutine(HealthPointsRedCoroutine(_healthPointsDelta));
        }
        hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints;
    }
    
    private IEnumerator HealthPointsRedCoroutine(int healthPointsDelta)
    {
        yield return new WaitForSeconds(0.1f);
        HealthPointsSpawn(healthPointsDelta * -1, new Color32(255, 0, 0, 255));
        _healthPointsCooldownTrigger = 0;
    }
    private void Update()
    {
        if (BarlogY > 800) _healthPoints -= 1;
        if (_healthPoints <= 0 && _barlogAlive)
        {
            _barlogAlive = false;
            _healthPoints = 0;
            hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints;
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
