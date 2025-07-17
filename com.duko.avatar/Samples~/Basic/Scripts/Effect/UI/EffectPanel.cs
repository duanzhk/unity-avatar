using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar.Sample
{

    //******************************************
    // EffectPanel
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 17:54
    //******************************************
    public class EffectPanel : IUI
    {
        private Dictionary<ConfigKinds, UsingInfo> _Kind2Using;
        
        public EffectPanel(Dictionary<ConfigKinds, UsingInfo> kind2Using)
        {
            _Kind2Using = kind2Using;
        }

        public void Render()
        {
            GUILayout.BeginVertical();
            foreach (var usingInfo in _Kind2Using)
            {
                _RenderEffect(usingInfo.Key, usingInfo.Value);
            }
            GUILayout.EndVertical();
        }

        private void _RenderEffect(ConfigKinds config, UsingInfo usingInfo)
        {
            if (usingInfo.Effects.Count == 0)
            {
                return;
            }
            
            GUILayout.Box(config.Title);

            for (int i = 0; i < usingInfo.Effects.Count; i++)
            {
                var configEffect = usingInfo.Config.ConfigEffect[i];
                var effect = usingInfo.Effects[i];
                EffectRegister.Render(configEffect, effect);
            }
        }
    }
}