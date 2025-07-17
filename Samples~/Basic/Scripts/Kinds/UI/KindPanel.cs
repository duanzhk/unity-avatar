using UnityEngine;

namespace Duko.Avatar.Sample
{

    //******************************************
    // KindPanel
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:41
    //******************************************
    public class KindPanel : IUI
    {
        public delegate void OnClick(ConfigKinds configKind, int index);
        public delegate bool IsUsing(ConfigKinds configKind, int index);
        
        private ConfigKinds[] _Kinds;
        private IsUsing _IsUsing;
        private event OnClick _OnClick = (ConfigKinds configKind, int index) => { };

        public KindPanel(ConfigKinds[] kinds, IsUsing isUsing)
        {
            _Kinds = kinds;
            _IsUsing = isUsing;
        }

        public void Render()
        {
            for (int i = 0; i < _Kinds.Length; i++)
            {
                _RenderKind(_Kinds[i], 0);
            }
        }

        public KindPanel AddOnClick(OnClick onClick)
        {
            _OnClick += onClick;
            return this;
        }

        public KindPanel RemoveOnClick(OnClick onClick)
        {
            _OnClick -= onClick;
            return this;
        }

        private void _RenderKind(ConfigKinds configKind, int indent)
        {
            if (configKind.ConfigItems != null)
            {
                GUILayout.BeginVertical();
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent * 40);
                GUILayout.Box(configKind.Title);
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Space((indent + 1) * 40);
                for (int i = 0; i < configKind.ConfigItems.Length; i++)
                {
                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    if (_IsUsing(configKind, i))
                    {
                        style.normal.textColor = Color.green;
                    }

                    if (GUILayout.Button(configKind.ConfigItems[i] == null ? "None" : configKind.ConfigItems[i].Name, style))
                    {
                        _OnClick(configKind, i);
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            else if (configKind.ConfigSuitItems != null)
            {
                GUILayout.BeginVertical();
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent * 40);
                GUILayout.Box(configKind.Title);
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                GUILayout.Space((indent + 1) * 40);
                for (int i = 0; i < configKind.ConfigSuitItems.Count; i++)
                {
                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    if (_IsUsing(configKind, i))
                    {
                        style.normal.textColor = Color.green;
                    }

                    if (GUILayout.Button(configKind.ConfigSuitItems[i] == null ? "None" : "S#" + i, style))
                    {
                        _OnClick(configKind, i);
                    }
                }
                GUILayout.EndHorizontal();
                
                GUILayout.EndVertical();
            }
            else if(configKind.SubKinds != null)
            {
                GUILayout.BeginVertical();
                
                GUILayout.BeginHorizontal();
                GUILayout.Space(indent * 40);
                GUILayout.Box(configKind.Title);
                GUILayout.EndHorizontal();
                
                for (int i = 0; i < configKind.SubKinds.Length; i++)
                {
                    _RenderKind(configKind.SubKinds[i], indent + 1);
                }
                
                GUILayout.EndVertical();
            }
        }
    }
}