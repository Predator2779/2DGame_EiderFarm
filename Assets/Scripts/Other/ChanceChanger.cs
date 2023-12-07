
using Building;
using UnityEngine;

public class ChanceChanger : MonoBehaviour
{
    [SerializeField] private GameObject _gagaHousePrefab;

    private GameObject[] _gagaHouses;

    // [SerializeField] private GameObject gb;

    [SerializeField] private SaveSerial _save;

    private void Awake()
    {
        _gagaHousePrefab.GetComponent<FluffGiver>().ChangeChance(100);
        _gagaHouses = _save.GetGagaHouses();
        //
        // Debug.Log(gb.transform.GetChild(2).GetComponent<BuildMenu>());
        // Debug.Log(gb.GetComponentInChildren<BuildMenu>());
    }

    public void SetChance(int chance)
    {
        _gagaHousePrefab.GetComponent<FluffGiver>().ChangeChance(chance);

        for (int i = 0; i < _gagaHouses.Length; i++)
        {
            BuildMenu buildMenu = _gagaHouses[i].transform.GetChild(2).GetComponent<BuildMenu>();

            if (buildMenu.IsBuilded)
                buildMenu.GetBuilding().gameObject.GetComponent<FluffGiver>().ChangeChance(70);
        }
    }

}
