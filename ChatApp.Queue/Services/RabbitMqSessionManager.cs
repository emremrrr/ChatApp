using System;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ChatApp.Queue.Services;


public class RabbitMqSessionManager :IDisposable
{
    private readonly IConnection _connection;
    private readonly ConcurrentDictionary<Guid, IModel> _channels;

    public RabbitMqSessionManager()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channels = new ConcurrentDictionary<Guid, IModel>();
    }

    public IModel CreateSession(Guid clientId)
    {
        if (_channels.ContainsKey(clientId))
        {
            throw new InvalidOperationException("Session already exists!");
        }

        var channel = _connection.CreateModel();
        _channels[clientId] = channel;
        return channel;
    }

    public void CloseSession(Guid clientId)
    {
        if (_channels.TryRemove(clientId, out var channel))
        {
            channel.Close();
            channel.Dispose();
        }
    }
    public bool CheckSession(Guid clientId){
        return _channels.ContainsKey(clientId);
    }
    public void Dispose()
    {
        foreach (var channel in _channels.Values)
        {
            
            channel.Close();
            channel.Dispose();
        }

        _connection.Close();
        _connection.Dispose();
    }
}