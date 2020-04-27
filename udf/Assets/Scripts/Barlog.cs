using UnityEngine;
using TMPro;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CommentTypo")]
public class Barlog : MonoBehaviour
{
    public float VertSpeed; //переменная для передачи в скрипт Mine
    public GameObject cam;
    public TextMeshPro hpText;
    public GameObject hpUi;
    public GameObject deepBoard;
    public GameObject reFallButton;
    public int StartButtonPressed;
    public AudioClip soundBarlogHit2;
    private float _v; //Скорость _v полёта демона.
    private float _r; //Направление _r полёта демона.
    private float _a; //Ускорение A, которое демон создаёт в период маха крыла.
    private int _aTrigger;
    private float _k; //Коэффициент _k трения об воздух (не константа, т.к. зависит от того, как сильно расправлены крылья).
    private float _d; //переменная для вывода всякого в служебное меню
    private const int Ratio = 20;
    private float _aVx;
    private float _aVy;
    private float _centerTendencyCoefficient;
    private Animator _anim;
    private int _healthPoints = 100;
    private int _healthPointsDelta;
    private int _healthPointsCooldownTrigger;
    private bool _collided;
    private float BarlogX => transform.position.x;
    private float BarlogY => transform.position.y;
    private Quaternion BarlogRot => transform.rotation;
    private bool _reboundWingsBlockTrigger;
    Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();
    
    private void StartGame()
    {
        _anim = GetComponent<Animator>();
        StartButtonPressed = 1;
    }
    private void Update()
    {
        if (_healthPoints <= 0) {
            StartButtonPressed = 0;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            deepBoard.SetActive(true);
            reFallButton.SetActive(true);
        }
        if (StartButtonPressed == 1)
        {
            if (_collided)
            {
                _anim.SetBool("Collided", true);
                BarlogMovementAndRotation(true, 2);
            }
            else{
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
                BarlogMovementAndRotation(false, 1);
            }
            if (BarlogY > 800) _healthPoints -= 1;
            if (_healthPoints < 0) _healthPoints = 0;
            hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
        }
    }
    private void BarlogMovementAndRotation(bool collidedStatus, int speedCoefficient)
    {
        _v *= 1 - _k * Time.deltaTime;
        _v += _a * Time.deltaTime;
        if (!collidedStatus)
        {
            _aVx = _v* Mathf.Sin(_r);
        }
        _aVy = _v* Mathf.Cos(_r);
        _aVy += 8 * Time.deltaTime;
        VertSpeed = _aVy * Ratio / speedCoefficient;
        if (VertSpeed < 3) VertSpeed = 3;
        Rigidbody.MovePosition(new Vector3(BarlogX + _aVx * Ratio / speedCoefficient * Time.deltaTime, 
            BarlogY - _centerTendencyCoefficient));
        _v = Mathf.Sqrt(_aVx * _aVx + _aVy * _aVy);
        _anim.SetFloat("speed", _v);
        _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
        _r = Mathf.Asin(_aVx / _v);
        Rigidbody.transform.rotation = Quaternion.RotateTowards(BarlogRot, 
            Quaternion.Euler(new Vector3(BarlogRot.x, BarlogRot.y, 
                _r * 180 / Mathf.PI* RotationDirection(Input.GetAxis("Vertical")))), 90);
        if (collidedStatus)
        {
            StartCoroutine(Rebound());
            if (_reboundWingsBlockTrigger == false)
            {
                _reboundWingsBlockTrigger = true;
                StartCoroutine(ReboundWingsBlock());
            }
        }
    }
    private IEnumerator Rebound()
    {
        yield return new WaitForSeconds(0.25f);
        _collided = false;
    }
    
    private IEnumerator ReboundWingsBlock()
    {
        yield return new WaitForSeconds(2f);
        _anim.SetBool("Collided", false);
        _reboundWingsBlockTrigger = false;
    }
    int RotationDirection(float GetAxisVertical)
    {
        if (GetAxisVertical > 0)
        {
            return -1;
        }
        else return 1;
    }

    private void BoostEvent(int v)
    {
        _aTrigger = 1 * v;
        if (v == 1 && StartButtonPressed == 1) {
            _healthPointsDelta = 5;
            _healthPoints += _healthPointsDelta;
            HealthPointsSpawn(_healthPointsDelta, new Color32(0, 255, 0, 255));
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "VertebraPoint(Clone)")
        {
            _collided = true;
            _a = 0;
            _healthPointsDelta += Random.Range(1, 3);
            GetComponent<AudioSource>().PlayOneShot(soundBarlogHit2, 1f);
            if (collision.gameObject.tag == "LeftWall" && _aVx < 0 || collision.gameObject.tag == "RightWall" && _aVx > 0)
            {
                _aVx *= -1;
            }
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
    private IEnumerator HealthPointsRedCoroutine(int healthPointsDelta)
    {
        yield return new WaitForSeconds(0.1f);
        HealthPointsSpawn(healthPointsDelta * -1, new Color32(255, 0, 0, 255));
        _healthPointsCooldownTrigger = 0;
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