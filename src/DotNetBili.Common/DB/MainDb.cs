namespace DotNetBili.Common.DB
{
    public static class MainDb
    {
        /// <summary>
        /// 当前使用的数据库连接字符串ID，默认为Main，使用前请确保已在appsettings.json中配置了该ID的连接字符串
        /// </summary>
        public static string CurrentDbConId = "Main";
    }
}
