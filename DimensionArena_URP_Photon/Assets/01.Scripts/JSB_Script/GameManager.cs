using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance;
    public static GameManager instance
    {
        get
        {
            if(null == m_instance)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    [SerializeField]
    public GameObject playerPrefab;

    private void Awake()
    {
        if (instance != this)
            Destroy(gameObject);
    }

}
