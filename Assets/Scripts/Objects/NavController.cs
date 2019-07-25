using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NavController : MonoBehaviour
{
    public GameObject Display, Offline, Battery, map, MapCamera, MapTarget;
    public RectTransform batteryRect;
    
    Equipable_Nav Nav;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnEnable()
    {
        Nav = ((Equipable_Nav)GameController.instance.player.GetComponent<Player_Control>().equipment[(int)bodyPart.Hand]);
        if (Nav.isOnline)
        {
            GameController.instance.Map_RenderFull();
            Offline.SetActive(false);
        }
        else
        {
            GameController.instance.Map_RenderHalf();
            Offline.SetActive(true);
        }

        if (Nav.SpendBattery)
        {
            Battery.SetActive(true);
        }
        else
        {
            Battery.SetActive(false);
        }

        if (Nav.valueFloat < 0 && Nav.SpendBattery)
            Display.SetActive(false);
        else
            Display.SetActive(true);
    }

    private void Update()
    {
        int batPercent = ((int)Mathf.Floor((Nav.valueFloat / (100 / 100)) / 5));
        batteryRect.sizeDelta = new Vector2(batPercent * 8, 14);

        MapCamera.transform.position = new Vector3(GameController.instance.xPlayer, GameController.instance.yPlayer, MapCamera.transform.position.z);
        MapTarget.transform.position = new Vector3(GameController.instance.xPlayer + 0.5f, GameController.instance.yPlayer + 0.5f, MapTarget.transform.position.z);
        MapTarget.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -GameController.instance.player.transform.eulerAngles.y);
    }




}
