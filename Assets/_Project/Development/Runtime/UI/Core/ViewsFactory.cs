#region

using System;
using System.Collections.Generic;
using Runtime.Utility.AssetsLoader;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Runtime.UI.Core
{
    public class ViewsFactory
    {
        private readonly ResourcesAssetsLoader _resourcesAssetsLoader;

        private readonly Dictionary<string, string> _viewIDToResourcesPath = new()
        {
            { ViewIDs.MiniGamePopupViewID, "UI/MiniGameTypePopup" },
            { ViewIDs.MiniGameTileViewID, "UI/MiniGameTypeTile" },
            { ViewIDs.MainMenuScreenViewID, "UI/MainMenu/MainMenuScreen" },
            { ViewIDs.IconTextElementViewID, "UI/Common/IconTextView" },
            { ViewIDs.DoubleTextElementViewID, "UI/Common/DoubleTextView" },
            { ViewIDs.ConfirmCancelPopupViewID, "UI/Common/ConfirmCancelPopupView" },
            { ViewIDs.ConfirmPopupViewID, "UI/Common/ConfirmPopupView" }
        };

        public ViewsFactory(ResourcesAssetsLoader resourcesAssetsLoader)
        {
            _resourcesAssetsLoader = resourcesAssetsLoader;
        }

        public TView CreateView<TView>(string viewID, Transform parent = null) where TView : MonoBehaviour, IView
        {
            if (_viewIDToResourcesPath.TryGetValue(viewID, out string viewPath) == false)
                throw new ArgumentException($"No view for ID {viewID} (type {typeof(TView)}");

            GameObject viewPrefab = _resourcesAssetsLoader.Load<GameObject>(viewPath);

            GameObject instance = Object.Instantiate(viewPrefab, parent);
            TView view = instance.GetComponent<TView>();

            if (view == null)
                throw new InvalidCastException($"Prefab for view {typeof(TView)} does not contain such component");

            return view;
        }

        public void DestroyView<TView>(TView view) where TView : MonoBehaviour, IView
        {
            Object.Destroy(view.gameObject);
        }
    }
}