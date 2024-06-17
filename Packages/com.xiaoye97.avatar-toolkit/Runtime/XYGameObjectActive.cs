#if UNITY_EDITOR
namespace xiaoye97.AvatarToolkit
{
    /// <summary>
    /// 在构建时设置当前物体的激活
    /// 比如某些物体做的时候不想看见,但是打包的时候想默认开着
    /// </summary>
    public class XYGameObjectActive : XYBaseComponent
    {
        public bool Active;

        public override void Generate(XYAvatarPlugin plugin)
        {
            gameObject.SetActive(Active);
        }
    }
}
#endif