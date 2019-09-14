using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    public float vertSpeed; //переменная для передачи в скрипт Mine
    //public float horSpeed;
    //public float rotateSpeed;
    private float V = 0; //Скорость V полёта демона.
    private float R = Mathf.PI / 2; //Направление R полёта демона.
    private int G = 8; //Ускорение G свободного падения.
    private float A = 0; //Ускорение A, которое демон создаёт в период маха крыла.
    private float K = 0.2f; //Коэффициент K трения об воздух (не константа, т.к. зависит от того, как сильно у Б расправлены крылья).
    private float D; //переменная для вывода всякого в служебное меню
    private int Ratio = 20;
    //private int L = 200 - 64;
    private float aVx;
    private float aVy;
    //private bool aHit;

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            R += 3 * Input.GetAxis("Horizontal") * Mathf.PI / 180;
        }
        else R *= 1 - K * Time.deltaTime;

        if (Input.GetAxis("Vertical") > 0)
        {
            GetComponent<SpriteRenderer>().flipY = true;
            K = 0.99f;
            A = 0;
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            GetComponent<SpriteRenderer>().flipY = false;
            K = 0.5f;
            A = 0;
        }
        else {
            GetComponent<SpriteRenderer>().flipY = false;
            K = 0.4f;
        }

        V *= 1 - K * Time.deltaTime;
        V += A * Time.deltaTime;
        aVx = V* Mathf.Sin(R);
        aVy = V* Mathf.Cos(R);
        aVy += G * Time.deltaTime;
        vertSpeed = aVy * Ratio * Time.deltaTime;

        GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + aVx * Ratio * Time.deltaTime, transform.position.y, transform.position.z));

        V = Mathf.Sqrt(aVx* aVx + aVy* aVy);
        R = Mathf.Asin(aVx / V);
        if (Input.GetAxis("Vertical") <= 0)
        {
            GetComponent<Rigidbody2D>().transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, R * 180 / Mathf.PI)), 100);
        }
        else
        {
            GetComponent<Rigidbody2D>().transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, -R * 180 / Mathf.PI)), 100);
        }
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 30), Mathf.Round(V).ToString() + " ft./s. (V)");
        GUI.Box(new Rect(10, 50, 100, 30), "R = " + R.ToString());
        GUI.Box(new Rect(10, 90, 100, 30), "A = " + A.ToString());
        GUI.Box(new Rect(10, 130, 100, 30), "K = " + K.ToString());
        //GUI.Box(new Rect(10, 170, 100, 30), "D = " + D.ToString());
    }


}


//        if (Input.GetAxis("Horizontal") != 0)
//        {
//            R += 3 * Input.GetAxis("Horizontal") * Mathf.PI / 180;
//        }
//        else R *= 1 - K;
//        if (Input.GetAxis("Vertical") < 0)
//        {
//            K = 0.99f;
//            A = 0;
//        }
//        else if (Input.GetAxis("Vertical") == 0)
//        {
//            K = 0.5f;
//            A = 0;
//        }
//        else {
//            K = 0.4f;
//        }
//        V *= 1 - K;
//        V += A;
//        aVx = V* Mathf.Sin(R);
//aVy = V* Mathf.Cos(R);

//aVy += G;
//        
//        GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + aVx* Ratio, transform.position.y - aVy* Ratio, transform.position.z));
//        if (GetComponent<Rigidbody2D>().transform.position.x< -L) {
//            GetComponent<Rigidbody2D>().MovePosition(new Vector3(- L + (-L - transform.position.x), transform.position.y, transform.position.z));
//            aVx = Mathf.Abs(aVx);
//            aHit = true;
//        }
//        if (GetComponent<Rigidbody2D>().transform.position.x > L)
//        {
//            GetComponent<Rigidbody2D>().MovePosition(new Vector3(L - (transform.position.x - L), transform.position.y, transform.position.z));
//            aVx = -Mathf.Abs(aVx);
//            aHit = true;
//        }
//        if (aHit) {
//            aVx *= 0.5f;
//            aVy *= 0.5f;
//            A = 0;
//        }
//        V = Mathf.Sqrt(aVx* aVx + aVy* aVy);
//        R = Mathf.Asin(aVx / V);

//        if (Input.GetAxis("Vertical") >= 0)
//        {
//            GetComponent<Rigidbody2D>().transform.Rotate(transform.rotation.x, transform.rotation.y, -R* 180 / Mathf.PI);
//        }
//        else {
//            GetComponent<Rigidbody2D>().transform.Rotate(transform.rotation.x, transform.rotation.y, R* 180 / Mathf.PI);
//        }



//if (Input.GetAxis("Horizontal") != 0 && GetComponent<Rigidbody2D>().transform.rotation.z > -0.7 && GetComponent<Rigidbody2D>().transform.rotation.z < 0.7)
//{
//GetComponent<Rigidbody2D>().MovePosition(new Vector3(transform.position.x + Input.GetAxis("Horizontal") * horSpeed, transform.position.y, transform.position.z));
//    GetComponent<Rigidbody2D>().transform.Rotate(transform.rotation.x, transform.rotation.y, Input.GetAxis("Horizontal") * rotateSpeed);
//}
//else GetComponent<Rigidbody2D>().transform.rotation = 
//        Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, 0)), rotateSpeed);

//if (Input.GetAxis("Vertical") != 0)
//{
//    vertSpeed = -Input.GetAxis("Vertical");
//}