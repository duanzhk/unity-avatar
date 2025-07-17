using System;
using System.Collections.Generic;
using Duko.Avatar.Sample;
using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // MaterialPropertyRegister
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-30 16:02
    //******************************************
    public 

    class MaterialPropertyRegister
    {
        public delegate IMaterialArg Creator(ConfigEffect4Material.ConfigArg configArg);
        public delegate bool Renderer(IEffect effect, ConfigEffect4Material.ConfigArg configArg, IMaterialArg arg);

        private static Dictionary<PropertyType, Creator> _Creators = new Dictionary<PropertyType, Creator>();
        private static Dictionary<PropertyType, Renderer> _Renderers = new Dictionary<PropertyType, Renderer>();

        static MaterialPropertyRegister()
        {
            Register(PropertyType.Float, (config) => new MaterialFloatArg(config.Property), _FloatRenderer);
            Register(PropertyType.Vector3, (config) => new MaterialVectorArg(config.Property), _Vector3Renderer);
            Register(PropertyType.Color, (config) => new MaterialColorArg(config.Property), _ColorRenderer);
        }

        private static bool _FloatRenderer(IEffect effect, ConfigEffect4Material.ConfigArg configArg, IMaterialArg arg)
        {
            MaterialFloatArg materialFloatArg = arg as MaterialFloatArg;
            float oldValue = (float)materialFloatArg.Value;
            GUILayout.BeginHorizontal();
            GUILayout.Box(configArg.Property);
            float newValue = GUILayout.HorizontalSlider(oldValue, 0, 10);
            GUILayout.EndHorizontal();
            if (!Mathf.Approximately(newValue, oldValue))
            {
                materialFloatArg.Value = newValue;
                return true;
            }
            return false;
        }

        private static bool _Vector3Renderer(IEffect effect, ConfigEffect4Material.ConfigArg configArg, IMaterialArg arg)
        {
            MaterialVectorArg materialVectorArg = arg as MaterialVectorArg;
            Vector3 oldValue = (Vector3)materialVectorArg.Value;
            GUILayout.Box(configArg.Property);
            GUILayout.BeginHorizontal();
            GUILayout.Label("x");
            float newX = GUILayout.HorizontalSlider(oldValue.x, -1, 1);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("y");
            float newY = GUILayout.HorizontalSlider(oldValue.y, -1, 1);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("z");
            float newZ = GUILayout.HorizontalSlider(oldValue.z, -1, 1);
            GUILayout.EndHorizontal();

            var newValue = new Vector3(newX, newY, newZ);
            if (!Mathf.Approximately(Vector3.Distance(newValue, oldValue), 0))
            {
                materialVectorArg.Value = newValue;
                return true;
            }
            return false;
        }

        private static bool _ColorRenderer(IEffect effect, ConfigEffect4Material.ConfigArg configArg, IMaterialArg arg)
        {
            MaterialColorArg materialColorArg = arg as MaterialColorArg;
            Color oldValue = (Color)materialColorArg.Value;
            GUILayout.Box(configArg.Property);
            GUILayout.BeginHorizontal();
            GUILayout.Label("r");
            float newR = GUILayout.HorizontalSlider(oldValue.r, 0, 1);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("y");
            float newG = GUILayout.HorizontalSlider(oldValue.g, 0, 1);
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label("z");
            float newB = GUILayout.HorizontalSlider(oldValue.b, 0, 1);
            GUILayout.EndHorizontal();

            var newValue = new Color(newR, newG, newB);
            if (!(Mathf.Approximately(newValue.r, oldValue.r) && Mathf.Approximately(newValue.g, oldValue.g) && Mathf.Approximately(newValue.b, oldValue.b)))
            {
                materialColorArg.Value = newValue;
                return true;
            }
            return false;
        }

        public static void Register(PropertyType propertyType, Creator creator, Renderer renderer)
        {
            _Creators.Add(propertyType, creator);
            _Renderers.Add(propertyType, renderer);
        }

        public static IMaterialArg Create(ConfigEffect4Material.ConfigArg configArg)
        {
            if (_Creators.ContainsKey(configArg.PropertyType))
            {
                var arg = _Creators[configArg.PropertyType](configArg);
                arg.Value = configArg.DefaultValue;
                return arg;
            }
            throw new Exception($"Unsupported PropertyType={configArg.PropertyType}, use {nameof(MaterialPropertyRegister)}.{nameof(Register)} first.");
        }

        public static bool Render(IEffect effect, ConfigEffect4Material.ConfigArg configArg, IMaterialArg arg)
        {
            if (_Renderers.ContainsKey(configArg.PropertyType))
            {
                return _Renderers[configArg.PropertyType](effect, configArg, arg);
            }
            throw new Exception($"Unsupported PropertyType={configArg.PropertyType}, use {nameof(MaterialPropertyRegister)}.{nameof(Register)} first.");
        }
    }
}