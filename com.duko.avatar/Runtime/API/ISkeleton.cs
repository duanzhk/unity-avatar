using UnityEngine;

namespace Duko.Avatar
{
    //******************************************
    // ISkeleton
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-25 17:09
    //******************************************
    public interface ISkeleton
    {
        Transform Find(string boneName);
    }
}