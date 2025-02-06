using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Manager : MonoBehaviour
{
    [SerializeField] GameObject[] _fogs;
    [SerializeField] bool _isActive;
    [SerializeField] bool _isRoomCompleted;
    [SerializeField] int _count; // We have to set it by hand. Maybe we can find a way to not do it by hand (The trigger enter doesn´t work)

    private void Awake()
    {
        EventManager.Subscribe(TypeEvent.EnemyKilled, EnemyKilled);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEvent.EnemyKilled, EnemyKilled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3 && _isRoomCompleted == false)
        {
            _isActive = true;
            TurnOnFogs();
        }
    }

    public void EnemyKilled(object param)
    {
        if (_isActive == true)
        {
            _count--;
            if(_count <= 0 && _isRoomCompleted == false)
            {
                TurnOffFogs();
                _isActive = false;
                _isRoomCompleted = true;
            }
        }
    }
    public void TurnOffFogs()
    {
        for (int i = 0; i < _fogs.Length; i++)
        {
            _fogs[i].SetActive(false);
        }
    }
    public void TurnOnFogs()
    {
        for (int i = 0; i < _fogs.Length; i++)
        {
            _fogs[i].SetActive(true);
        }
    }

}
