using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Assets.Scripts
{
    public class Gandalf : MonoBehaviour
    {
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
            _speed = barlog.GetComponent<Barlog>().VertSpeed;
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
                barlog.GetComponent<Barlog>().StartButtonPressed == 1 &&
                _gandalfVertPosition < 800)
            {
                _fireCooldownTrigger = 1;
                StartCoroutine(Fire(position));
            }
        }
        private IEnumerator Fire(Vector3 position)
        {
            yield return new WaitForSeconds(2);
            Instantiate(projectile, new Vector3(position.x, position.y), Quaternion.identity);
            _fireCooldownTrigger = 0;
        }
    }
}