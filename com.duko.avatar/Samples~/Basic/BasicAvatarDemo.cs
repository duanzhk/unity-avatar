using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar.Sample
{
    /// <summary>
    /// Usage of BasicAvatar
    /// </summary>
    public class BasicAvatarDemo : MonoBehaviour
    {
        public const int PART_TYPE_SKELETON = 0;
        public const int PART_TYPE_HAIR = 1;
        public const int PART_TYPE_BODY = 2;
        public const int PART_TYPE_CLOTH = 3;
        public const int PART_TYPE_PANTS = 4;
        public const int PART_TYPE_SHOE = 5;
        public const int PART_TYPE_GLOVE = 6;
        public const int PART_TYPE_HAT = 7;
        public const int PART_TYPE_OTHER = 8;
        
        public Transform Parent;
        
        private ConfigKinds[] _Kinds;
        private IAvatar<int> _Avatar;
        private Dictionary<ConfigKinds, UsingInfo> _Kind2Using = new Dictionary<ConfigKinds, UsingInfo>();
        private List<IUI> _UIs;

        private void Awake()
        {
            // init config
            _Kinds = _MockConfig();
            
            // init ui
            _UIs = new List<IUI>
            {
                new KindPanel(_Kinds, _IsUsing).AddOnClick(_Use),
                new EffectPanel(_Kind2Using)
            };
            
            // init avatar
            _Avatar = new BasicAvatar<int>();
            _Avatar.GetGameObject().transform.SetParent(Parent, false);
            foreach (var kind in _Kinds)
            {
                _SetDefaultAvatar(kind);
            }
            
            // init drag function
            gameObject.AddComponent<RotateViaDrag>().RotateTarget = _Avatar.GetTransform();
        }

        private ConfigKinds[] _MockConfig()
        {
            // NOTE!!! Please read from config in production environment
                
            return new []
            {
                ConfigKinds.CreateChild(PART_TYPE_SKELETON, "Skeleton", KindType.Skeleton),
                ConfigKinds.CreateChild(PART_TYPE_HAIR, "Hair", KindType.Part, true, 1),
                ConfigKinds.CreateChild(PART_TYPE_BODY, "Body", KindType.Part, false, 0, new ConfigKindItem[]
                {
                    new ConfigKindItem("001.prefab", new List<IConfigEffect>
                    {
                        new ConfigEffect4Material(new []{new ConfigEffect4Material.ConfigArg("_Color", PropertyType.Color, new Color(0.5f, 0.5f, 0.5f))})
                    })
                }),
                ConfigKinds.CreateChild(PART_TYPE_CLOTH, "Cloth"),
                ConfigKinds.CreateChild(PART_TYPE_PANTS, "Pants"),
                ConfigKinds.CreateChild(PART_TYPE_SHOE, "Shoe"),
                ConfigKinds.CreateParent( "Decorator", new []
                {
                    ConfigKinds.CreateChild(PART_TYPE_GLOVE, "Glove", KindType.Part, true, 0, new []
                    {
                        new ConfigKindItem("001.prefab", new List<IConfigEffect>
                        {
                            new ConfigEffect4Material(new ConfigEffect4Material.ConfigArg[]
                            {
                                new ConfigEffect4Material.ConfigArg("_Color", PropertyType.Color, new Color(0.5f, 0.5f, 0.5f))
                            })
                        }),
                        new ConfigKindItem("002.prefab"),
                        new ConfigKindItem("003.prefab")
                    }),
                    ConfigKinds.CreateChild(PART_TYPE_HAT, "Hat", KindType.Part, true, 0, new ConfigKindItem[]
                    {
                        new ConfigKindItem("001.prefab", new List<IConfigEffect>()
                        {
                            new ConfigEffect4Material(new []
                            {
                                new ConfigEffect4Material.ConfigArg("_Scale", PropertyType.Float, 1f),
                                new ConfigEffect4Material.ConfigArg("_Offset", PropertyType.Vector3, new Vector3(0,0,0)),
                            })
                        }),
                        new ConfigKindItem("002.prefab", new List<IConfigEffect>()
                        {
                            new ConfigEffect4Material(new []{ new ConfigEffect4Material.ConfigArg("_Scale", PropertyType.Float, 1f) })
                        })
                    }),
                    ConfigKinds.CreateChild(PART_TYPE_OTHER, "Other", KindType.Part, true)
                })
            };
        }

        private void _SetDefaultAvatar(ConfigKinds configKind)
        {
            if (configKind.ConfigItems == null)
            {
                foreach (var subKind in configKind.SubKinds)
                {
                    _SetDefaultAvatar(subKind);
                }
            }
            else
            {
                _Use(configKind, configKind.DefaultIndex);
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            for (int i = 0; i < _UIs.Count; i++)
            {
                _UIs[i].Render();
                GUILayout.Space(20);
            }
            GUILayout.EndVertical();
        }

        private bool _IsUsing(ConfigKinds configKind, int index)
        {
            return _Kind2Using.ContainsKey(configKind) && _Kind2Using[configKind].Index == index;
        }

        private void _Use(ConfigKinds configKind, int index)
        {
            UsingInfo usingInfo;
            if (_Kind2Using.ContainsKey(configKind))
            {
                usingInfo = _Kind2Using[configKind];
                GameObject.Destroy(usingInfo.Go);
            }
            else
            {
                usingInfo = new UsingInfo();
                _Kind2Using.Add(configKind, usingInfo);
            }

            var prefab = configKind.ConfigItems[index];
            var go = prefab == null ? null : Instantiate(prefab.LoadPrefab());
            if (go)
            {
                go.transform.SetParent(_Avatar.GetTransform(), false);    
            }
            usingInfo.Go = go;
            usingInfo.Config = prefab;
            usingInfo.Index = index;
            usingInfo.ReCreateEffect();
            
            if (configKind.KindType == KindType.Skeleton)
            {
                _Avatar.SetSkeleton(new BasicSkeleton(go.transform));
            }
            else if (configKind.KindType == KindType.Part)
            {
                _Avatar.AddPart(configKind.Type, new SkinnedMeshRendererPart<int>(0, go));
            }
            else
            {
                throw new Exception($"Unsupported KindType {configKind.KindType}");
            }
            _Avatar.ApplyChange();
        }
    }
}