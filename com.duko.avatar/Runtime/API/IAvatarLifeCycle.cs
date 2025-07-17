namespace Duko.Avatar
{
    //******************************************
    // IAvatarLifeCycle
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-08-01 10:32
    //******************************************
    public interface IAvatarLifeCycle<C>
    {
        void OnRemovePart(IAvatar<C> avatar, IAvatarPart<C> removedPart);
        void OnAddPart(IAvatar<C> avatar, IAvatarPart<C> addedPart);
        void OnChangeSkeleton(IAvatar<C> avatar, ISkeleton oldSkeleton, ISkeleton skeleton);
    }
}