using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // MaterialFloatArg
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 11:21
    //******************************************
    public class MaterialFloatArgGeneric : AbstractMaterialArgGeneric<float>
    {
        public MaterialFloatArgGeneric(string propertyName) : base(propertyName)
        {
        }
        public override float Value { get; set; }
        public override void Apply(Material material)
        {
            material.SetFloat(PropertyID, Value);
        }
    }
    
    public class MaterialFloatArg : AbstractMaterialArg
    {
        public MaterialFloatArg(string propertyName) : base(propertyName)
        {
        }

        public override object Value { get; set; }
        public override void Apply(Material material)
        {
            material.SetFloat(PropertyID, (float)Value);
        }
    }
}