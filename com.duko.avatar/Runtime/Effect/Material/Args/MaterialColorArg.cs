using UnityEngine;

namespace Duko.Avatar
{

    //******************************************
    // MaterialColorArg
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-30 16:00
    //******************************************
    public class MaterialColorArgGeneric : AbstractMaterialArgGeneric<Color>
    {
        public MaterialColorArgGeneric(string propertyName) : base(propertyName)
        {
        }

        public override Color Value { get; set; }
        public override void Apply(Material material)
        {
            material.SetColor(PropertyID, Value);
        }
    }
    
    public class MaterialColorArg : AbstractMaterialArg
    {
        public MaterialColorArg(string propertyName) : base(propertyName)
        {
        }

        public override object Value { get; set; }
        public override void Apply(Material material)
        {
            material.SetColor(PropertyID, (Color)Value);
        }
    }
}