using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static DateTime FromUnixTime(int unixTime)
    {
        return new DateTime(1970, 1, 1, 0, 0, 0,DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
    }

    public static int ToUnixTime(this DateTime dateTime)
    {
        return (int)(dateTime.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
    }
}
