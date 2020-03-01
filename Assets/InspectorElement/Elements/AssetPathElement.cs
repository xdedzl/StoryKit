using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XFramework.UI
{
    public class AssetPathElement<T> : InspectorElement where T : Object
    {
        protected ObjectField assetFiled;
        //private Image preview;

        public AssetPathElement()
        {
            this.AddToClassList("asset-path-element");

            assetFiled = new ObjectField
            {
                objectType = typeof(T),
            };
            assetFiled.AddToClassList("input");
            this.Add(assetFiled);

            assetFiled.RegisterValueChangedCallback((v) =>
            {
                T asset = v.newValue as T;
                SetObjValue(asset);
            });
        }

        protected override void OnBound()
        {
            base.OnBound();

            var tex = AssetDatabase.LoadAssetAtPath<T>(Value as string);
            SetObjValue(tex);
        }

        private void SetObjValue(T asset)
        {
            if (asset != null)
            {
                string path = AssetDatabase.GetAssetPath(asset);
                Value = path;
            }
            else
            {
                Value = null;
            }

            OnAssetChange(asset);
        }

        protected virtual void OnAssetChange(T asset)
        {

        }
    }
}