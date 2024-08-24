# C#

## Base

类比理解为Java中的extend和implement，具有几个不同的用途，都与继承有关

```C#
// 调用基类构造函数
public class DerivedClass: BaseClass{
    public DerivedClass(): base(){}		// 调用基类构造函数
    public DerivedClass(): base(value){}	// 调用且传参
}

// 访问基类成员
public class BaseClass{
    public void PrintMessage() {
        CW("...");
    }
}

public class Derived: BaseClass{
    public void PrintMessage() {
        CW("...");
        base.PrintMessage();	// 调用基类PrintMessage方法
    }
}

// 指定基类作为强制转换目标
public class BaseClass{}

public class Derived: BaseClass{
    public void Method() {
        BaseClass bc = base;	// 将派生类对象转换为基类类型
    }
}
```

## ?.条件运算符

类似Java里面的optional，左侧为null则表达式为null，不为null则走后续流程，避免空指针

```C#
if pwd == auth?.pwd
```

## ??合并运算符

类似Python的海象符，如果左边为null则返回右边的数

```C#
string result = str ?? "default";
```



## Using()



Using是资源管理语句，常用于需要显式释放的非托管资源，如文件流、网络连接、数据库连接等。

- 当 `using` 块内的代码执行完毕时，无论是正常完成还是因为异常而退出，`using` 语句都会自动调用每个对象的 `Dispose` 方法。这样可以确保释放

```c#
// base64编码的字符串转换成字节数组
using(var mStream = new MemoryStream(Convert.FromBase64String(source)));

// 使用解密器对数据解密
using (var cryptoStream = new CryptoStream(mStream,
                       DesHandler.CreateDecryptor(DesHandler.Key, DesHandler.IV), CryptoStreamMode.Read));

// 创建对象读取解密后的文本数据
using (var reader = new StreamReader(cryptoStream));
```

## readonly

类比Java中的finall，初始化后不能修改

- 不能用于修饰静态字段、修饰类或接口
- 字段或局部变量使用使用了readonly则不需要const

```C#
// 字段，必须在声明时或构造函数中初始化，不能修改
public readonly int Field;

// 变量，必须在声明时初始化，不能修改
public void Func() {
    readonly int local = 10;
}

// 方法，不能包含任何可变参数，不可抛出异常
public readonly void MyMethod() {
    // pass
}

// 委托，不能更改
public readonly Action MyDelegate;

// 属性，get访问器必须返回一个常量，而set访问器不能被实现
public readonly int Property{get; private set;}
```

## out关键字

用于声明的一个输出参数，方法调用时通过参数通过引用传递，如果使用out参数则必须在方法体内为每个out参数赋值。我的理解是，就是将带有out关键字的参数return出去，

```C#
public void Example(int a, out int b) {
    b = a * 2;
}
int result;
Example(5, out result);	// result = 10
```

## record关键字

默认实现不可变性、Equals和GetHashCode方法及ToString方法，自动提供了构造函数、属性的比较和字符串表示的功能

## 判空

### str

```C#
string.IsNullOrEmpty(s);	// 如果是空串判不出来
string.IsNullOrWhiteSpace(s);	// 空串判的出来
```

### List

```c#
if (myList == null || !myList.Any()){}	// 集合为null或没有元素
if (myList?.Any() == false){}	// 集合为null或没有元素
```

### 对象

```C#
if (obj == null) {}	// 对象为空
if (obj is null) {} // 对象为空

Object.ReferenceEquals(obj1, obj2);	// 判断对象引用地址是否一样
object.Equals(str1, str2);	// 判断两对象值是否相等，需要重写Equals方法
object DefaultObj = obj ?? new object();	// 使用？？运算符提供默认值
```

### 值引用

```C#
int? myInt = null;
if (!myInt.HasValue) {}
int defaultInt = myInt ?? 0;	// 使用？？运算符提供默认值
```







# Asp.Net

## 创建项目步骤

> > [【asp.net core 系列】6 实战之 一个项目的完整结构 - 月影西下 - 博客园 (cnblogs.com)](https://www.cnblogs.com/c7jie/p/13055315.html)

**单一项目解决方案**：只需要创建一个解决方案，在解决方案中添加多个项目

- 优点：简化管理、构建速度、版本控制、跨项目依赖、Ide集成

```python
dotnet new sln --name Template	# 创建Solution

dotnet new classlib --name Data	# 创建项目

dotnet sln add Data	# 添加项目进Solution
```

**项目引用：** 项目之间依赖关系，引用者可以使用被引用者项目中的类型和资源。eg：类、接口、枚举等

```PYTHON
dotnet new classlib --name Domain
dotnet sln add Domain 
# locate domain folder 
cd Domain
# Domain reference Data，Data中存放模型层
dotnet add reference ../Data
```

**创建接口层与实现层：**

```PYTHON
dotnet new classlib --name Domain.Implements	# 实现层
cd Domain.Implements	# 准备引入接口层
dotnet add reference ../Data
dotnet add reference ../Domain
```

**参考图：**
[1266612-20200606164559783-1600568456.png (1401×1116) (cnblogs.com)](https://img2020.cnblogs.com/other/1266612/202006/1266612-20200606164559783-1600568456.png)



Data是各层间的数据流图依据，各个项目都依赖此项目，各接口层的实现层都只对Web可见，其他层不知道具体实现，这样的优点是：

- 调用方不知实现方逻辑，避免调用方对特定实现的依赖。
- 利于团队协作和后期优化，不论分层还是分模块，只需要切换实现层即可。

## LINQ

```C#
T Get(Expression<Func<T,bool>> predicate);	// 查询
```

- `T`：这是一个泛型参数，表示方法的返回类型。`T`可以代表任何类型。
- `Get`：这是方法的名字。
- `(Expression<Func<T, bool>> predicate)`：这是方法的参数。它是一个泛型表达式，其中`Expression`是一个泛型类型，而`Func<T, bool>`是一个委托类型，表示一个返回布尔值的无参函数。

**实现：**

```c#
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var evenNumber = numbers.Get(n => n % 2 == 0); // 返回第一个偶数
```



```C#
List<T> Search<P>(Expression<Func<T, bool>> predicate, Expression<Func<T, P>> order);	// 根据提供的predicate表达式在集合中找到满足条件的元素，然后根据order表达式对这些元素进行排序。
```

- `List<T>`：这是一个泛型参数，表示方法的返回类型。`T`可以代表任何类型。
- `Search`：这是方法的名字。
- `(Expression<Func<T, bool>> predicate, Expression<Func<T, P>> order)`：这是方法的参数。它接受两个泛型表达式，其中`predicate`是一个Lambda表达式，它定义了一个条件，LINQ将使用这个条件来过滤集合；`order`也是一个Lambda表达式，它定义了一个排序规则，LINQ将使用这个规则来对满足条件的元素进行排序。

**实现：**

```C#
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var sortedNumbers = numbers.Search(n => n % 2 == 0, n => n); // 返回所有偶数，并按原顺序排序，左边是泛型，右边是return类型
```

## T和P的理解

```C#
List<T> Search<P>(Expression<Func<T, bool>> predicate, Expression<Func<T, P>> order);
```

- T：第一个泛型类型参数，表示方法可以操作集合中元素的类型，如果是整数可以替换成int
- P：第二个泛型类型参数，表示排序时使用的键类型，可以使用对象的属性来排序

---



# EFCore

> [EFCore 从入门到精通-2（初体验)-CSDN博客](https://blog.csdn.net/xieyunhappy/article/details/112093495?spm=1001.2014.3001.5501)

下表列出了EF Core的数据库提供程序和NuGet程序包。

---

数据库	Nuget程序包
SQL Server	Microsoft.EntityFrameworkCore.SqlServer
MySQL	MySql.Data.EntityFrameworkCore（官方版，不建议使用）
MySQL	Pomelo.EntityFrameworkCore(第三方提供，Bug少建议使用)
PostgreSQL	Npgsql.EntityFrameworkCore.PostgreSQL
SQLite	Microsoft.EntityFrameworkCore.SQLite
SQL Compact	Microsoft.EntityFrameworkCore.SQLite
In-memory	Microsoft.EntityFrameworkCore.InMemory

---

## 数据迁移

> 使用 Rider 在 Console 中的指令：[Entity Framework教程-数据迁移（migrations） - 重庆熊猫 - 博客园 (cnblogs.com)](https://www.cnblogs.com/cqpanda/p/16815263.html)

```csharp
public class TestContext: DbContext
{
    public DbSet<Student> Students { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;database=efcorelearn;user=root;password=123456", new MySqlServerVersion("8.0.39"));
    }
}

public partial class Student
{
    public uint StudentId { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
}
```

- 添加迁移：dotnet ef migrations add [migration name]  // --verbose 显示日志
- 更新：dotnet ef database update

## Demo

1. 创建Console
2. 输出CMD添加包

```C#
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet tool install --global dotnet-ef
```

3. Scaffold-DbContext：逆向创建模型

```PYTHON
dotnet ef dbcontext scaffold "Server=localhost;Database=test;User=root;Password=123456;" Pomelo.EntityFrameworkCore.MySql -o Models   
```

---

##　ＣＲＵＤ

``` python
# 单个查询
var teacher = db.Teacher.Single(x => x.Name == "Lee");
var teacher = db.Teacher.Where(x => x.Age > 20).FirstOrDefault()

# 多个查询
var teachers = db.Teacher.ToList();
var courses = db.Course.ToArray();

# 模糊查询
db.Teacher.Where(x => x.Age > 20).FirstOrDefault();
db.Student.Where(x => x.Name.StartsWith("l")).FirstOrDefault();

'''
区别：非追踪查询效率高，但是修改数据后不能同步进数据库中，反之追踪查询在修改后调用saveChange函数可以同步修改内容进数据库
'''
# 追踪查询
db.Teacher.Single(x => x.TeacherId == 10001);
db.SaveChanges();
# 非追踪查询
db.Teacher.AsNoTracking().Single(x => x.TeacherId == 100001);
```



# 项目过程

## SQL

创建数据库表

```sql
use information;
create table student(
    id  varchar(256) not null primary key comment 'id',
    name    varchar(10) comment '姓名',
    sex int comment '性别 0-man，1-female',
    class_id   long comment '班级id',
    birth   varchar(50) comment '出生年月',
    address varchar(50) comment '家庭住址',
    dept    varchar(50) comment '所在院系',
    create_time datetime DEFAULT CURRENT_TIMESTAMP comment '创建时间',
    create_by   varchar(50) comment '创建人',
    update_time datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50) comment '更新人'
)comment '学生信息表' ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

create table clazz(
    id  varchar(256) not null primary key comment 'id',
    grade   varchar(50) comment '年级',
    class   varchar(50) comment '班级',
    year    varchar(50) comment '年制',
    room    varchar(50) comment '教室',
    teacher_id long comment '教师id',
    total   int comment '学生人数',
    sub int comment '专业',
    create_time datetime DEFAULT CURRENT_TIMESTAMP comment '创建时间',
    create_by   varchar(50) comment '创建人',
    update_time datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50) comment '更新人'
)comment '班级信息表' ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

create table teacher(
    id  varchar(256) not null primary key comment 'id',
    name    varchar(50) comment '教师姓名',
    sex int comment '性别 0-man，1-female',
    phone   varchar(50) comment '手机号',
    create_time datetime DEFAULT CURRENT_TIMESTAMP comment '创建时间',
    create_by   varchar(50) comment '创建人',
    update_time datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50) comment '更新人'
)comment '教师表' ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;

create table user(
    id varchar(256) not null primary key comment 'id',
    user    varchar(50) not null comment '账户',
    pwd varchar(256) not null comment '密码',
    permission  int not null comment '权限id',
    create_time datetime DEFAULT CURRENT_TIMESTAMP comment '创建时间',
    update_time datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP comment '更新时间'
)  comment '用户表' ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = DYNAMIC;
```

## SnowFlake

生成分布式系统中唯一Id的算法，设计原理如下：

`[数据中心Id] [工作机器Id] [序列号] [时间戳]`

1. 时间戳：当前时间作为生成Id的基础，时间戳位数取决于你需要的Id最大生成时间跨度。如使用41位时间戳可以支持69年（1970-2039）
2. 数据中心：使用5位表示数据中心Id。最多可以有32个不同的数据中心
3. 工作Id：区分同一数据中心内不同的机器，使用5位表示工作机器Id，同一数据中心内最多有32台机器。

4. 序列号：同一毫秒内，同一台机器可能生产多个Id，使用12位表示序列号，每台机器每毫秒生成4096个Id

```C#
```



# 理解ASP.NET CORE

> [理解ASP.NET Core - Startup - xiaoxiaotank - 博客园 (cnblogs.com)](https://www.cnblogs.com/xiaoxiaotank/p/15185325.html)

## Startup

在Program的**ConfigureServices**中有很多常用服务，它们大都是可选的

- AddControllers：注册Controller相关服务
- AddOptions：注册Options相关服务
- AddRouting：注册路由相关服务
- AddLogging：注册日志
- AddAuthentication：注册身份认证
- AddAuthorization：注册用户授权
- AddMvc：注册Mvc
- AddHealthChecks：注册健康检查

---

还有**Configure**方法，该方法是必须的，在Configure Services方法后调用，其内中间件的注册顺序与代码的书写顺序是一致的，常用的中间件有：

- UseDeveloperExceptionPage：发生异常时展开信息页
- UseRouting：根据Url路径导航到对应EndPoint，必须与UseEndPoints搭配
- UseAuthentication：身份认证，对请求用户的身份认证
- UseAuthorization：授权中间件
- UseMvc：Mvc中间件
- UseHealthChecks：健康检查中间件
- UseMiddleware：匿名中间件
