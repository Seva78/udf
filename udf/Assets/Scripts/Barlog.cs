﻿using UnityEngine;
using TMPro;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Serialization;

[SuppressMessage("ReSharper", "CommentTypo")]
public class Barlog : MonoBehaviour
{
    public float VertSpeed; //переменная для передачи в скрипт Mine
    public GameObject cam;
    public GameObject deepBoard;
    public GameObject reFallButton;
    public int StartButtonPressed;
    public AudioClip soundBarlogHit2;
    private float _velocity; //Скорость полёта демона.
    private float _rotation; //Направление полёта демона.
    private float _acceleration; //Ускорение, которое демон создаёт в период маха крыла.
    private int _accelerationTrigger;
    private float _windage; //Коэффициент трения об воздух (не константа, т.к. зависит от того, как сильно расправлены крылья).
    private const int Ratio = 20;
    private float _aVx;
    private float _aVy;
    private Vector3 _moveTo; // Куда стремится барлог средствами Rigidbody.MovePosition 
    private float _centerTendencyCoefficient;
    private Animator _anim;
    private bool _rebounded;
    private float BarlogY => transform.position.y;
    private Quaternion BarlogRot => transform.rotation;
    Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();
    
    private void StartGame()
    {
        _anim = GetComponent<Animator>();
        StartButtonPressed = 1;
        // QualitySettings.vSyncCount = 0;  // VSync must be disabled
        // Application.targetFrameRate = 15;
    }

    public void Death()
    {
        StartButtonPressed = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        deepBoard.SetActive(true);
        reFallButton.SetActive(true);
    }

    private void Update()
    {
        if (StartButtonPressed == 1)
        {
            if (_rebounded)
            {
                _anim.SetBool("Collided", true);
                BarlogMovementAndRotation(true, 10);
            }
            else
            {
                _rotation = MakeRotationFromMinusPiToPI(_rotation);
                var inputGetAxisHorizontal = Input.GetAxis("Horizontal");

                //если юзер не жмёт ни вправо, ни влево
                if (inputGetAxisHorizontal == 0) _rotation *= 1 - _windage * Time.deltaTime;
                else
                {
                    if (_rotation > Mathf.PI / 2 && inputGetAxisHorizontal > 0) inputGetAxisHorizontal = 0;
                    if (_rotation < -Mathf.PI / 2 && inputGetAxisHorizontal < 0) inputGetAxisHorizontal = 0;
                    _rotation += 2 * inputGetAxisHorizontal * Mathf.PI * Time.deltaTime;
                }
                if (Input.GetAxis("Vertical") > 0)
                {
                    GetComponent<SpriteRenderer>().flipY = true;
                    _windage = 0.99f;
                    _acceleration = 0;
                    _centerTendencyCoefficient = (BarlogY - (cam.transform.position.y + 200)) / 100;
                }
                else if (Input.GetAxis("Vertical") == 0)
                {
                    GetComponent<SpriteRenderer>().flipY = false;
                    _windage = 0.5f;
                    _acceleration = 0;
                    _centerTendencyCoefficient = (BarlogY - cam.transform.position.y) / 100;
                }
                else {
                    GetComponent<SpriteRenderer>().flipY = false;
                    _windage = 0.4f;
                    _centerTendencyCoefficient = (BarlogY - (cam.transform.position.y - _accelerationTrigger * 200))/100;
                    _acceleration = _accelerationTrigger * 20;
                }
                BarlogMovementAndRotation(false, 1);
            }
        }
    }
    private void BarlogMovementAndRotation(bool collidedStatus, int speedCoefficient)
    {
        _velocity *= 1 - _windage * Time.deltaTime * speedCoefficient;
        _velocity += _acceleration * Time.deltaTime;
        _aVx = _velocity * Mathf.Sin(_rotation);
        _aVy = _velocity * Mathf.Cos(_rotation);

        if (!collidedStatus)
        {
            _aVy += 8 * Time.deltaTime;
            _velocity = Mathf.Sqrt(_aVx * _aVx + _aVy * _aVy);
            _anim.SetFloat("speed", _velocity);
            _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
            _rotation = Vector2.SignedAngle(new Vector2(_aVx, _aVy), Vector2.up) * Mathf.Deg2Rad;
        }
        Rigidbody.transform.rotation = Quaternion.RotateTowards(BarlogRot, 
            Quaternion.Euler(new Vector3(BarlogRot.x, BarlogRot.y, 
                _rotation * Mathf.Rad2Deg * RotationDirection(Input.GetAxis("Vertical")))), 180);

        // VertSpeed = _aVy * Ratio / speedCoefficient;
        VertSpeed = _aVy * Ratio;
        if (!collidedStatus && VertSpeed < 3) VertSpeed = 3;

        // В начале полёта и после столкновения начинаем с фактического местоположения
        if (_moveTo == Vector3.zero) _moveTo = transform.position;
        // Корректируем позицию куда стремится барлог
        _moveTo += new Vector3(_aVx * Ratio * Time.deltaTime,
            - _centerTendencyCoefficient * Time.deltaTime * 100);
        // _moveTo += new Vector3(_aVx * Ratio / speedCoefficient * Time.deltaTime,
            // - _centerTendencyCoefficient * Time.deltaTime * 100);
        // Направляем барлога в расчётную позицию
        Rigidbody.MovePosition(_moveTo);
    }
    int RotationDirection(float getAxisVertical)
    {
        if (getAxisVertical > 0) return -1;
        else return 1;
    }

    private void BoostEvent(int v)
    {
        _accelerationTrigger = v;
        if (v == 1 && StartButtonPressed == 1) {
            GetComponent<HealthPointsManager>().BoostHeal();
        }
    }

    private float MakeDegreePositive(float degree)
    {
        return (degree + 360) % 360;
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "VertebraPoint(Clone)" && !_rebounded)
        {
            _acceleration = 0;
            var wallRotation = MakeDegreePositive(collision.gameObject.transform.eulerAngles.z - 90);
            var balrogRotation = MakeDegreePositive(transform.eulerAngles.z);
            GetComponent<HealthPointsManager>().CollisionDamage();
            GetComponent<AudioSource>().PlayOneShot(soundBarlogHit2, 1f);
            _moveTo = Vector3.zero; // Обнуляем позицию к которой стремимся. Чтобы начать от фактического положения
            StartCoroutine(ReboundWingsBlock());
            if (collision.gameObject.tag == "RightWall" && wallRotation < 90 ||
                collision.gameObject.tag == "RightWall" && wallRotation > 325 || 
                collision.gameObject.tag == "LeftWall" && wallRotation < 35 ||
                collision.gameObject.tag == "LeftWall" && wallRotation > 270)
            {
                _rebounded = true;
                StartCoroutine(Rebound()); 
                var rotationDifference = wallRotation - balrogRotation;
                var angleAfterRebound = MakeDegreePositive(wallRotation + rotationDifference);
                if (angleAfterRebound > 45 && angleAfterRebound <= 180) angleAfterRebound = 45;
                if (angleAfterRebound > 180 && angleAfterRebound < 315) angleAfterRebound = 315;            
                _rotation = angleAfterRebound * Mathf.Deg2Rad;
                _rotation = MakeRotationFromMinusPiToPI(_rotation);
                if (collision.gameObject.tag == "LeftWall" && _rotation < 0) _rotation *= -1;
                if (collision.gameObject.tag == "RightWall" && _rotation > 0) _rotation *= -1;   
            }
            else
            {
                _rotation = wallRotation * Mathf.Deg2Rad;
            }
        }
        if (collision.gameObject.tag == "Projectile")
        {
            GetComponent<HealthPointsManager>().ProjectileDamage();
        }
    }
    private IEnumerator Rebound()
    {
        yield return new WaitForSeconds(0.15f);
        _rebounded = false;
    }
    
    private IEnumerator ReboundWingsBlock()
    {
        yield return new WaitForSeconds(2f);
        _anim.SetBool("Collided", false);
    }
    private float MakeRotationFromMinusPiToPI(float rotation)
    {
        // Приводим _rotation к значению [от -PI до PI]
        while (rotation > Mathf.PI) rotation -= Mathf.PI * 2;
        while (rotation < -Mathf.PI) rotation += Mathf.PI * 2;
        return rotation;
    }

}