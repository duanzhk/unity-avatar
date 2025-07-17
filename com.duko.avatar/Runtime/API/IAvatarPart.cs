namespace Duko.Avatar
{
    //******************************************
    // IAvatarPart
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-07-25 16:13
    //******************************************
    public interface IAvatarPart<C>
    {
        C Config { get; }
        
        void Apply(IAvatar<C> avatar);

        void Destroy();
    }
}