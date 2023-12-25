using Building.Constructions;
using Characters;
using Economy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageSpriteChanger : MonoBehaviour
{
    [SerializeField] private Sprite[] _gradesOpenStorageSprites;
    [SerializeField] private Construction _construciton;


    private void Start()
    {
        _construciton = GetComponent<Construction>();
    }
    public Sprite ChangeSprite()
    {
        switch (_construciton.GetCurrentGrade())
        {
            case 1: return _gradesOpenStorageSprites[0];
            case 2: return _gradesOpenStorageSprites[1];
            case 3: return _gradesOpenStorageSprites[2];
        }
        return _gradesOpenStorageSprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Person>()) return;

        ChangeToGradeSprite();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Person>()) return;
        
        Change(_construciton.GetGradeBuildings()[_construciton.GetCurrentGrade() - 1]);
    }

    public void Change(Sprite sprite) => gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

    public void ChangeToGradeSprite() => Change(ChangeSprite());

}
