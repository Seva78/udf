using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Projectile : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset explosion;
    public GameObject projectileExplosion;
    public string currentState;
    public AudioClip iceBallFire;
    public AudioClip iceBallExplode;
    public int ExplodeTrigger;
    private Vector3 _barlogPosition;
    private Vector3 _gandalfPosition;
    private int _changeTrajectoryTrigger;
    private int _changeTrajectoryValue;
    private void Start()
    {
        _barlogPosition = GameObject.Find("Balrog").transform.position;
        _gandalfPosition = GameObject.Find("Gandalf").transform.position;
        GetComponent<AudioSource>().PlayOneShot(iceBallFire, 1f);
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        if (ExplodeTrigger == 0)
        {
            transform.position = 
                new Vector3(transform.position.x - (_gandalfPosition.x - (_barlogPosition.x + _changeTrajectoryValue)) * Time.deltaTime, 
                    transform.position.y - (_gandalfPosition.y - _barlogPosition.y) * Time.deltaTime);
        }
        if (_changeTrajectoryTrigger == 0) {
            _changeTrajectoryTrigger = 1;
            StartCoroutine("ChangeTrajectory");
        }
    }
    private IEnumerator ChangeTrajectory()
    {
        yield return new WaitForSeconds(0.15f);
        _changeTrajectoryValue = Random.Range(300,-300);
        _changeTrajectoryTrigger = 0;
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        ExplodeTrigger = 1;
        transform.localScale = new Vector3(10,10,1);
        transform.parent = collision.gameObject.transform;
        GetComponent<AudioSource>().PlayOneShot(iceBallExplode, 1f);
        currentState = "Explosion";
        SetCharacterState(currentState);
//        Instantiate(projectileExplosion, transform.position, Quaternion.identity);
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine("Destroy");
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.55f);
        Destroy(gameObject);
    }
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }
    public void SetCharacterState(string state)
    {
        if (state.Equals("Explosion"))
        {
            SetAnimation(explosion, true, 1f);
        }
    }
}