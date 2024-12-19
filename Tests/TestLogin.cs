using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class TestLogin
    {
        private readonly ITestOutputHelper _output;

        public TestLogin(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestMultipleLoginScenarios_WithIntervals()
        {
            _output.WriteLine("测试开始：连接服务器并尝试多个用户登录（带时间间隔）...");

            int numberOfClients = 5; // 例如，10 个客户端
            int loginIntervalMilliseconds = 1000; // 每个登录之间的间隔（毫秒）
            var tasks = new List<Task>();

            // 用于存储每个客户端的结果
            var results = new List<(string Username, bool LoginSuccess, bool UserCountSuccess, string? ErrorMessage)>();

            // 锁对象以同步对 results 的访问
            object lockObj = new object();

            for (int i = 1; i <= numberOfClients; i++)
            {
                string username = $"User{i}";
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        // 可选：为每个客户端引入随机延迟（例如，0 到 loginIntervalMilliseconds 毫秒）
                        // var random = new Random();
                        // int delay = random.Next(0, loginIntervalMilliseconds);
                        // await Task.Delay(delay);

                        // 固定延迟
                        await Task.Delay(loginIntervalMilliseconds);

                        using (var client = new TcpClient())
                        {
                            await client.ConnectAsync("127.0.0.1", 51888);
                            _output.WriteLine($"[{username}] 已连接服务器 127.0.0.1:51888");

                            using (var ns = client.GetStream())
                            using (var sr = new StreamReader(ns))
                            using (var sw = new StreamWriter(ns) { AutoFlush = true })
                            {
                                
                                string loginMsg = $"Login,{username}";
                                _output.WriteLine($"[{username}] 发送消息: {loginMsg}");
                                await sw.WriteLineAsync(loginMsg);

                                // 读取服务器回应
                                string? line1 = await sr.ReadLineAsync();
                                _output.WriteLine($"[{username}] 服务器回应: {line1}");

                                bool loginSuccess = line1 != null && line1.StartsWith("LoginOK,");

                                // 期望收到UserCount消息
                                string? line2 = await sr.ReadLineAsync();
                                if (line2 == "") {
                                    line2 = await sr.ReadLineAsync();
                                }
                                _output.WriteLine($"[{username}] 服务器回应: {line2}");

                                bool userCountSuccess = line2 != null && line2.StartsWith("UserCount,");

                                lock (lockObj)
                                {
                                    results.Add((username, loginSuccess, userCountSuccess, null));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine($"[{username}] 异常: {ex.Message}");
                        lock (lockObj)
                        {
                            results.Add((username, false, false, ex.Message));
                        }
                    }
                }));
            }

            // 等待所有任务完成
            await Task.WhenAll(tasks);

            // 验证结果
            foreach (var result in results)
            {
                Assert.True(result.LoginSuccess, $"[{result.Username}] 登录失败。");
                Assert.True(result.UserCountSuccess, $"[{result.Username}] UserCount 验证失败。");
            }

            _output.WriteLine("多个用户登录测试（带时间间隔）成功结束！");
        }
    }
}
