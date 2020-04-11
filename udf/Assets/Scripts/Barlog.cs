using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

public class Barlog : MonoBehaviour
{
    public float vertSpeed; //переменная для передачи в скрипт Mine
    public GameObject cam;
    public TextMeshPro hpText;
    public GameObject hpUi;
    public GameObject deepBoard;
    public GameObject reFallButton;
    public int startButtonPressed;
    public AudioClip soundBarlogHit2;
    float _v; //Скорость _v полёта демона.
    float _r; //Направление _r полёта демона.
    float _a; //Ускорение A, которое демон создаёт в период маха крыла.
    int _aTrigger;
    float _k; //Коэффициент _k трения об воздух (не константа, т.к. зависит от того, как сильно расправлены крылья).
    float _d; //переменная для вывода всякого в служебное меню
    int _ratio = 20;
    float _aVx;
    float _aVy;
    float _centerTendencyCoefficient;
    Animator _anim;
    int _healthPoints = 100;
    int _healthPointsDelta;
    int _healthPointsCooldownTrigger;
    float BarlogX => transform.position.x;
    float BarlogY => transform.position.y;
    Quaternion BarlogRot => transform.rotation;
    Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();
    
    void StartGame()
    {
        _anim = GetComponent<Animator>();
        startButtonPressed = 1;
    }

    void Update()
    {
        if (_healthPoints <= 0) {
            startButtonPressed = 0;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            deepBoard.SetActive(true);
            reFallButton.SetActive(true);
        }
        if (startButtonPressed == 1)
        {
            //если юзер не жмёт ни вправо, ни влево
            if (Input.GetAxis("Horizontal") == 0) _r *= 1 - _k * Time.deltaTime;  
            else _r += 3 * Input.GetAxis("Horizontal") * Mathf.PI / 180;

            if (Input.GetAxis("Vertical") > 0)
            {
                GetComponent<SpriteRenderer>().flipY = true;
                _k = 0.99f;
                _a = 0;
                _centerTendencyCoefficient = (BarlogY - (cam.transform.position.y + 200)) / 100;
            }
            else if (Input.GetAxis("Vertical") == 0)
            {
                GetComponent<SpriteRenderer>().flipY = false;
                _k = 0.5f;
                _a = 0;
                _centerTendencyCoefficient = (BarlogY - cam.transform.position.y) / 100;
            }
            else {
                GetComponent<SpriteRenderer>().flipY = false;
                _k = 0.4f;
                _centerTendencyCoefficient = (BarlogY - (cam.transform.position.y - _aTrigger * 200))/100;
                _a = _aTrigger * 20;
            }
            _v *= 1 - _k * Time.deltaTime;
            _v += _a * Time.deltaTime;
            _aVx = _v* Mathf.Sin(_r);
            _aVy = _v* Mathf.Cos(_r);
            _aVy += 8 * Time.deltaTime;
            vertSpeed = _aVy * _ratio * Time.deltaTime;
            if (vertSpeed < 3) vertSpeed = 3;
            Rigidbody.MovePosition(new Vector3(BarlogX + _aVx * _ratio * Time.deltaTime, 
                BarlogY - _centerTendencyCoefficient));
            _v = Mathf.Sqrt(_aVx * _aVx + _aVy * _aVy);
            _anim.SetFloat("speed", _v);
            _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
            _r = Mathf.Asin(_aVx / _v);
            Rigidbody.transform.rotation = Quaternion.RotateTowards(BarlogRot, 
                             Quaternion.Euler(new Vector3(BarlogRot.x, BarlogRot.y, 
                                 _r * 180 / Mathf.PI* RotationDirection(Input.GetAxis("Vertical")))), 90);
            if (BarlogY > 800) _healthPoints -= 1;
            if (_healthPoints < 0) _healthPoints = 0;
            hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
            print(Mathf.Round(_v).ToString() + " ft./s.");
        }
    }

    int RotationDirection(float GetAxisVertical)
    {
        if (GetAxisVertical > 0)
        {
            return -1;
        }
        else return 1;
    }

    void BoostEvent(int v)
    {
        _aTrigger = 1 * v;
        if (v == 1 && startButtonPressed == 1) {
            _healthPointsDelta = 5;
            _healthPoints += _healthPointsDelta;
            HealthPointsSpawn(_healthPointsDelta, new Color32(0, 255, 0, 255));
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "VertebraPoint(Clone)")
        {
            _healthPointsDelta += Random.Range(1, 3);
            GetComponent<AudioSource>().PlayOneShot(soundBarlogHit2, 1f);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            _healthPointsDelta += Random.Range(1, 2);
        }
        _healthPoints -= _healthPointsDelta;
        if (_healthPoints < 0) _healthPoints = 0;
        hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
        if (_healthPointsCooldownTrigger == 0) {
            _healthPointsCooldownTrigger = 1;
            StartCoroutine(HealthPointsRedCoroutine(_healthPointsDelta));
        }
    }
    IEnumerator HealthPointsRedCoroutine(int healthPointsDelta)
    {
        yield return new WaitForSeconds(0.1f);
        HealthPointsSpawn(healthPointsDelta*-1, new Color32(255, 0, 0, 255));
        _healthPointsCooldownTrigger = 0;
    }

    void HealthPointsSpawn(int healthPointsValue, Color32 healthPointsColor)
    {
        var hpTextI = Instantiate(hpText, new Vector3(BarlogX + 40, BarlogY, 0), Quaternion.identity);
        hpTextI.GetComponent<HealthPointsPopup>().balrog = gameObject;
        hpTextI.GetComponent<TextMeshPro>().text = healthPointsValue.ToString();
        hpTextI.GetComponent<TextMeshPro>().color = healthPointsColor;
        _healthPointsDelta = 0;
    }


}