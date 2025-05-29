using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothGameManager : MonoBehaviour
{
    #region Instance
    public static SmoothGameManager Instance;

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

    private SmoothPlayerModel _playerModel;
    public SmoothPlayerModel PlayerModel
    {
        get { return _playerModel; }
        set { _playerModel = value; }
    }

}
