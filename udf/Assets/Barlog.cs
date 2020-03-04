using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

public class Barlog : MonoBehaviour
{
    public float vertSpeed; //переменная для передачи в скрипт Mine
    private float _v; //Скорость _v полёта демона.
    private float _r; //Направление _r полёта демона.
    private float _a; //Ускорение A, которое демон создаёт в период маха крыла.
    private int _aTrigger;
    private float _k; //Коэффициент _k трения об воздух (не константа, т.к. зависит от того, как сильно у Б расправлены крылья).
    private float _d; //переменная для вывода всякого в служебное меню
    private int _ratio = 20;
    private float _aVx;
    private float _aVy;
    private float _centerTendencyCoefficient;
    public GameObject cam;
    public TextMeshPro hpText;
    public GameObject hpUi;
    public GameObject deepBoard;
    public GameObject reFallButton;
    private Animator _anim;
    private int _healthPoints = 100;
    private int _healthPointsDelta;
    private int _healthPointsCooldownTrigger;
    public int startButtonPressed;
    public AudioClip soundBarlogHit2;

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
            if (Input.GetAxis("Horizontal") == 0) _r *= 1 - _k * Time.deltaTime; //если юзер не жмёт ни вправо, ни влево 
            else _r += 3 * Input.GetAxis("Horizontal") * Mathf.PI / 180;

            if (Input.GetAxis("Vertical") > 0)
            {
                GetComponent<SpriteRenderer>().flipY = true;
                _k = 0.99f;
                _a = 0;
                _centerTendencyCoefficient = (transform.position.y - (cam.transform.position.y + 200)) / 100;
            }
            else if (Input.GetAxis("Vertical") == 0)
            {
                GetComponent<SpriteRenderer>().flipY = false;
                _k = 0.5f;
                _a = 0;
                _centerTendencyCoefficient = (transform.position.y - cam.transform.position.y) / 100;
            }
            else {
                GetComponent<SpriteRenderer>().flipY = false;
                _k = 0.4f;
                _centerTendencyCoefficient = (transform.position.y - (cam.transform.position.y - _aTrigger * 200))/100;
                _a = _aTrigger * 20;
            }
            _v *= 1 - _k * Time.deltaTime;
            _v += _a * Time.deltaTime;
            _aVx = _v* Mathf.Sin(_r);
            _aVy = _v* Mathf.Cos(_r);
            _aVy += 8 * Time.deltaTime;
            vertSpeed = _aVy * _ratio * Time.deltaTime;
            if (vertSpeed < 3) vertSpeed = 3;
            GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + _aVx * _ratio * Time.deltaTime, transform.position.y - _centerTendencyCoefficient, transform.position.z));
            _v = Mathf.Sqrt(_aVx * _aVx + _aVy * _aVy);
            _anim.SetFloat("speed", _v);
            _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
            _r = Mathf.Asin(_aVx / _v);
            if (Input.GetAxis("Vertical") <= 0)
            {
                GetComponent<Rigidbody2D>().transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, _r * 180 / Mathf.PI)), 90);
            }
            else
            {
                GetComponent<Rigidbody2D>().transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, -_r * 180 / Mathf.PI)), 90);
            }
            if (transform.position.y > 800) _healthPoints -= 1;
            if (_healthPoints < 0) _healthPoints = 0;
            hpUi.GetComponent<TextMeshProUGUI>().text = "HP: " + _healthPoints.ToString();
        }
    }
    void BoostEvent(int v)
    {
        _aTrigger = 1 * v;
        if (v == 1 && startButtonPressed == 1) {
            _healthPointsDelta = 5;
            _healthPoints += _healthPointsDelta;
            var HP_textI = Instantiate(hpText, new Vector3(transform.position.x + 40, transform.position.y, 0), Quaternion.identity);
            HP_textI.GetComponent<HP>().b = gameObject;
            HP_textI.GetComponent<TextMeshPro>().text = _healthPointsDelta.ToString();
            _healthPointsDelta = 0;
            HP_textI.GetComponent<TextMeshPro>().color = new Color32(0, 255, 0, 255);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SPL" || collision.gameObject.tag == "SPR")
        {
            _healthPointsDelta += Random.Range(1, 3);
            _healthPoints -= _healthPointsDelta;
            GetComponent<AudioSource>().PlayOneShot(soundBarlogHit2, 1f);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            _healthPointsDelta += Random.Range(1, 2);
            _healthPoints -= _healthPointsDelta;
        }
        if (_healthPoints < 0) _healthPoints = 0;
        hpUi.GetComponent<TextMeshProUGUI>().text = "_healthPoints: " + _healthPoints.ToString();
        if (_healthPointsCooldownTrigger == 0) {
            _healthPointsCooldownTrigger = 1;
            StartCoroutine(HP_Coroutine(_healthPointsDelta));
        }
    }
    IEnumerator HP_Coroutine(int HP_delta)
    {
        yield return new WaitForSeconds(0.1f);
        var HP_textI = Instantiate(hpText, new Vector3(transform.position.x + 40, transform.position.y, 0), Quaternion.identity);
        HP_textI.GetComponent<HP>().b = gameObject;
        HP_textI.GetComponent<TextMeshPro>().text = (HP_delta*-1).ToString();
        HP_textI.GetComponent<TextMeshPro>().color = new Color32(255, 0, 0, 255);
        HP_delta_0();
        _healthPointsCooldownTrigger = 0;
    }
    void HP_delta_0() {
        _healthPointsDelta = 0;
    }
}