#region

using UnityEngine;

#endregion

namespace Runtime.UI.MainMenu
{
    public class MainMenuUIRoot : MonoBehaviour
    {
        [field: SerializeField] public Transform HUDLayer;
        [field: SerializeField] public Transform PopupsLayer;
        [field: SerializeField] public Transform VFXUnderPopupsLayer;
        [field: SerializeField] public Transform VFXOverPopupsLayer;
    }
}