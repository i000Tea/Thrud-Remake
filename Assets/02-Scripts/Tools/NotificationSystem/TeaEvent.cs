namespace TeaFramework
{
   // 委托类,自己进行委托封装，可以添加多个参数
   /// <summary> 无参委托 </summary>
   public delegate void TeaEvent();
   /// <summary> 1参委托 </summary>
   public delegate void TeaEvent<T>(T arg);
   /// <summary> 2参委托 </summary>
   public delegate void TeaEvent<T, X>(T arg1, X arg2);
   /// <summary> 3参委托 </summary>
   public delegate void TeaEvent<T, X, Y>(T arg1, X arg2, Y arg3);
   /// <summary> 4参委托 </summary>
   public delegate void TeaEvent<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
   /// <summary> 5参委托 </summary>
   public delegate void TeaEvent<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);

}