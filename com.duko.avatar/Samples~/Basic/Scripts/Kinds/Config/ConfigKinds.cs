using System;
using System.Collections.Generic;
using UnityEditor;

namespace Duko.Avatar.Sample
{

    //******************************************
    // ConfigKinds
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:11
    //******************************************
    public enum KindType
    {
        Skeleton,
        Part,
        Suit
    }
    
    public class ConfigKinds
    {
        public int Type;
        public string Title;
        public ConfigKindItem[] ConfigItems;
        public List<ConfigKindItem[]> ConfigSuitItems;
        public ConfigKinds[] SubKinds;
        public KindType KindType;
        public int DefaultIndex;
        public bool AllowEmpty;

        public static ConfigKinds CreateParent(string title, ConfigKinds[] subKinds)
        {
            var kinds = new ConfigKinds();
            kinds.Title = title;
            kinds.SubKinds = subKinds;
            return kinds;
        }
            
        public static ConfigKinds CreateChild(int type, string title, KindType kindType = KindType.Part, bool allowEmpty = false, int defaultIndex = 0, ConfigKindItem[] items = null, string prefabDirectory = null)
        {
            var kinds = new ConfigKinds();
            kinds.Type = type;
            kinds.Title = title;
            kinds.AllowEmpty = allowEmpty;
            kinds.KindType = kindType;
            kinds.DefaultIndex = defaultIndex;
            kinds.ConfigItems = _Create(prefabDirectory ?? title, allowEmpty, items);
            return kinds;
        }
            
        public static ConfigKinds CreateSuitChild(int type, string title, string prefabDir1, string prefabDir2, bool allowEmpty = false, int defaultIndex = 0)
        {
            var kinds = new ConfigKinds();
            kinds.Type = type;
            kinds.Title = title;
            kinds.AllowEmpty = allowEmpty;
            kinds.KindType = KindType.Suit;
            kinds.DefaultIndex = defaultIndex;
            
            kinds.ConfigSuitItems = new List<ConfigKindItem[]>();
            if (allowEmpty)
            {
                kinds.ConfigSuitItems.Add(null);    
            }

            var configKindItems1 = _Create(prefabDir1, false, null);
            var configKindItems2 = _Create(prefabDir2, false, null);

            for (int i = 0; i < configKindItems1.Length; i++)
            {
                for (int j = 0; j < configKindItems2.Length; j++)
                {
                    kinds.ConfigSuitItems.Add(new [] { configKindItems1[i], configKindItems2[j] });
                }   
            }
            
            return kinds;
        }

        private static ConfigKindItem[] _Create(string path, bool allowEmpty, ConfigKindItem[] items)
        {
            path = "Assets/Duko/Avatar/Samples/Basic/DemoResources/R/" + path;
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].PrefabName = path + "/" + items[i].PrefabName;
                }
                if (allowEmpty)
                {
                    ConfigKindItem[] newItems = new ConfigKindItem[items.Length + 1];
                    Array.Copy(items, 0, newItems, 1, items.Length);
                    items = newItems;
                }
                return items;
            }
            
            var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new []{ path });
            items = new ConfigKindItem[prefabGUIDs.Length + (allowEmpty ? 1 : 0)];
            
            for (int i = 0; i < prefabGUIDs.Length; i++)
            {
                items[i + (allowEmpty ? 1 : 0)] = new ConfigKindItem(AssetDatabase.GUIDToAssetPath(prefabGUIDs[i]));
            }
            return items;
        }
    }
}