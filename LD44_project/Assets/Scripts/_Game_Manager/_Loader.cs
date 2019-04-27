using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Loader responsible for instantiation of all the manager and controller singletons.
/// </summary>
public class _Loader : MonoBehaviour
{
// Pragmas added for Unity warnings cleanup
#pragma warning disable
    [SerializeField]
    private GameObject gameManager;
    [SerializeField]
    private GameObject levelController;
#pragma warning restore

    private void Awake()
    {
        // Instantiate game manager if not already instantiated
        if(_GameManager.instance == null)
        {
            Instantiate(gameManager);
        }

        // Instantiate level controller if not already instantiated
        if(_LevelController.instance == null)
        {
            Instantiate(levelController);
        }
    }
}
