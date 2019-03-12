using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomChance
{
    public GameObject Room;
    public int Chance, Id;
    public bool isSpecial, hasEvent, hasSpecial;
    public RoomType type;
    public int Zone;

}


public class RoomList : MonoBehaviour
{
    public int Zone;
    //This is our list we want to use to represent our class as an array.
    public List<RoomChance> twoWay_List = new List<RoomChance>(1);
    public List<RoomChance> cornerWay_List = new List<RoomChance>(1);
    public List<RoomChance> tWay_List = new List<RoomChance>(1);
    public List<RoomChance> endWay_List = new List<RoomChance>(1);
    public List<RoomChance> fourWay_List = new List<RoomChance>(1);


    public void AddNew(RoomType choiceList)
    {
        switch (choiceList)
        {
            case RoomType.TwoWay:
                {
                    twoWay_List.Add(new RoomChance());
                    break;
                }
            case RoomType.CornerWay:
                {
                    cornerWay_List.Add(new RoomChance());
                    break;
                }
            case RoomType.TWay:
                {
                    tWay_List.Add(new RoomChance());
                    break;
                }
            case RoomType.EndWay:
                {
                    endWay_List.Add(new RoomChance());
                    break;
                }
            case RoomType.FourWay:
                {
                    fourWay_List.Add(new RoomChance());
                    break;
                }
        }
    }

    public void Remove(int index, RoomType choiceList)
    {
        //Remove an index position from our list at a point in our list array
            switch (choiceList)
            {
            case RoomType.TwoWay:
                {
                    twoWay_List.RemoveAt(index);
                    break;
                    }
            case RoomType.CornerWay:
                {
                    cornerWay_List.RemoveAt(index);
                    break;
                    }
            case RoomType.TWay:
                {
                    tWay_List.RemoveAt(index);
                    break;
                }
            case RoomType.EndWay:
                {
                    endWay_List.RemoveAt(index);
                    break;
                }
            case RoomType.FourWay:
                {
                    fourWay_List.RemoveAt(index);
                    break;
                }
        }
    }
}
