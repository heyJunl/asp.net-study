/*
 * @Author: Jun
 * @Description:
 */

using System.Collections.Concurrent;
using StackExchange.Redis;

namespace WebApplication1.Utils;

public class RedisUtils: IDisposable
{
    private string connectionString;
    private string instanceName;
    private int defaultDb;
    private ConcurrentDictionary<string, ConnectionMultiplexer> connections;

    public RedisUtils(string connectionString, string instanceName, int defaultDb)
    {
        this.connectionString = connectionString;   //  存储Redis的连接字符串
        this.instanceName = instanceName;   // 表示连接实例的名称
        this.defaultDb = defaultDb; // 默认使用数据库编号
        this.connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();   // 一个线程安全的字典，用于存储已建立的连接实例
    }

    public ConnectionMultiplexer GetConnection()
    {
        // 字段不存在则创建对应连接
        return connections.GetOrAdd(instanceName, p => ConnectionMultiplexer.Connect(connectionString));
    }

    public IDatabase GetDatabase()
    {
        // 获取默认数据库
        return GetConnection().GetDatabase(defaultDb);
    }

    public IServer GetsServer(string? configName = null, int endpointsIndex = 0)
    {
        // 根据连接字符串解析出配置选项，从中选择端点的服务器实例
        var confOptions = ConfigurationOptions.Parse(connectionString);
        return GetConnection().GetServer(confOptions.EndPoints[endpointsIndex]);
    }

    public ISubscriber GetSubscriber()
    {
        // 获取订阅者实例，处理Redis发布/订阅
        return GetConnection().GetSubscriber();
    }

    /**
     * 释放已存在连接的资源
     */
    public void Dispose()
    {
        if (connections != null && connections.Count > 0)
        {
            foreach (var item in connections.Values)
            {
                item.Dispose();
            }
        }
    }
}