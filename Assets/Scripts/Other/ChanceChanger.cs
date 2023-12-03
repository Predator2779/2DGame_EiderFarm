
using UnityEngine;

public class ChanceChanger : MonoBehaviour
{
    [SerializeField] private GameObject _gagaHousePrefab;

    [SerializeField] private GameObject _gagaHouseParent;

    private void Awake()
    {
        _gagaHousePrefab.GetComponent<FluffGiver>().ChangeChance(100);
    }
    public void SetChance(int chance)
    {
        _gagaHousePrefab.GetComponent<FluffGiver>().ChangeChance(chance);
        Debug.Log("Поменяли шансы в префабе");

        _gagaHouseParent = GameObject.Find("GAGAHOUSE");
        for (int i = 0; i < _gagaHouseParent.transform.childCount; i++)
        {
            GameObject childGagaHouse = _gagaHouseParent.transform.GetChild(i).gameObject;
            childGagaHouse.GetComponent<FluffGiver>().ChangeChance(chance);
        }
        Debug.Log("Поменяли шансы во всех активных домиках гаг на сцене");
    }

}
