using System;

namespace ModlistVerification;

public struct ModMetadata
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string GUID { get; set; }

    public static object[] ToRPCBuffer(ModMetadata modlist)
    {
        return
        [
            modlist.Name,
            modlist.Version,
            modlist.GUID
        ];
    }

    public static ModMetadata FromRPCBuffer(object[] buffer)
    {
        if (buffer[0] is not string ||
            buffer[1] is not string ||
            buffer[2] is not string)
        {
            throw new ArgumentException("Invalid modlist buffer received.");
        }

        return new ModMetadata
        {
            Name = (string)buffer[0],
            Version = (string)buffer[1],
            GUID = (string)buffer[2]
        };
    }
}
