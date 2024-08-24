using System;
using System.Threading;
namespace  WebApplication1.Utils;

public class SnowFlake
{
    private const long Twepoch = 1288834974657L; 
    private const int WorkerIdBits = 5;
    private const int DataCenterIdBits = 5;
    private const long MaxWorkerId = -1L ^ (-1L << WorkerIdBits); // 2^5-1
    private const long MaxDataCenterId = -1L ^ (-1L << DataCenterIdBits);

    private const int SequenceBits = 12;

    private const long WorkerIdShift = SequenceBits;
    private const long DataCenterIdShift = SequenceBits + WorkerIdBits;
    private const long TimestampLeftShift = SequenceBits + WorkerIdBits + DataCenterIdBits;
    private const long SequenceMask = -1L ^ (-1L << SequenceBits);

    private long _workerId;
    private long _dataCenterId;
    private long _sequence = 0L;

    private long _lastTimestamp = -1L;

    public SnowFlake(long workerId, long dataCenterId)
    {
        if (workerId > MaxWorkerId || workerId < 0)
        {
            throw new ArgumentException($"Worker Id cannot be greater than {MaxWorkerId} or less than 0");
        }

        if (dataCenterId > MaxDataCenterId || dataCenterId < 0)
        {
            throw new ArgumentException($"Data center Id cannot be greater than {MaxDataCenterId} or less than 0");
        }

        _workerId = workerId;
        _dataCenterId = dataCenterId;
    }

    public long NextId()
    {
        var timestamp = GetCurrentTimestamp();

        if (_lastTimestamp == timestamp)
        {
            _sequence = (_sequence + 1) & SequenceMask;
            if (_sequence == 0)
            {
                timestamp = WaitNextMillis(_lastTimestamp);
            }
        }
        else
        {
            _sequence = 0L;
        }

        if (timestamp < _lastTimestamp)
        {
            throw new Exception("Clock moved backwards. Refusing to generate id for " + (timestamp - _lastTimestamp) + " milliseconds");
        }

        _lastTimestamp = timestamp;

        return unchecked(
            (
                ((long)(timestamp - Twepoch)) << (int)TimestampLeftShift
            ) |
            (
                ((long)_dataCenterId) << (int)DataCenterIdShift
            ) |
            (
                ((long)_workerId) << (int)WorkerIdShift
            ) |
            _sequence
        );
    }

    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private long WaitNextMillis(long lastTimestamp)
    {
        var timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }

    // public static void Main(string[] args)
    // {
    //     var snowFlake = new SnowFlake(1, 1);
    //     Console.WriteLine(snowFlake.NextId());
    //     Thread.Sleep(10000);
    // }
}
