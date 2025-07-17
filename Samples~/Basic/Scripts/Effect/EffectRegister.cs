
using System;
using System.Collections.Generic;
using Duko.Avatar.Sample;
using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // EffectManager
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 18:54
    //******************************************
    public class EffectRegister
    {
        public delegate IEffect Creator(IConfigEffect config, GameObject go);
        public delegate void Renderer(IConfigEffect config, IEffect effect);

        private static Dictionary<Type, Creator> _EffectCreators = new Dictionary<Type, Creator>();
        private static Dictionary<Type, Renderer> _EffectRenderers = new Dictionary<Type, Renderer>();

        static EffectRegister()
        {
            Register<ConfigEffect4Material>(ConfigEffect4Material.Creator, ConfigEffect4Material.Render);
        }

        public static IEffect Create(IConfigEffect configEffect, GameObject go)
        {
            var type = configEffect.GetType();
            if (_EffectCreators.TryGetValue(type, out Creator creator))
            {
                return creator(configEffect, go);
            }
            throw new Exception($"Can not found creator for such ConfigEffect type=${type}.Invoke {nameof(Register)} for it first.");
        }

        public static void Render(IConfigEffect configEffect, IEffect effect)
        {
            var type = configEffect.GetType();
            if (_EffectRenderers.TryGetValue(type, out Renderer renderer))
            {
                renderer(configEffect, effect);
                return;
            }
            throw new Exception($"Can not found renderer for such ConfigEffect type=${type}.Invoke {nameof(Register)} for it first.");
        }

        public static void Register<T>(Creator creator, Renderer renderer) where T : IConfigEffect
        {
            var configEffectType = typeof(T);
            _EffectCreators.Add(configEffectType, creator);
            _EffectRenderers.Add(configEffectType, renderer);
        }
    }
}