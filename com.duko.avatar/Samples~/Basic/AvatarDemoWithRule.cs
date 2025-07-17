using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar.Sample
{
    /// <summary>
    /// Basic usage of Avatar with rule
    /// </summary>
    public class AvatarDemoWithRule : MonoBehaviour
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
        public const int PART_TYPE_SUIT_FULL = 9;
        
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
            _Avatar = new Avatar<int>(new BasicAvatar<int>(), _CreateAvatarRule(), _CreateLifeCycle());
            _Avatar.GetGameObject().transform.SetParent(Parent, false);
            foreach (var kind in _Kinds)
            {
                _SetDefaultAvatar(kind);
            }
            
            // init drag function
            gameObject.AddComponent<RotateViaDrag>().RotateTarget = _Avatar.GetTransform();
        }

        private IAvatarRule<int> _CreateAvatarRule()
        {
            return new BasicAvatarRule<int>()
                .AddMute(PART_TYPE_HAIR, PART_TYPE_HAIR)
                .AddMute(PART_TYPE_BODY, PART_TYPE_BODY)
                .AddMute(PART_TYPE_CLOTH, PART_TYPE_CLOTH, PART_TYPE_SUIT_FULL)
                .AddMute(PART_TYPE_PANTS, PART_TYPE_PANTS, PART_TYPE_SUIT_FULL)
                .AddMute(PART_TYPE_SHOE, PART_TYPE_SHOE)
                .AddMute(PART_TYPE_GLOVE, PART_TYPE_GLOVE)
                .AddMute(PART_TYPE_HAT, PART_TYPE_HAT)
                .AddMute(PART_TYPE_OTHER, PART_TYPE_OTHER)
                .AddMute(PART_TYPE_SUIT_FULL, PART_TYPE_SUIT_FULL, PART_TYPE_CLOTH, PART_TYPE_PANTS);
        }

        private IAvatarLifeCycle<int> _CreateLifeCycle()
        {
            return new DelegateLifeCycle<int>()
                .AddOnAddPartHandler(((avatar, part) => Debug.Log($"Add part {part.Config}")))
                .AddOnRemovePartHandler(((avatar, part) =>
                {
                    Debug.Log($"Remove part {part.Config}");
                    part.Destroy();
                }))
                .AddOnChangeSkeletonHandler(((avatar, oldSkeleton, skeleton) => Debug.Log($"Change Skeleton old={oldSkeleton} new={skeleton}")));
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
                    ConfigKinds.CreateSuitChild(PART_TYPE_SUIT_FULL, "Suit", "Cloth", "Pants", true),
                    ConfigKinds.CreateChild(PART_TYPE_OTHER, "Other", KindType.Part, true)
                })
            };
        }

        private void _SetDefaultAvatar(ConfigKinds configKind)
        {
            if (configKind.SubKinds != null)
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
                usingInfo.Clear();
            }
            else
            {
                usingInfo = new UsingInfo();
                _Kind2Using.Add(configKind, usingInfo);
            }
            
            if (configKind.KindType == KindType.Skeleton)
            {
                var go = _UseSinglePart(usingInfo, configKind, index);
                _Avatar.SetSkeleton(new BasicSkeleton(go.transform));
            }
            else if (configKind.KindType == KindType.Part)
            {
                var go = _UseSinglePart(usingInfo, configKind, index);
                if (go != null)
                {
                    _Avatar.AddPart(configKind.Type, new SkinnedMeshRendererPart<int>(configKind.Type * 1000 + index, go));    
                }
            }
            else if (configKind.KindType == KindType.Suit)
            {
                var gos = _UseMultiParts(usingInfo, configKind, index);
                if (gos != null)
                {
                    _Avatar.AddPart(configKind.Type, new MultiSkinnedMeshRendererPart<int>(configKind.Type * 1000 + index, gos));    
                }
            }
            else
            {
                throw new Exception($"Unsupported KindType {configKind.KindType}");
            }
            _Avatar.ApplyChange();
        }

        private GameObject[] _UseMultiParts(UsingInfo usingInfo, ConfigKinds configKind, int index)
        {
            var configKindConfigSuitItem = configKind.ConfigSuitItems[index];
            if (configKindConfigSuitItem == null)
            {
                return null;
            }
            GameObject[] gos = new GameObject[configKindConfigSuitItem.Length];
            for (int i = 0; i < configKindConfigSuitItem.Length; i++)
            {
                var configKindItem = configKindConfigSuitItem[i];
                var go = configKindItem == null ? null : Instantiate(configKindItem.LoadPrefab());
                if (go)
                {
                    go.name = configKind.Title + "_" + configKindItem.Name;
                    go.transform.SetParent(_Avatar.GetTransform(), false);    
                }
                gos[i] = go;
            }
            usingInfo.Gos = gos;
            usingInfo.Configs = configKindConfigSuitItem;
            usingInfo.Index = index;
            usingInfo.ReCreateEffect();  
            return gos;
        }

        private GameObject _UseSinglePart(UsingInfo usingInfo, ConfigKinds configKind, int index)
        {
            var configKindItem = configKind.ConfigItems[index];
            var go = configKindItem == null ? null : Instantiate(configKindItem.LoadPrefab());
            if (go)
            {
                go.name = configKind.Title + "_" + configKindItem.Name;
                go.transform.SetParent(_Avatar.GetTransform(), false);    
            }
            usingInfo.Go = go;
            usingInfo.Config = configKindItem;
            usingInfo.Index = index;
            usingInfo.ReCreateEffect();
            return go;
        }
    }
}