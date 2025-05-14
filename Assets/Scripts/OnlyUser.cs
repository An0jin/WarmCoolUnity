using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyUser : MonoBehaviour
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        gameObject.SetActive(!Session.session.isGeust);
    }
}
