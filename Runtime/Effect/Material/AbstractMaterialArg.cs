using System.Collections.Generic;
using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // AbstractMaterialArg
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 11:22
    //******************************************
    public abstract class AbstractMaterialArgGeneric<T> : IMaterialArgGeneric<T>
    {
        public string PropertyName { get; }
        public int PropertyID { get; }
        public abstract T Value { get; set; }
        public abstract void Apply(Material material);

        public AbstractMaterialArgGeneric(string propertyName)
        {
            PropertyName = propertyName;
            PropertyID = Shader.PropertyToID(propertyName);
        }
    }
    
    public abstract class AbstractMaterialArg : AbstractMaterialArgGeneric<object>, IMaterialArg
    {
        protected AbstractMaterialArg(string propertyName) : base(propertyName)
        {
        }
    }
}