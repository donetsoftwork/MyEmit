using PocoEmit.Activators;
using PocoEmit.Members;
using System;
using System.Collections.Generic;

namespace PocoEmit.Collections.Activators;

/// <summary>
/// 列表激活器
/// </summary>
/// <param name="elementType"></param>
/// <param name="sourceCount"></param>
public class ListActivator(Type elementType, IEmitCollectionCounter sourceCount)
    : CollectionActivator(elementType, typeof(List<>).MakeGenericType(elementType), sourceCount)
    , IEmitActivator
{
}
