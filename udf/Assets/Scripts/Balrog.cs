using System.Collections;
using UnityEngine;
using System.Diagnostics.CodeAnalysis;
using Spine.Unity;

[SuppressMessage("ReSharper", "CommentTypo")]
public class Balrog : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset balrogIdle;
    public AnimationReferenceAsset barlogWingsCollapsed;
    public AnimationReferenceAsset balrogFlyDown;
    public string currentState;
    public GameObject balrogUp;
    public float VertSpeed; //переменная для передачи в скрипт Mine
    public GameObject cam;
    public GameObject deepBoard;
    public GameObject reFallButton;
    public int StartButtonPressed;
    public AudioClip soundBarlogHit2;
    private float _velocity; //Скорость полёта демона.
    private float _rotation; //Направление полёта демона.
    private float _сenterTendencyCoefficient;
    public float Acceleration; //Ускорение, которое демон создаёт в период маха крыла.
    private int _accelerationTrigger;
    private float _windage; //Коэффициент трения об воздух (не константа, т.к. зависит от того, как сильно расправлены крылья).
    private const int Ratio = 20;
    private float _aVx;
    private float _aVy;
    private Vector3 _moveTo; // Куда стремится барлог средствами Rigidbody.MovePosition 
    private bool _rebounded;
    private float BalrogY => transform.position.y;
    private Quaternion BalrogRot => transform.rotation;
    Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();
    private Coroutine _reboundWingsBlock;
    private Coroutine _rebound;
    private void StartGame()
    {
        StartButtonPressed = 1;
        currentState = "Idle";
        SetCharacterState(currentState);
    }
    void Start()
    {
        skeletonAnimation.state.Event += Boost_Event;
    }

    private void Boost_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "swing") {
            _accelerationTrigger = 1;
            Debug.Log(_accelerationTrigger);
            if (StartButtonPressed == 1)
            {
                GetComponent<HealthPointsManager>().BoostHeal();
            }
        }
    }

    public void Death()
    {
        StartButtonPressed = 0;
        enabled = false;
        //GetComponent<CircleCollider2D>().enabled = false;
        deepBoard.SetActive(true);
        reFallButton.SetActive(true);
    }

    private void Update()
    {
        if (StartButtonPressed == 1)
        {
            if (_rebounded)
            {
                BalrogMovementAndRotation(true, 10);
            }
            else
            {
                _rotation = MakeRotationFromMinusPiToPI(_rotation);
                var inputGetAxisHorizontal = Input.GetAxis("Horizontal");
                //если юзер не жмёт ни вправо, ни влево
                if (inputGetAxisHorizontal == 0) _rotation *= 1 - _windage * Time.deltaTime;
                else
                {
                    if (_rotation > Mathf.PI / 3 && inputGetAxisHorizontal > 0) inputGetAxisHorizontal = 0;
                    if (_rotation < -Mathf.PI / 3 && inputGetAxisHorizontal < 0) inputGetAxisHorizontal = 0;
                    _rotation += 2 * inputGetAxisHorizontal * Mathf.PI * Time.deltaTime;
                } 
                if (Input.GetAxis("Vertical") > 0)
                {
                    balrogUp.SetActive(true);
                    GetComponent<MeshRenderer>().enabled = false;
                    if (currentState != "Idle")
                    {
                        currentState = "Idle";
                        SetCharacterState(currentState);
                    }
                    _windage = 0.99f;
                    Acceleration = 0;
                    _сenterTendencyCoefficient = (BalrogY - (cam.transform.position.y + 200)) / 100;
                }
                else if (Input.GetAxis("Vertical") == 0)
                {
                    balrogUp.SetActive(false);
                    GetComponent<MeshRenderer>().enabled = true;
                    if (currentState != "Idle")
                    {
                        currentState = "Idle";
                        SetCharacterState(currentState);
                    }
                    _windage = 0.5f;
                    Acceleration = 0;
                    _сenterTendencyCoefficient = (BalrogY - (cam.transform.position.y + 100)) / 100;
                }
                else {
                    balrogUp.SetActive(false);
                    GetComponent<MeshRenderer>().enabled = true;
                    if (currentState != "Fly" && currentState != "FlyDown")
                    {
                        currentState = "Fly";
                        SetCharacterState(currentState);
                    }
                    _windage = 0.4f;
                    _сenterTendencyCoefficient = 
                        (BalrogY - (cam.transform.position.y + 100 - _accelerationTrigger * 200))/100;
                    Acceleration = _accelerationTrigger * 20;
                }
                BalrogMovementAndRotation(false, 1);
            }
        }
    }
    private void BalrogMovementAndRotation(bool collidedStatus, int speedCoefficient)
    {
        _velocity *= 1 - _windage * Time.deltaTime * speedCoefficient;
        _velocity += Acceleration * Time.deltaTime;
        _aVx = _velocity * Mathf.Sin(_rotation);
        _aVy = _velocity * Mathf.Cos(_rotation);

        if (!collidedStatus)
        {
            _aVy += 8 * Time.deltaTime;
            _velocity = Mathf.Sqrt(_aVx * _aVx + _aVy * _aVy);
            if (currentState != "FlyDown" && _velocity >= 17)
            {
                currentState = "FlyDown";
                SetCharacterState(currentState);
            }
//            _anim.SetFloat("speed", _velocity);
//            _anim.SetFloat("InputGetAxisVertical", Input.GetAxis("Vertical"));
            _rotation = Vector2.SignedAngle(new Vector2(_aVx, _aVy), Vector2.up) * Mathf.Deg2Rad;
        }
        Rigidbody.transform.rotation = Quaternion.RotateTowards(BalrogRot, 
            Quaternion.Euler(new Vector3(BalrogRot.x, BalrogRot.y, 
                _rotation * Mathf.Rad2Deg * RotationDirection(Input.GetAxis("Vertical")))), 180);

        VertSpeed = _aVy * Ratio;
        if (!collidedStatus && VertSpeed < 3) VertSpeed = 3;

        // В начале полёта и после столкновения начинаем с фактического местоположения
        if (_moveTo == Vector3.zero) _moveTo = transform.position;
        // Корректируем позицию куда стремится барлог
        _moveTo += new Vector3(_aVx * Ratio * Time.deltaTime, - _сenterTendencyCoefficient * Time.deltaTime * 100);
        // Направляем барлога в расчётную позицию
        Rigidbody.MovePosition(_moveTo);
    }
    int RotationDirection(float getAxisVertical)
    {
        if (getAxisVertical > 0) return -1;
        else return 1;
    }

    void BoostEvent(int v)
    {
        _accelerationTrigger = v;
        Debug.Log(_accelerationTrigger);
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
        if (collision.gameObject.name == "VertebraPoint(Clone)")
        {
//            _anim.SetBool("Collided", true);
            Acceleration = 0;
            var wallRotation = MakeDegreePositive(collision.gameObject.transform.eulerAngles.z - 90);
            var balrogRotation = MakeDegreePositive(transform.eulerAngles.z);
            GetComponent<HealthPointsManager>().CollisionDamage();
            GetComponent<AudioSource>().PlayOneShot(soundBarlogHit2, 1f);
            _moveTo = Vector3.zero; // Обнуляем позицию к которой стремимся. Чтобы начать от фактического положения
            if(_reboundWingsBlock != null) StopCoroutine(_reboundWingsBlock);
            _reboundWingsBlock = StartCoroutine(ReboundWingsBlock());
            if (collision.gameObject.tag == "RightWall" && wallRotation < 90 ||
                collision.gameObject.tag == "RightWall" && wallRotation > 325 || 
                collision.gameObject.tag == "LeftWall" && wallRotation < 35 ||
                collision.gameObject.tag == "LeftWall" && wallRotation > 270)
            {
                _rebounded = true;
                if (_rebound != null) StopCoroutine(_rebound);
                _rebound = StartCoroutine(Rebound());
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
//        _anim.SetBool("Collided", false);
    }
    private float MakeRotationFromMinusPiToPI(float rotation)
    {
        // Приводим _rotation к значению [от -PI до PI]
        while (rotation > Mathf.PI) rotation -= Mathf.PI * 2;
        while (rotation < -Mathf.PI) rotation += Mathf.PI * 2;
        return rotation;
    }
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("Idle"))
        {
            SetAnimation(balrogIdle, true, 1f);
        }
        else if (state.Equals("Fly"))
        {
            SetAnimation(barlogWingsCollapsed, true, 1f);
        }
        else if (state.Equals("FlyDown"))
        {
            SetAnimation(balrogFlyDown, true, 1f);
        }
    }
}
