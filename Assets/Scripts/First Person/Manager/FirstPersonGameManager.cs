using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonGameManager : MonoBehaviour
{
    #region Instance
    public static FirstPersonGameManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private FirstPersonModel _playerModel;
    public FirstPersonModel PlayerModel
    {
        get { return _playerModel; }
        set { _playerModel = value; }
    }
}
