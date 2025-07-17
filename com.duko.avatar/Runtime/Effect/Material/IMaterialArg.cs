
using UnityEngine;

namespace Duko.Avatar
{
    //******************************************
    // IMaterialArg
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-29 11:17
    //******************************************
    public interface IMaterialArgGeneric<T>
    {
        string PropertyName { get; }
        int PropertyID { get; }
        T Value { get; set; }
        void Apply(Material material);
    }
    
    public interface IMaterialArg : IMaterialArgGeneric<object>
    {
    }
}