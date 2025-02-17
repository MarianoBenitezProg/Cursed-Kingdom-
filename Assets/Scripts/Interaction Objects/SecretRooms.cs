using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRooms : Interaction
{
    [SerializeField]float _distanceWithDoor;
    [SerializeField]GameObject _roomDoor;
    bool isInRoom;
    


    private void Update()
    {
        CheckDistance();
        if(Input.GetKeyDown(interactionKey) && _distanceWithPlayer <= _distanceForAction || Input.GetKeyDown(interactionKey) && _distanceWithDoor <= _distanceForAction)
        {
            Action();
        }
    }

    public override void Action()
    {
        if(IsMarkus == _playerRef.isMarkus)
        {
            if (isInRoom == false)
            {
                _playerRef.transform.position = _roomDoor.transform.position;
                isInRoom = true;
            }
            else
            {
                _playerRef.transform.position = this.transform.position;
                isInRoom = false;
            }
            SoundManager.instance.PlaySound("Hatchdoor");
        }
        
    }
    void CheckDistance()
    {
        if(_playerRef != null)
        {
            _distanceWithPlayer = Vector3.Distance(this.transform.position, _playerRef.transform.position);
            _distanceWithDoor = Vector3.Distance(_roomDoor.transform.position, _playerRef.transform.position);
        }
    }
}
