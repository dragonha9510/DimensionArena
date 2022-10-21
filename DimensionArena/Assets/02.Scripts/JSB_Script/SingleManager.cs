using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SingleManager : MonoBehaviour
{
    private enum OPTION
    {
        OPTION_OPTIONS,
        OPTION_REDZONE,
        OPTION_SAFEZONE,
        OPTION_MAPSELECT,
        OPTION_END
    }

    [SerializeField]
    private GameObject[] mapPrefaps;
    [SerializeField]
    private GameObject[] playerPrefaps;
    [SerializeField]
    private GameObject[] optionLists;
    [SerializeField]
    private GameObject[] mapSelectButton;


    [SerializeField]
    private GameObject redZone;
    [SerializeField]
    private GameObject safeZone;


    // Dummy Character
    [SerializeField]
    private Vector3 dummySpawnPoint;
    [SerializeField]
    private GameObject dummyCharacter;

    private GameObject onFieldDummy;
    ////////////////////

    // Map Select
    private int mapIndex = 0;
    private GameObject map;
    ////////////////////
    
    private bool[] optionsHides = new bool[(int)OPTION.OPTION_END] { false, false , false ,false };
    

    private void Start()
    {

        mapPrefaps = Resources.LoadAll<GameObject>("Tool/Map");
        //playerPrefaps = Resources.LoadAll<GameObject>("")
        map = Instantiate(mapPrefaps[mapIndex],new Vector3(0,-1,0),Quaternion.identity);
    }

    private void ObjectSetActive(OPTION type ,GameObject obj)
    {
        obj.SetActive(!optionsHides[(int)type]);
        optionsHides[(int)type] = !optionsHides[(int)type];
    }

    private void ObjectSetActive(OPTION type, GameObject[] objlist)
    {
        foreach (GameObject obj in objlist)
            obj.SetActive(!optionsHides[(int)type]);
        optionsHides[(int)type] = !optionsHides[(int)type];
    }

    public void CreateDummyCharacter()
    {
        if (null == onFieldDummy)
            onFieldDummy = Instantiate(dummyCharacter, new Vector3(dummySpawnPoint.x, dummySpawnPoint.y, dummySpawnPoint.z),Quaternion.identity);
    }

    public void OptionsHideOrAppear()
    {
        ObjectSetActive(OPTION.OPTION_OPTIONS, optionLists);
    }

    public void RedZoneSetActive()
    {
        ObjectSetActive(OPTION.OPTION_REDZONE, redZone);
    }
    public void SafeZoneSetActive()
    {
        ObjectSetActive(OPTION.OPTION_SAFEZONE, safeZone);
    }

    public void MapSelectSetActive()
    {
        ObjectSetActive(OPTION.OPTION_MAPSELECT, mapSelectButton);
    }

    public void ChangeNextMap()
    {
        if (mapIndex + 1 >= mapPrefaps.Length)
            mapIndex = 0;
        else
            ++mapIndex;
        Destroy(map);
        if (null != onFieldDummy)
            Destroy(onFieldDummy);
        map = Instantiate(mapPrefaps[mapIndex], new Vector3(0, -1, 0), Quaternion.identity);

    }
    public void ChangePrevMap()
    {
        if (mapIndex - 1 < 0)
            mapIndex = mapPrefaps.Length - 1;
        else
            --mapIndex;
        Destroy(map);
        if (null != onFieldDummy)
            Destroy(onFieldDummy);
        map = Instantiate(mapPrefaps[mapIndex], new Vector3(0, -1, 0), Quaternion.identity);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene("Lobby_Main");
    }
}
