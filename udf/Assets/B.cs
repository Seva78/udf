using UnityEngine;
using UnityEngine.SceneManagement;
public class B : MonoBehaviour
{
    public float vertSpeed; //переменная для передачи в скрипт Mine
    private float V = 0; //Скорость V полёта демона.
    private float R = Mathf.PI / 2; //Направление R полёта демона.
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
    private Animator _anim;
    private int HP = 100;

    void Start() {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (HP <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        if (Input.GetAxis("Horizontal") == 0) R *= 1 - K * Time.deltaTime; 
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

        GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + aVx * Ratio * Time.deltaTime, transform.position.y - OC, transform.position.z));

        V = Mathf.Sqrt(aVx* aVx + aVy* aVy);
        _anim.SetFloat("speed", V);
        _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
        R = Mathf.Asin(aVx / V);
        if (Input.GetAxis("Vertical") <= 0)
        {
            GetComponent<Rigidbody2D>().transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, R * 180 / Mathf.PI)), 180);
        }
        else
        {
            GetComponent<Rigidbody2D>().transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, -R * 180 / Mathf.PI)), 180);
        }
    }
    void BoostEvent(int v) {
        A_trigger = 1 * v;
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(206, 10, 100, 30), "HP: " + HP);
        //GUI.Box(new Rect(10, 10, 100, 30), Mathf.Round(V).ToString() + " ft./s. (V)");
        //GUI.Box(new Rect(10, 10, 100, 30), Mathf.Round(V).ToString() + " ft./s. (V)");
        //GUI.Box(new Rect(10, 50, 100, 30), "R = " + R.ToString());
        //GUI.Box(new Rect(10, 90, 100, 30), "A = " + A.ToString());
        //GUI.Box(new Rect(10, 130, 100, 30), "K = " + K.ToString());
        //GUI.Box(new Rect(10, 170, 100, 30), "D = " + D.ToString());
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "SPL" || collision.gameObject.tag == "SPR") HP -= 1;
        if (collision.gameObject.tag == "Projectile") HP -= 5;
    }
}