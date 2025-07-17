using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar.Sample
{

    //******************************************
    // ConfigEffect4Material
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:13
    //******************************************
    public enum PropertyType
    {
        Float,
        Vector3,
        Color
    }
    
    public class ConfigEffect4Material : IConfigEffect
    {
        public class ConfigArg
        {
            public string Property;
            public PropertyType PropertyType;
            public object DefaultValue;
            
            public ConfigArg(string property, PropertyType propertyType, object defaultValue)
            {
                Property = property;
                PropertyType = propertyType;
                DefaultValue = defaultValue;
            }
        }

        public ConfigArg[] Args;

        public ConfigEffect4Material(ConfigArg[] args)
        {
            Args = args;
        }

        public static IEffect Creator(IConfigEffect config, GameObject go)
        {
            ConfigEffect4Material configEffect4Material = config as ConfigEffect4Material;
            var materialEffect = new MaterialEffect(go);
            var args = configEffect4Material.Args;
            for (int i = 0; i < args.Length; i++)
            {
                materialEffect.AddMaterialArg(MaterialPropertyRegister.Create(args[i]));
            }
            return materialEffect;
        }

        public static void Render(IConfigEffect config, IEffect effect)
        {
            var materialEffect = effect as MaterialEffect;
            var configEffect4Material = config as ConfigEffect4Material;

            var args = materialEffect.GetMaterialArgs();
            var configArgs = configEffect4Material.Args;
            bool changed = false;
            for (int i = 0; i < configArgs.Length; i++)
            {
                changed |= MaterialPropertyRegister.Render(effect, configArgs[i], args[i]);
            }

            if (changed)
            {
                effect.Apply();    
            }
        }
    }
}