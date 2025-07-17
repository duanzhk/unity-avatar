using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duko.Avatar.Sample
{

    //******************************************
    // ConfigKindItem
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:12
    //******************************************
    public class ConfigKindItem
    {
        public string PrefabName;
        public List<IConfigEffect> ConfigEffect;

        public ConfigKindItem(string prefabName, List<IConfigEffect> configEffect = null)
        {
            PrefabName = prefabName;
            ConfigEffect = configEffect;
        }

        public string Name => PrefabName.Substring(PrefabName.LastIndexOf('/') + 1);

        public GameObject LoadPrefab()
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>(PrefabName);
        }
    }
}