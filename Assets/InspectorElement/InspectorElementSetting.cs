using System;
using UnityEngine;

namespace XFramework.UI
{
    [CreateAssetMenu(fileName = "UIItem Settings", menuName = "SFramework/UIItem/Settings", order = 111)]
    public class InspectorElementSetting : ScriptableObject
    {
        [SerializeField] private InspectorElement[] m_DefaultDrawers;

        //public Type[] DefaultDrawers => m_DefaultDrawers;
    }
}