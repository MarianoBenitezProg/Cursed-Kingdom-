using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxAndBarrel : BreakableObjects, ItakeDamage
{
    [SerializeField]GameObject[] dropsPrefabs;
    MaterialTintColor _tintColor;

    private void Awake()
    {
        _tintColor = GetComponent<MaterialTintColor>();
    }
    public void TakeDamage(int dmg)
    {
        objectLife--;
        _tintColor.SetTintColor(Color.red); //It doesn´t work with black, idk why
        if(objectLife <=0)
        {
            Destruction();
        }
    }

    protected override void Destruction()
    {
        int randomizer = Random.Range(0,10);
        if(randomizer <= 2)
        {
            int objectDropRandomizer = Random.Range(0, dropsPrefabs.Length);
            Instantiate(dropsPrefabs[objectDropRandomizer],transform.position, Quaternion.identity);
            Debug.Log(dropsPrefabs[objectDropRandomizer]);
        }
        Destroy(gameObject);
    }
}
