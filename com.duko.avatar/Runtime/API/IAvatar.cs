
using UnityEngine;

namespace Duko.Avatar
{
    //******************************************
    // IAvatar
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-25 16:04
    //******************************************
    public interface IAvatar<C> : IPartManager<C>
    {
        public void SetSkeleton(ISkeleton skeleton);
        
        public ISkeleton GetSkeleton();

        public GameObject GetGameObject();
        public Transform GetTransform();

        void ApplyChange();

        void Destroy();
    }
}