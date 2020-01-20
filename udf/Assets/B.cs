using UnityEngine;
using TMPro;
using System.Collections;
public class B : MonoBehaviour
{
    public float vertSpeed; //переменная для передачи в скрипт Mine
    private float V = 0; //Скорость V полёта демона.
    private float R; //Направление R полёта демона.
    private int G = 8; //Ускорение G свободного падения.
    private float A = 0; //Ускорение A, которое демон создаёт в период маха крыла.
    private int A_trigger;
    private float K; //Коэффициент K трения об воздух (не константа, т.к. зависит от того, как сильно у Б расправлены крылья).
    private float D; //переменная для вывода всякого в служебное меню
    private int Ratio = 20;
    private float aVx;
    private float aVy;
    private float OC;
    public GameObject cam;
    public TextMeshPro HP_text;
    public GameObject HP_UI;
    public GameObject Deepboard;
    public GameObject refall_button;
    private Animator _anim;
    private int HP = 100;
    private int HP_delta;
    private int hp_cooldown_trigger;
    public int startButtonPressed;
    public AudioClip sound_barlog_hit2;

    void StartGame()
    {
        _anim = GetComponent<Animator>();
        startButtonPressed = 1;
    }

    void Update()
    {
        if (HP <= 0) {
            startButtonPressed = 0;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            Deepboard.SetActive(true);
            refall_button.SetActive(true);
        }

        if (startButtonPressed == 1)
        {
            if (Input.GetAxis("Horizontal") == 0) R *= 1 - K * Time.deltaTime; //если юзер не жмёт ни вправо, ни влево, 
            else R += 3 * Input.GetAxis("Horizontal") * Mathf.PI / 180;

            if (Input.GetAxis("Vertical") > 0)
            {
                GetComponent<SpriteRenderer>().flipY = true;
                K = 0.99f;
                A = 0;
                OC = (transform.position.y - (cam.transform.position.y + 200)) / 100;
            }
            else if (Input.GetAxis("Vertical") == 0)
            {
                GetComponent<SpriteRenderer>().flipY = false;
                K = 0.5f;
                A = 0;
                OC = (transform.position.y - cam.transform.position.y) / 100;
            }
            else {
                GetComponent<SpriteRenderer>().flipY = false;
                K = 0.4f;
                OC = (transform.position.y - (cam.transform.position.y - A_trigger * 200))/100;
                A = A_trigger * 20;
            }
            V *= 1 - K * Time.deltaTime;
            V += A * Time.deltaTime;
            aVx = V* Mathf.Sin(R);
            aVy = V* Mathf.Cos(R);
            aVy += G * Time.deltaTime;
            vertSpeed = aVy * Ratio * Time.deltaTime;
            if (vertSpeed < 3) vertSpeed = 3;
            GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + aVx * Ratio * Time.deltaTime, transform.position.y - OC, transform.position.z));
            V = Mathf.Sqrt(aVx * aVx + aVy * aVy);
            _anim.SetFloat("speed", V);
            _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
            R = Mathf.Asin(aVx / V);
            if (Input.GetAxis("Vertical") <= 0)
            {
                GetComponent<Rigidbody2D>().transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, R * 180 / Mathf.PI)), 90);
            }
            else
            {
                GetComponent<Rigidbody2D>().transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, -R * 180 / Mathf.PI)), 90);
            }
            if (transform.position.y > 800) HP -= 1;
            if (HP < 0) HP = 0;
            HP_UI.GetComponent<TextMeshProUGUI>().text = "HP: " + HP.ToString();
        }
    }
    void BoostEvent(int v)
    {
        A_trigger = 1 * v;
        if (v == 1 && startButtonPressed == 1) {
            HP_delta = 5;
            HP += HP_delta;
            var HP_textI = Instantiate(HP_text, new Vector3(transform.position.x + 40, transform.position.y, 0), Quaternion.identity);
            HP_textI.GetComponent<HP>().b = gameObject;
            HP_textI.GetComponent<TextMeshPro>().text = HP_delta.ToString();
            HP_delta = 0;
            HP_textI.GetComponent<TextMeshPro>().color = new Color32(0, 255, 0, 255);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SPL" || collision.gameObject.tag == "SPR")
        {
            HP_delta += Random.Range(1, 3);
            HP -= HP_delta;
            GetComponent<AudioSource>().PlayOneShot(sound_barlog_hit2, 1f);
        }
        if (collision.gameObject.tag == "Projectile")
        {
            HP_delta += Random.Range(1, 2);
            HP -= HP_delta;
        }
        if (HP < 0) HP = 0;
        HP_UI.GetComponent<TextMeshProUGUI>().text = "HP: " + HP.ToString();
        if (hp_cooldown_trigger == 0) {
            hp_cooldown_trigger = 1;
            StartCoroutine(HP_Coroutine(HP_delta));
        }
    }
    IEnumerator HP_Coroutine(int HP_delta)
    {
        yield return new WaitForSeconds(0.1f);
        var HP_textI = Instantiate(HP_text, new Vector3(transform.position.x + 40, transform.position.y, 0), Quaternion.identity);
        HP_textI.GetComponent<HP>().b = gameObject;
        HP_textI.GetComponent<TextMeshPro>().text = (HP_delta*-1).ToString();
        HP_textI.GetComponent<TextMeshPro>().color = new Color32(255, 0, 0, 255);
        HP_delta_0();
        hp_cooldown_trigger = 0;
    }
    void HP_delta_0() {
        HP_delta = 0;
    }
}