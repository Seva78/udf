using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Spine.Unity;
namespace Assets.Scripts
{
    public class Gandalf : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public AnimationReferenceAsset idle;
        public AnimationReferenceAsset attack;
        public string currentState;
        public float gandalfVertPosDefault;
        public float gandalfVertPosHighSpeed;
        public int gandalfMaxSpeed;
        public GameObject barlog;
        public GameObject projectile;
        public GameObject controller;
        private float _gandalfVertPosition;
        private float _speed;
        private List<GameObject> _mineList;
        private int _fireCooldownTrigger;
        private void Start()
        {
            _gandalfVertPosition = gandalfVertPosDefault;
            currentState = "Idle";
            SetCharacterState(currentState);
        }
        private void Update()
        {
            _mineList = controller.GetComponent<Mine>().mineList;
            var checkStart = _mineList.IndexOf(_mineList.First()) + 2;
            int checkFinish;
            if (_mineList.Count < 7) checkFinish = _mineList.IndexOf(_mineList.Last());
            else checkFinish = _mineList.IndexOf(_mineList.First()) + 6;
            var position = transform.position;
            for (var i = checkStart; i < checkFinish; i++)
            {
                var leftPointPosition = _mineList[i].GetComponent<Vertebra>().LeftPoint.transform.position;
                var rightPointPosition = _mineList[i].GetComponent<Vertebra>().RightPoint.transform.position;
            
                if (leftPointPosition.x > position.x - 20 && 
                    leftPointPosition.y > position.y - 90 &&
                    leftPointPosition.y < position.y) 
                {
                    transform.position = new Vector3(position.x + 2, position.y);
                }
                if (rightPointPosition.x < position.x + 20 && 
                    rightPointPosition.y > position.y - 90 && 
                    rightPointPosition.y < position.y)
                {
                    transform.position = new Vector3(position.x - 2, position.y);
                }
            }
            _speed = barlog.GetComponent<Balrog>().VertSpeed;
            if (_speed > gandalfMaxSpeed && _gandalfVertPosition < gandalfVertPosHighSpeed)
            {
                _gandalfVertPosition += (_speed - gandalfMaxSpeed) * Time.deltaTime / 2;
                transform.position = new Vector3(position.x, _gandalfVertPosition);
            }
            if (_speed <= gandalfMaxSpeed && _gandalfVertPosition > gandalfVertPosDefault) {
                _gandalfVertPosition -= (gandalfMaxSpeed - _speed) * Time.deltaTime;
                transform.position = new Vector3(position.x, _gandalfVertPosition);
            }
            if (_fireCooldownTrigger == 0 && 
                barlog.GetComponent<Balrog>().StartButtonPressed == 1 &&
                _gandalfVertPosition < 800)
            {
                _fireCooldownTrigger = 1;
                StartCoroutine(Fire(position));
            }
        }

        private IEnumerator Fire(Vector3 position)
        {
            yield return new WaitForSeconds(2);
            currentState = "Attack";
            SetCharacterState(currentState);
            yield return new WaitForSeconds(0.2f);
            Instantiate(projectile, new Vector3(position.x, position.y), Quaternion.identity);
            _fireCooldownTrigger = 0;
            currentState = "Idle";
            SetCharacterState(currentState);
        }
        public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
        {
            skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        }
        public void SetCharacterState(string state)
        {
            if (state.Equals("Idle"))
            {
                SetAnimation(idle, true, 1f);
            }
            else if (state.Equals("Attack"))
            {
                SetAnimation(attack, false, 1f);
            }
        }
    }
}