namespace Duko.Avatar
{
    //******************************************
    // IAvatarRule
    //
    // @Author: duanzhk
    // @Email: dzk_dzk@163.com
    // @Date: 2022-08-01 10:30
    //******************************************
    public interface IAvatarRule<C>
    {
        int[] GetMuteParts(int partType, IAvatarPart<C> part);
    }
}