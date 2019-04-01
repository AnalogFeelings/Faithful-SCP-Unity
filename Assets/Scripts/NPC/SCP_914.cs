using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCP_914 : MonoBehaviour
{
    public GameObject Knob, spawner;
    public Object_914Dial dial;
    public Object_914Key key;
    public Transform intake, outake;
    public Object_Door door1, door2;
    public LayerMask items;
    public AudioClip refine;

    Collider [] inItems;

    bool Activated;
    float Refining;

    [System.Serializable]
    public class ChanceTable
        {
            public Item Spawn;
            public int Rate;
        }

    [System.Serializable]
    public class Table914
    {
        public Item Original;
        public ChanceTable [] _Coarse;
        public ChanceTable[] _Rough;
        public ChanceTable[] _11;
        public ChanceTable[] _Fine;
        public ChanceTable[] _VeryFine;
    }

    public Table914 [] itemtable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (dial.Option)
        {
            case 0:
                {
                    Knob.transform.localRotation = Quaternion.Euler(0, 0, 83f);
                    break;
                }
            case 1:
                {
                    Knob.transform.localRotation = Quaternion.Euler(0, 0, 43.5f);
                    break;
                }
            case 2:
                {
                    Knob.transform.localRotation = Quaternion.Euler(0, 0, 0f);
                    break;
                }
            case 3:
                {
                    Knob.transform.localRotation = Quaternion.Euler(0, 0, -44f);
                    break;
                }
            case 4:
                {
                    Knob.transform.localRotation = Quaternion.Euler(0, 0, -83f);
                    break;
                }
        }
        if (key.Activated == true && Activated == false)
        {
            Activated = true;
            GameController.instance.GlobalSFX.PlayOneShot(refine);
            door1.DoorSwitch();
            door2.DoorSwitch();
            Refining = 13f;
        }

        if (Activated == true)
        {
            Refining -= Time.deltaTime;
            if (Refining <= 0)
            {
                Refine();
                door1.DoorSwitch();
                door2.DoorSwitch();
                Activated = false;
            }
        }




    }

    public void Refine()
    {
        inItems = Physics.OverlapSphere(intake.transform.position, 1.5f, items);

        if (inItems.Length != 0)
        {
            for(int i = 0; i < inItems.Length; i++)
            {
                GameObject newItem;
                newItem = Instantiate(spawner, outake.position, Quaternion.identity);
                newItem.GetComponent<Object_Item>().item = TransformItem(inItems[i].GetComponent<Object_Item>().item);
                newItem.GetComponent<Object_Item>().Spawn();
                inItems[i].GetComponent<Object_Item>().Delete();

            }
        }

    }



    public Item TransformItem (Item item)
    {
        int gend;
        for (int i = 0; i < itemtable.Length; i++)
        {
            if (item.name == itemtable[i].Original.name)
            {
                switch (dial.Option)
                {
                    case 0:
                        {
                            for (int j = 0; j < itemtable[i]._Coarse.Length; j++)
                            {
                                gend = Random.Range(0, 100);
                                if (gend < itemtable[i]._Coarse[j].Rate)
                                {
                                    return (itemtable[i]._Coarse[j].Spawn);
                                }
                            }
                            break;
                        }
                    case 1:
                        {
                            for (int j = 0; j < itemtable[i]._Rough.Length; j++)
                            {
                                gend = Random.Range(0, 100);
                                if (gend < itemtable[i]._Rough[j].Rate)
                                {
                                    return (itemtable[i]._Rough[j].Spawn);
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            for (int j = 0; j < itemtable[i]._11.Length; j++)
                            {
                                gend = Random.Range(0, 100);
                                if (gend < itemtable[i]._11[j].Rate)
                                {
                                    return (itemtable[i]._11[j].Spawn);
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            for (int j = 0; j < itemtable[i]._Fine.Length; j++)
                            {
                                gend = Random.Range(0, 100);
                                if (gend < itemtable[i]._Fine[j].Rate)
                                {
                                    return (itemtable[i]._Fine[j].Spawn);
                                }
                            }
                            break;
                        }
                    case 4:
                        {
                            for (int j = 0; j < itemtable[i]._VeryFine.Length; j++)
                            {
                                gend = Random.Range(0, 100);
                                if (gend < itemtable[i]._VeryFine[j].Rate)
                                {
                                    return (itemtable[i]._VeryFine[j].Spawn);
                                }
                            }
                            break;
                        }

                }



                return (item);
            }



        }

        return (item);


    }
}
