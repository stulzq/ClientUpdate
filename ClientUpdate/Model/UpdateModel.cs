
namespace ClientUpdate.Model
{
    /// <summary>
    /// 此类主要用来存储更新信息 方便全局存取
    /// </summary>
    public static class UpdateModel
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public static string Version;

        /// <summary>
        /// 更新内容url
        /// </summary>
        public static string ContentUrl;

        /// <summary>
        /// 更新内容
        /// </summary>
        public static string Content;

        /// <summary>
        /// 更新包url
        /// </summary>
        public static string FileUrl;

        /// <summary>
        /// 更新完毕需要启动的程序（本程序更目录 *.exe 不用加exe）
        /// </summary>
        public static string Start;

        /// <summary>
        /// 更新完后需要删除的文件 多个文件 英文逗号分隔
        /// </summary>
        public static string Delete;

        /// <summary>
        /// 脚本url
        /// </summary>
        public static string ScriptUrl;

        /// <summary>
        /// 脚本公钥
        /// </summary>
        public static string ScriptKey;

        /// <summary>
        /// 更新包存放路径
        /// </summary>
        public static string UpdateFilePath;

    }
}
