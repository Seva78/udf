using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gandalf : MonoBehaviour
    {
        public int gandalfY;
        public GameObject barlog;
        public GameObject projectile;
        public GameObject controller;
        private float _speed;
        private List<GameObject> _mineList;
        int _fireCooldownTrigger;
        void Update()
        {
            _mineList = controller.GetComponent<Mine>().mineList;
            int checkStart = _mineList.IndexOf(_mineList.First()) + 2;
            int checkFinish;
            if (_mineList.Count < 7) checkFinish = _mineList.IndexOf(_mineList.Last());
            else checkFinish = _mineList.IndexOf(_mineList.First()) + 6;
            var position = transform.position;
            for (int i = checkStart; i < checkFinish; i++)
            {
                var leftPointPosition = _mineList[i].GetComponent<Vertebra>().leftPoint.transform.position;
                var rightPointPosition = _mineList[i].GetComponent<Vertebra>().rightPoint.transform.position;
            
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
        
            _speed = barlog.GetComponent<Barlog>().VertSpeed*Time.deltaTime;
            if (position.y > gandalfY) {
                transform.position = new Vector3(position.x, position.y - _speed);
            }
            if (_fireCooldownTrigger == 0 && barlog.GetComponent<Barlog>().startButtonPressed == 1)
            {
                _fireCooldownTrigger = 1;
                StartCoroutine(Fire(position));
            }
        }
    
    
    
        IEnumerator Fire(Vector3 position)
        {
            yield return new WaitForSeconds(2);
            Instantiate(projectile, new Vector3(position.x, position.y), Quaternion.identity);
            _fireCooldownTrigger = 0;
        }
    }
}