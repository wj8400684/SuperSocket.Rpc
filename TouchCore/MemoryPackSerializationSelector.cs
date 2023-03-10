﻿using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Rpc.TouchRpc;

namespace TouchCore;

public class MemoryPackSerializationSelector : DefaultSerializationSelector
{
    public override byte[] SerializeParameter(SerializationType serializationType, object parameter)
    {
        if ((byte)serializationType == 4)
        {
            return MemoryPackSerializer.Serialize(parameter.GetType(), parameter);
        }
        return base.SerializeParameter(serializationType, parameter);
    }

    public override object DeserializeParameter(SerializationType serializationType, byte[] parameterBytes, Type parameterType)
    {
        if ((byte)serializationType == 4)
        {
            if (parameterBytes==null)
            {
                return new object();
            }
            return MemoryPackSerializer.Deserialize(parameterType, parameterBytes)!;
        }
        return base.DeserializeParameter(serializationType, parameterBytes, parameterType);
    }
}
