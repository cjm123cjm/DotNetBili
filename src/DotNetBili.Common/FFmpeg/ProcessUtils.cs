using DotNetBili.Model.CustomerException;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace DotNetBili.Common.FFmpeg
{
    public static class ProcessUtils
    {
        // 日志对象（.NET Core/5+ 标准日志）
        private static readonly ILogger _logger = LoggerFactory.Create(builder => { }).CreateLogger("ProcessUtils");

        // 操作系统类型
        private static readonly string _osName = Environment.OSVersion.ToString().ToLower();

        /// <summary>
        /// 执行系统命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <param name="showLog">是否打印日志</param>
        /// <returns>执行结果</returns>
        /// <exception cref="BusinessException"></exception>
        public static string ExecuteCommand(string cmd, bool showLog)
        {
            if (string.IsNullOrWhiteSpace(cmd))
                return null;

            Process process = null;
            try
            {
                ProcessStartInfo startInfo;

                // Windows 系统
                if (_osName.Contains("win"))
                {
                    startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c {cmd}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    };
                }
                // Linux/Mac 系统
                else
                {
                    startInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/sh",
                        Arguments = $"-c \"{cmd}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        StandardOutputEncoding = Encoding.UTF8,
                        StandardErrorEncoding = Encoding.UTF8
                    };
                }

                process = Process.Start(startInfo);

                // 异步读取输出流和错误流
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // 等待进程执行完成
                process.WaitForExit();

                // 拼接结果（错误流+标准输出）
                string result = $"{error}\n{output}";

                if (showLog)
                {
                    _logger.LogInformation("执行命令 {Cmd} 结果：{Result}", cmd, result);
                }

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "执行命令失败 {Cmd}", cmd);
                throw new BusinessException("视频转换失败");
            }
            finally
            {
                if (process != null)
                {
                    // 注册程序退出钩子，销毁进程（对应Java addShutdownHook）
                    AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                    {
                        try
                        {
                            if (!process.HasExited)
                            {
                                process.Kill();
                            }
                            process.Dispose();
                        }
                        catch
                        {
                            // 忽略销毁异常
                        }
                    };
                }
            }
        }
    }
}
