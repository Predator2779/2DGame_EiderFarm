using System;

[Serializable] public class SaveData
{
    public int Money;
    public int UncleanedFluff;
    public int CleanedFluff;
    public int Cloth;
    public int Flag;

    public int[] GagaHouses;
    public int[] Cleaners;
    public int[] ClothMachines;
    public int[] Storages;

    public bool[] Flags;
}
