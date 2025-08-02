# PocoEmit
>* 简单对象Emit工具
>* 支持跨平台
>* 支持net4.5;net5.0;net6.0;net7.0;net8.0;net9.0
>* 支持netstandard1.1;netstandard1.3;netstandard1.6;netstandard2.0;netstandard2.1
>* Nuget包名: PocoEmit

## 一、PocoEmit简介
>* 用来生成读写POCO对象属性和字段的委托
>* 用来生成基础类型转化
>* 使用空间换时间,把生成的结果及中间结果缓存在内存中
>* 支持配置增强或调整生成行为
>* 生成的委托性能比反射快

## 二、封装、继承和多态
### 1. 基于接口IPocoOptions封装
>* 封装反射属性、字段、读、写、转化等功能
>* 缓存各种生成的结果及中间结果
>* 封装各种配置

### 2. 继承
>* Poco.Global可以独立使用
>* Poco.Global的配置可能影响每个Poco对象的行为

### 3. 多态
>* 每个[Poco](xref:PocoEmit.Poco)对象可以通过配置覆盖Poco.Global对自己的影响

### 4. 项目级继承和多态
>* PocoEmit.Mapper是PocoEmit的继承和多态(扩展)