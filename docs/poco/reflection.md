# 反射
>通过静态类InstanceHelper或扩展方法获取反射信息

## 1. GetProperties
>通过GetProperties方法获取类型所有公开实例属性

## 2. GetFields
>通过GetFields方法获取类型所有公开实例字段

## 3. GetConstructors
>通过GetConstructors方法获取类型所有构造函数

## 4. GetMethods
>通过GetMethods方法类型所有函数

## 5. IPocoOptions的扩展方法GetTypeMembers
>* 通过GetTypeMembers方法获取类型的成员信息
>* 含可读成员、可写成员、读取器和写入器
>* 成员一般是属性或字段(可通过配置修改规则)

## 6. IPocoOptions的扩展方法GetReadMember
>* 通过GetReadMember获取类型的可读成员

## 7. IPocoOptions的扩展方法GetWriteMember
>* 通过GetWriteMember获取类型的可写成员


## 8. IPocoOptions的扩展方法GetEmitReader
>* 通过GetEmitReader获取类型的读取器

## 9. IPocoOptions的扩展方法GetEmitWriter
>* 通过GetEmitWriter获取类型的写入器