using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar.Sample
{

    //******************************************
    // UsingInfo
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-08-01 18:07
    //******************************************
    public class UsingInfo
    {
        private GameObject _Go;

        public GameObject Go {
            get { return _Go; }
            set
            {
                _Go = value;
            }
        }
        
        private GameObject[] _Gos;

        public GameObject[] Gos {
            get { return _Gos; }
            set
            {
                _Gos = value; 
            }
        }

        public ConfigKindItem Config { get; set; }
        public ConfigKindItem[] Configs { get; set; }

        public List<IEffect> Effects = new List<IEffect>();

        public int Index;

        public void ReCreateEffect()
        {
            Effects.Clear();
            if (Go != null && Config != null)
            {
                _ReCreateEffect(Go, Config);
            }

            if (_Gos != null && _Gos.Length > 0 && Configs != null && Configs.Length > 0)
            {
                for (int i = 0; i < _Gos.Length; i++)
                {
                    _ReCreateEffect(_Gos[i], Configs[i]);
                }
            }
        }

        private void _ReCreateEffect(GameObject go, ConfigKindItem config)
        {
            if (config == null || config.ConfigEffect == null)
            {
                return;
            }

            var configEffects = config.ConfigEffect;
            for (int i = 0; i < configEffects.Count; i++)
            {
                var effect = EffectRegister.Create(configEffects[i], go);
                Effects.Add(effect);
            }
        }

        public void Destroy()
        {
            if (_Go != null)
            {
                GameObject.Destroy(_Go);
                _Go = null;
            }

            if (_Gos != null && _Gos.Length > 0)
            {
                for (int i = 0; i < _Gos.Length; i++)
                {
                    GameObject.Destroy(_Gos[i]);    
                }
                _Gos = null;
            }
        }
        public void Clear()
        {
            _Go = null;
            _Gos = null;
        }
    }
}