using System.Collections.Generic;

namespace Duko.Avatar
{
    //******************************************
    // IPartManager
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-28 11:26
    //******************************************
    public interface IPartManager<C> : IEnumerable<IAvatarPart<C>>
    {
        public void AddPart(int partType, IAvatarPart<C> part);

        public IAvatarPart<C> RemovePart(int partType);

        public IAvatarPart<C> GetPart(int partType);
    }
}