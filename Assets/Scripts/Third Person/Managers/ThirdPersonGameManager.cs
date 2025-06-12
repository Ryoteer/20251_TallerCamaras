using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonGameManager : MonoBehaviour
{
    #region Instance
    public static ThirdPersonGameManager Instance;

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

    private ThirdPersonModel _playerModel;
    public ThirdPersonModel PlayerModel
    {
        get { return _playerModel; }
        set { _playerModel = value; }
    }
}
