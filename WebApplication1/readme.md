# C#

## 编程规范

### 命名规则

1. 编程符必须以字母或下划线开头
2. 标识符可以包含Unicode字符、十进制数字字符、Unicode连接字符、Unicode组合字符或Unicode格式字符

可以在标识符上使用@前缀声明与C#关键字匹配的标识符。eg.@if声明为if的标识符

## 命名约定

类型名称、命名空间和所有公共成员使用PascalCase

- 接口名称以I开头，属性类型以Attribute结尾，对变量、方法和类使用有意义的描述名称
- 枚举类型对非标记使用单数名词，对标记使用复数名词，标识符不应包含两个连续的下划线_字符，这些名称保留給编译器生成
- 清晰>简洁，类名和方法名称采用PascalCase，常量名包括字段和局部变量也是
- 对方法参数、局部变量使用驼峰式大小写，专用实例字段以下划线_开头，其余文本为驼峰式大小写，静态字段以s_开头
- 避免在名称中使用缩写或首字母，广为人知的除外，使用遵循反向域名表示法的命名空间
- S用于结构，C用于类，M用于方法，v用于变量，p用于参数，r用于ref参数

## 编码约定

### 字符串

1. 使用内插连接短字符串

```C#
string displayName = $"{first}, {second}";
```

2. 循环追加字符串，尤其是大量文本，使用stringbuilder

```C#
var a = "aaaaaaaaaaaaaaaaaaa";
var sb = new StringBuilder();
foreach (var i in a)
{
    sb.Append(i);
}
```

### 数组

1. 声明行上初始化数组时，使用简介的语法

```C#
// 不能使用var
int[] v1 = {1, 2, 3, 4, 5};
// 显式
var v2 = new string[]{"a, b, c, d"};
```

### 委托

使用Func<>和Action<>，而不是委托在类中定义委托方法

```C#
Action<string> a1 = x => Console.WriteLine($"x is:{x}");
Action<string, string> a2 = (x, y) => Console.WriteLine($"x is:{x}, y is {y}");

Func<string, int> f1 = x => Convert.ToInt32(x);
Func<int, int, int> f2 = (x, y) => x + y;

// 使用Func<> 或 Action<>委托定义的签名调用方法
a1("string for x");
a2("string for x", "string for y");
Console.WriteLine($"The value is {f1("1")}");
Console.WriteLine($"The sum is {f2(1, 2)}");
```

**多播委托**

```c#
// 委托是一种声明，类似于一个接口，带有关键字，后面跟着返回类型和参数列表
public delegate void MyDelegate(string message);

public class Program
{
    public static void Main()
    {
        MyDelegate printDelegate = new MyDelegate(PrintMessage);
        printDelegate += PrintUpperCase;
        printDelegate("Hello World");
    }
    public static void PrintMessage(string message)
    {
        Console.WriteLine(message);
    }
    public static void PrintUpperCase(string message)
    {
        Console.WriteLine(message.ToUpper());
    }
}
```

### 释放资源

```C#
// 显示释放资源
Font bodyStyle = new Font("Arial", 10.0f);
try
{
    byte charset = bodyStyle.GdiCharSet;
}
finally
{
    if (bodyStyle != null)
    {
        ((IDisposable)bodyStyle).Dispose();
    }
}
// 使用using，在离开作用域时自动调用Dispose方法，即使出现异常任然可以释放掉资源
using Font normalStyle = new Font("Arial", 10.0f);
byte charset3 = normalStyle.GdiCharSet;
```

### new关键字

```C#
// 创建对象时下列两种方法等效
var user = new User();
User user2 = new();
```

## 索引

^类似于取反，..类似于python中的:，可以切片。[String](https://learn.microsoft.com/zh-cn/dotnet/api/system.string)、[Span](https://learn.microsoft.com/zh-cn/dotnet/api/system.span-1) 和 [ReadOnlySpan](https://learn.microsoft.com/zh-cn/dotnet/api/system.readonlyspan-1)。[List](https://learn.microsoft.com/zh-cn/dotnet/api/system.collections.generic.list-1) 支持索引，但不支持范围。

**在数组中获取范围是从初始数组复制的数组而不是引用的数组，修改生成的值不会更改数组中的值**

```C#
string[] words = [
    // index from start    index from end
    "The",      // 0                   ^9
    "quick",    // 1                   ^8
    "brown",    // 2                   ^7
];
// brown
Console.WriteLine(words[^1]);
// uick
Console.WriteLine(words[1][1..]);
// ui
Console.WriteLine(words[1][1..3]);
// brown
Console.WriteLine(words[words.Length - 1]);
```



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

## 匿名类型

将只读数据封装到单个对象中，而无需显示定义一个类型

```C#
var v = new {Amount = 108, Message = 'Hello'};
```

通常用在查询表达式的select子句中（LINQ），其用来初始化的属性不能为null、匿名函数或指针类型。匿名类型是class类型，直接派生自object，且无法强制转换为除object外的任何类型，支持采用with表达式形式的非破坏性修改，类似于Java中的Set不过只能修改已存在的属性。匿名类型会重写ToString方法

```C#
var apple = new {Item = "apple", Price = 1.35};
var onSale = apple with {Price=0.79};
Console.WriteLine(apple);
Console.WriteLine(onSale);
```





## record 记录关键字

类似Java中的final，默认实现不可变性（不可更改对象的任何属性或字段值）、Equals和GetHashCode方法及ToString方法，自动提供了构造函数、属性的比较和字符串表示的功能

- **值相等性**：record 类型自动拥有一个经过优化的 `Equals` 方法，该方法比较两个 record 实例的字段值是否相等。
- **简洁的声明式语法**：record 允许使用更简洁的语法来声明只包含数据的类。
- **不可变性**：record 类型的实例一旦创建，其状态就不能更改（除非显式地使用可变记录）。
- **继承**：record 可以继承自其他 record 或 class，并且可以添加额外的成员。

以下情况考虑使用记录：

1. 定义依赖值相等性的数据模型
2. 定义对象不可变类型

## 关系模式

```C#
string WaterState(int temp) => temp switch
    {
        (>32) and (<212) => "liquid",
        < 32 => "solid",
        > 212 => "gas",
        32 => "solid/liquid transition",
        212 => "liquid/gas transition",
        _ => throw new ArgumentOutOfRangeException()
    };
```



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



## LINQ

### Where

从数据源中筛选出元素

```C#
from city in cities where city.Pop is < 200 and >100 select city;
```

### 排序

#### OrderBy

可按升序或降序排列，例子中以Area为主，population为辅

```C#
var orderedEnumerable = from country in countries 
    orderby country.Area, country.Population descending select country;
```

#### ThenBy

按升序执行次要排序

#### Reverse

反转集合中的元素



### Join

将数据源中元素于另一个数据源元素进行关联和/或合并，连接序列之后必须使用select或group语句指定存储在输入序列中的元素。示例关联Category属性与categories字符串数组中一个类别匹配的prod对象

```C#
var cateQuery = from cat in categories
            join prod in products on cat equals prod.Category
            select new
            {
                Category = cat,
                Name = prod.Name
            };
```

### Let

使用let将结果存储在新范围变量中

```C#
from name in names select names 
    let firstName = name.Split(" ")[0]
    select firstName；
```

### 多查询

```C#
var query = from student in students
            // 按照student.Year分组
            group student by student.Year
            // 为分组定义别名，后续使用别名进行分组
            into studentGroup
            select new
            {   // 每个分组的键，学生的年级
                Level = studentGroup.Key,
                // 每个分组中，所有学生的平均成绩
                HighestScore = (from student2 in studentGroup select student2.ExamScores.Average()).Max()
            };
```

### 查询对象

```c#
var entity = from o in InComingOrders
    where o.OrderSize > 5
    select new Customer { Name = o.Name, Phone = o.Phone };
// LINQ写法
var entity2 = InComingOrders.Where(e => e.OrderSize > 5)
    .Select(e => new Customer { Name = e.Name, Phone = e.Phone });
```

### 作为数据表达式（Lambda）

结合out关键字返回查询

```C#
void QueryMethod(int[] ints, out List<string> returnQ) =>
            returnQ = (from i in ints where i < 4 select i.ToString()).ToList();

int[] nums = [0, 1, 2, 3, 4, 5, 6, 7];
QueryMethod(nums, out List<string> result);
foreach (var item in result)
{
    Console.WriteLine(item);
}
```

#### eg

```C#
// 普通方法编写查询总分数据
var studentQuery1 = from student in studnets
    let totalScore = student.Scores[0] + student.Scores[1] + student.Scores[2] + student.Scores[3]
    select totalScore;
// 使用Linq方法编写查询总分数据
var studentQuery2 = studnets.Select(e => e.Scores[0] + e.Scores[1] + e.Scores[2] + e.Scores[3]);
// 统计平均分
double average = studentQuery1.Average();

// 将大于平均分的学生数据映射为对象
var query1 =
    from student in studnets
    let x = student.Scores[0] + student.Scores[1] +
            student.Scores[2] + student.Scores[3]
    where x > average
    select new { id = student.ID, score = x };
// 使用Linq写法
var query2 = studnets.Where(e => e.Scores[0] + e.Scores[1] + e.Scores[2] + e.Scores[3] > average).Select(e =>
    new { id = e.ID, score = e.Scores[0] + e.Scores[1] + e.Scores[2] + e.Scores[3] });
// Linq简洁写法
var query3 = studnets.Select(e => new { id = e.ID, score = e.Scores[0] + e.Scores[1] + e.Scores[2] + e.Scores[3] })
    .Where(e => e.score > average);


foreach (var item in query1)
{
    Console.WriteLine("Student ID: {0},Score: {1}", item.id, item.score);
}
```

### 投影运算

#### SelectMany

多个from子句投影字符串列表中每个字符串中的单词

```C#
List<string> phrases = ["an apple a day", "the quick brown fox"];
// 普通写法
var query = from phrase in phrases from word in phrase.Split(' ') select word;
// Linq写法
var query2 = phrases.SelectMany(e => e.Split(' '));
```

### Zip列表压缩元组，类似python

```c#
// An int array with 7 elements.
IEnumerable<int> numbers = [1, 2, 3, 4, 5, 6, 7];
// A char array with 6 elements.
IEnumerable<char> letters = ['A', 'B', 'C', 'D', 'E', 'F'];
// A string array with 8 elements.
IEnumerable<string> emoji = [ "🤓", "🔥", "🎉", "👀", "⭐", "💜", "✔", "💯"];

foreach (var (first, second, third) in numbers.Zip(letters, emoji))
{
    Console.WriteLine($"Number:{first} is zipped with letter: {second} and emoji {third}");
}
```

### Set集合操作

#### 去重

```C#
string[] words = ["the", "quick", "brown", "fox", "jumped", "over", "the", "lazy", "dog"];
// 去重
var query = from word in words.Distinct() select word;
// 根据条件去重
var query2 = from word in words.DistinctBy(e => e.Length)select word;
```

#### 差集

```C#
string[] words1 = ["the", "quick", "brown", "fox"];
string[] words2 = ["jumped", "over", "the", "lazy", "dog"];
// console：queik、brown、fox，输出1在2中没有的元素
IEnumerable<string> query = from word in words1.Except(words2) select word;
// expectBy同理，根据自定义字段进行操作
var result = new List<Person> { new Person { Name = "Alice" }, new Person { Name = "Bob" } }
                            .ExceptBy(person => person.Name,
                                     new List<Person> { new Person { Name = "Alice" }, new Person { Name = "Charlie" } });
// result 将包含 { new Person { Name = "Bob" } }，因为 "Alice" 在两个集合中都存在，而 "Bob" 和 "Charlie" 只在第一个集合中。
```

#### 交集

```C#
string[] words1 = ["the", "quick", "brown", "fox"];
string[] words2 = ["jumped", "over", "the", "lazy", "dog"];
// 输出the
IEnumerable<string> query = from word in words1.Intersect(words2) select word;
var list = words1.Intersect(words2).Select(e => e.ToUpper()).ToList();
// 通过比较名称生成 Teacher 和 Student 的交集
(Student person in
    students.IntersectBy(
        teachers.Select(t => (t.First, t.Last)), s => (s.FirstName, s.LastName)))
```

#### 并集

```C#
string[] words1 = ["the", "quick", "brown", "fox"];
string[] words2 = ["jumped", "over", "the", "lazy", "dog"];
// 使用UnionBy
(var person in
    students.Select(s => (s.FirstName, s.LastName)).UnionBy(
        teachers.Select(t => (FirstName: t.First, LastName: t.Last)), s => (s.FirstName, s.LastName)))
// 输出：the quick brown fox jumped over lazy dog
var query = (from word in words1.Union(words2) select word).ToList();
var list = words1.Union(words2).ToList();
```

### 限定符

- All()：所有
- Any():任何
- Contains():正好

```
IEnumerable<string> names = from student in students
                            where student.Scores.Contains(95)
                            select $"{student.FirstName} {student.LastName}: {string.Join(", ", student.Scores.Select(s => s.ToString()))}";
```

- Skip():跳过序列中指定位置之前的元素
- SkipWhile():基于谓词函数跳过元素，直到元素不符合条件
- Take():获取序列中指定位置之前的元素
- TakeWhile():同上操作
- Chunk():将序列元素拆分为指定最大大小的区块

```C#
var resource = Enumerable.Range(0, 8);
// 012
foreach (var i in resource.Take(3)){ }
// 345678
foreach (var i in resource.Skip(3)){ }
// 012345
foreach (var i in resource.TakeWhile(e=>e<5)){ }s
// 678
foreach (var i in resource.SkipWhile(e=>e<5)){ }
// 平均分块，将数据分成三块，123、456、78
int chunkNum = 1;
foreach (int[] chunk in Enumerable.Range(0, 8).Chunk(3))
{
    Console.WriteLine($"Chunk {chunkNum++}:)");
    foreach (int item in chunk)
    {
        Console.WriteLine($"   {item}");
    }
    Console.WriteLine();
}
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

## CancellationToken

一种取消操作机制，在进行异步操作时可传递 CancellationToken 作为参数，在 default 关键字后跟随 CancellationToken 时，会进行默认创建不绑定到任何取消源，除非手动调用 Cancel 方法

---

## 入参类型

1. [FromBody]：以 Json 对象形式传递

2. [FromFrom]：表单形式提交，Key：Value

3. [FromHeader]：请求标头

4. [FromQuery]：请求查询字符串参数，以uri形式，?ID=1&NAME=LEE

5. [FromRoute]：请求路由数据，/api/Get/Route

    ```C#
    [HttpGet("{method}/{value}")]
    public async Task<Parameter> GetRouteAsync([FromRoute] Parameter route)
    {
        return await Task.FromResult(route);
    }
    ```

    

6. [FromServices]：作为操作参数插入的请求服务

---

## IResult 和 TypeResults

1. IResult 是一个接口，用于定义响应操作的契约
2. TypeResults 是一个静态类，提供创建实现IResults接口实现类
3. IResult用于定义自定义响应逻辑，TypeResults用于快速生成预定义模型

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

## 自动填充CreateTime/UpdateTime

重写 Efcore 中的 SaveChange 方法

```C#
public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        var entityEntries = ChangeTracker.Entries().ToList();
        foreach (var item in entityEntries)
        {
            if (item.State == EntityState.Added)
            {
                Entry(item.Entity).Property(nameof(BaseData.CreateTime)).CurrentValue = DateTime.Now;
            }

            if (item.State == EntityState.Modified)
            {
                Entry(item.Entity).Property(nameof(BaseData.UpdateTime)).CurrentValue = DateTime.Now;
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
```



# 项目过程

## SQL

创建数据库表

```sql
-- auto-generated definition
create table clazz
(
    id          varchar(256)                       not null comment 'id'
        primary key,
    grade       varchar(50)                        null comment '年级',
    class       varchar(50)                        null comment '班级',
    year        varchar(50)                        null comment '年制',
    room        varchar(50)                        null comment '教室',
    teacher_id  mediumtext                         null comment '教师id',
    total       int                                null comment '学生人数',
    sub         int                                null comment '专业',
    create_time datetime default CURRENT_TIMESTAMP null comment '创建时间',
    create_by   varchar(50)                        null comment '创建人',
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50)                        null comment '更新人'
)
    comment '班级信息表' collate = utf8mb4_general_ci
                         row_format = DYNAMIC;

-- auto-generated definition
create table student
(
    id          varchar(256)                       not null comment 'id'
        primary key,
    name        varchar(10)                        null comment '姓名',
    sex         int                                null comment '性别 0-man，1-female',
    class_id    varchar(256)                       null comment '班级id',
    birth       varchar(50)                        null comment '出生年月',
    address     varchar(50)                        null comment '家庭住址',
    dept        varchar(50)                        null comment '所在院系',
    create_time datetime default (curtime())       null comment '创建时间',
    create_by   varchar(50)                        null comment '创建人',
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50)                        null comment '更新人'
)
    comment '学生信息表' collate = utf8mb4_general_ci
                         row_format = DYNAMIC;

-- auto-generated definition
create table teacher
(
    id          varchar(256)                       not null comment 'id'
        primary key,
    name        varchar(50)                        null comment '教师姓名',
    sex         int                                null comment '性别 0-man，1-female',
    phone       varchar(50)                        null comment '手机号',
    create_time datetime default CURRENT_TIMESTAMP null comment '创建时间',
    create_by   varchar(50)                        null comment '创建人',
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP comment '更新时间',
    update_by   varchar(50)                        null comment '更新人'
)
    comment '教师表' collate = utf8mb4_general_ci
                     row_format = DYNAMIC;

-- auto-generated definition
create table user
(
    id          varchar(256)                       not null comment 'id'
        primary key,
    username    varchar(50)                        not null comment '账户',
    pwd         varchar(256)                       not null comment '密码',
    permission  int                                not null comment '权限id',
    state       int                                null comment '状态 0-正常，1-冻结',
    salt        varchar(256)                       null comment '盐',
    create_by   varchar(256)                       null comment '创建人',
    create_time datetime default CURRENT_TIMESTAMP null comment '创建时间',
    update_by   varchar(256)                       null comment '更新人',
    update_time datetime default CURRENT_TIMESTAMP null on update CURRENT_TIMESTAMP comment '更新时间'
)
    comment '用户表' collate = utf8mb4_general_ci
                     row_format = DYNAMIC;


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

# 集成与单元测试

学习文章:[.Net单元测试xUnit和集成测试指南(1) - 董瑞鹏 - 博客园 (cnblogs.com)](https://www.cnblogs.com/ruipeng/p/18112221)

## XUnit

### 基础

单元测试通常遵循 AAA 模式， Arrange 准备、 Act 执行、 Assert 断言。方法命名遵循测试方法的名称+测试的方案+调用方案时的预期。**编写测试单元的时候避免出现额外的逻辑，如if、for等等**

```C#
	[Fact]
    public void Add_TwoNumbers_ReturnSum()
    {
        // Arrange
        var calculator = new MathCalculator();

        // Act
        var result = calculator.Add(3, 5);

        // Assert
        Assert.Equal(8, result);
    }
```

1. Fact属性：标记方法无需参数且不返回任何内容
2. Theory属性：表示方法可接受参数运行并允许多次，每次运行使用不同的参数值
3. InlineData属性：指定输入 Theory 标记的测试方法参数值，适用于静态、硬编码的测试数据集合
4. MemberData属性：从字段、属性或方法中获取测试数据用作方法入参
    - 标记测试方法：使用[Theory]属性标记，接收测试数据
    - 准备测试数据：创建公共静态字段、属性或方法，方法返回IEnumerable<object[]>，每个对象代表一组测试数据
    - 传递测试数据：在MemberData属性中指定使用的数据源，将数据传递給测试

```C#
public static IEnumerable<object[]> GetComplexTestData()
    {
        yield return new object[] { 10, 5, 15 };
        yield return new object[] { -3, 7, 4 };
        yield return new object[] { 0, 0, 0 };
    }

    [Theory]
    [MemberData(nameof(GetComplexTestData))]
    public void Add(int first, int second, int sum)
    {
        // Arrange
        var calculator = new Ma();
        // Act
        var result = calculator.Add(first, second);
        // Assert
        Assert.Equal(sum, result);
    }
}
```

5. 自定义属性：继承DataAttribute实现自定义Attribute

```C#
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CustomDataAttribute : DataAttribute
{
    private readonly int _first;
    private readonly int _second;
    private readonly int _sum;

    public CustomDataAttribute(int first, int second, int sum)
    {
        _first = first;
        _second = second;
        _sum = sum;
    }

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] { _first, _second, _sum };
    }
}

	[Theory]
    [CustomData(1, 2, 3)]
    [CustomData(2, 3, 5)]
    public void Add(int first, int second, int sum)
    {
        // Arrange
        var calculator = new Ma();
        // Act
        var result = calculator.Add(first, second);
        // Assert
        Assert.Equal(sum, result);
    }
```



### 输出

在 xUnit 中无法用 cw 输出控制台内容，需要用 ITestOutputHelper 完成控制台输出

### 驱动测试开发TDD

1. 在开发前，先编写一个或多个单元测试，这些单元测试因为没有实现所以失败，在此之后去生产代码实现业务逻辑
2. 在编写单元测试时只编写使测试失败的最小代码量，这样可知新写的代码是否解决了问题，编译不通过意味着代码无法进行
3. 编写生产代码时，只需要编写足够让失败的单元测试通过的代码，而不是一次性编写完整的功能

### Mock 与 Stub

> [掌握 xUnit 单元测试中的 Mock 与 Stub 实战 - 董瑞鹏 - 博客园 (cnblogs.com)](https://www.cnblogs.com/ruipeng/p/18130083)

### 集成测试

> [实战指南：使用 xUnit 和 ASP.NET Core 进行集成测试【完整教程】 - 董瑞鹏 - 博客园 (cnblogs.com)](https://www.cnblogs.com/ruipeng/p/18141877)

### 断言

```C#
// 检查两个对象或值是否相等
Assert.AreEqual(exp, act);

// 检查不相等
Assert.AreNotEqual(exp, act);

// 检查表达式是否为真
Assert.IsTrue(con);

// 检查表达式为假
Assert.IsFalse(con);

// 检查对象是否为Null
Assert.IsNull(obj);

// 对象不为Null
Assert.IsNotNull(obj);

// 检查对象是否为指定类
Assert.IsType<Type>(obj);

// 检查对象是否可分配給指定类
Assert.IsAssignableFrom<Type>(obj);

// 检查对象是否引用相同实例
Assert.AreSame(exp, act);

// 检查对象是否不引用相同
Assert.AreNotSame(exp, act);

// Lambda委托
Assert.That(condition);

// 检查执行操作是否异常
Assert.Throws<Type>(action);

// 异步检查异常
Assert.ThrowsAsync<Type>(action);
```

# Docker

## SQLServer Docker

1. docker pull mcr.microsoft.com/mssql/server:2022-latest

2. This program requires a machine with at least 2000 megabytes of memory.

3. docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Li123456" -p 1433:1433 --name mssql --hostname mssql -d mcr.microsoft.com/mssql/server:2022-latest 

## RabbitMQ

1. docker pull rabbitmq:management
2. docker run -id --name=rabbitmq -p 5671:5671 -p 5672:5672 -p 4369:4369 -p 15671:15671 -p 15672:15672 -p 25672:25672 -e RABBITMQ_DEFAULT_USER=root -e RABBITMQ_DEFAULT_PASS=123456 rabbitmq:management

# Git

Git与SVN都是版本控制系统，区别为一个是分布式另一个是集成式。Git的优势在没有网络时能进行提交代码的操作，其操作快速且轻量，可以处理复杂的合并。

## 基础指令

**Github上传code流程**

```
配置KEY：
git config --global user.name "<name>"
git config --global user.email "<email>"
生成密钥：
ssh-keygen -t rsa -b 4096 -C "<Email>"

初始化仓库：
git init
连接远程仓库：
git remote add origin <url>
跟踪文件：
git add <filename>/*
提交分支：
git commit -m '<info>'
推送代码，第一次需要-u参数：
git push -u origin master
```

---

其他常用指令

1. 取消跟踪文件：git rm <filename> 
2. 取消缓存状态：git reset HEAD <filename>
3. 取消本次提交：git reset head~ --soft （不能取消第一次提交）/ (head：指当前提交；head~代表上一次提交)
4. 修改远程仓库名字：git remote rename orig origin
5. 查看具体文件修改：git diff
6. 查看日志：git log，图形化展示：git log --graph
7. 查看分支：git branch --list    显示一行日志：git log --pretty=online
8. 切换分支：git checkout <branchName> / git switch master 
9. 新建并切换分支：git checkout -b <branchName> / git switch -c dev
10. 合并分支：git merge <branchName>
11. 推送分支：git push -u origin
12. 拉取分支：git pull 
13. 贮藏：git stash，将未提交的代码隐藏，解除：git stash apply
14. 从远程仓库拉分支到本地分支：git checkout -b dev origin/dev
15. 回滚：git reset --hard HEAD^
    - HEAD表示当前版本，上个版本是HEAD^，上上个版本是HEAD^^；`--hard`会回退到上个版本的已提交状态，而`--soft`会回退到上个版本的未提交状态，`--mixed`会回退到上个版本已添加但未提交的状态。
16. 查看历史命令：git reflog
17. 丢弃文件修改：git checkout -- readme.txt/git reset HEAD <file>
18. 添加到暂存区再撤销：git reset HEAD <file>；git checkout -- <file>
19. 删除文件：git rm <file>

---

### 提交代码流程图

git 提交的时候会将文件放到Stage暂存区，在commit后才会将文件提交到分支上，具体流程如图所示 

![image-20240906150310778](./csharp study record.assets/image-20240906150310778.png)

---

## 四区五状态

### 四区

1. 工作区-Working Area：
    - 直接编辑文件的地方，可对文件进行增删改等操作
2. 暂存区-Stage:
    - 暂存即将进行的更改
3. 本地仓库-Local Repository:
    - 工作区根目录下的.git文件中，本地仓库是执行git commit命令后保存提交的地方，也是git log查看历史的地方
4. 远程仓库-Remote Repository:
    - 位于服务器的git仓库中，push推送、pull和fetch拉去的地方

### 五状态

1. 未修改-Origin
    - git status命令查看不到未修改的文件
2. 已修改，未追踪-Modified、Untracked
    - git status会显示这些文件，新创建的文件才是未追踪
3. 已暂存-Staged
4. 已提交-Committed
5. 已推送-Pushed

![](./csharp study record.assets/7459409-a05dfa63fe8a11b4.webp)

---

## GitFlow工作流

### 常用分支说明

Production	生产分支，即 Master分支。只能从其他分支合并，不能直接修改
Release	发布分支，基于 Develop 分支创建，待发布完成后合并到 Develop 和 Production 分支去
Develop	主开发分支，包含所有要发布到下一个 Release 的代码，该分支主要合并其他分支内容
Feature	新功能分支，基于 Develop 分支创建，开发新功能，待开发完毕合并至 Develop 分支
Hotfix	修复分支，基于 Production 分支创建，待修复完成后合并到 Develop 和 Production 分支去，同时在 Master 上打一个tag

1. Master:
    - 主要分支，存放最稳定的正式版本，随时可用在生产环境中，任何人不允许在主要分支上进行代码直接提交，只接受其他分支的合并，原则上主要分支上的代码不许是合并经过多轮测试及已经发布一段时间且线上稳定的预发分支
2. Develop：
    - 开发分支，其更新的代码始终反映下一个版本要交付的功能，接受其他辅助分支的合入，合入开发分支必须保证功能完整，不影响开发分支的正常运行
3. Feature：
    - 功能分支，用于开发即将发布版本或未来版本的新功能，该分支只能拉取自开发分支
4. Release：
    - 预发分支，专为测试-发布新的版本，测试工程师进行测试再由开发工程师修复，只能拉取自开发分支，合并回开发分支和主要分支
5. Hotfix：
    - 热修复分支，当生产环境上的代码遇到严重到必须立即修复的缺陷时，就需要从主分支上指定的版本拉取热修复分支上的代码进行修复，并附上版本号，只能从主要分支上拉去

![](./csharp study record.assets/7a4c9ee492db52f9ccebbf1327c46c5f.png)

---

## 分支管理

#### 创建分支

```
创建并切换分支：
git checkout -b dev/git switch -c dev
查看当前分支：
git branch

切换分支：
git checkout master/git switch master
合并分支，dev=>master：
git merge dev
删除分支：
git branch -d dev


```

#### 分支冲突

发现分支冲突后 git 会进行提示，之后可以使用 vim 编辑冲突内容后再进行代码的提交，再用带参数的指令`git log`可以看到分支合并情况

- vi <fileName>  // 编辑代码
- git log  // 查看分支树

#### Bug分支

bug分支通常以编号命名，通常为`ISSUE-101`以此类推。但当正在进行的开发分支还没编写完成同时bug又急着修复等待上线该怎么办，这时候就有了`stash`储藏功能

- git stash  // 存储代码
- git status  // 查看工作区

``` 
存储dev代码：
git stash
假定要在master分支上修复：
git checkout master
创建bug分支并切换：
git checkout -b issue-101
// 省略modify

切换分支并完成合并：
git switch master
git merge -no-ff -m "merge bug fix 101" issue-101

// 恢复dev分支
git stash list
git stash pop / git stash apply stash@{0}

// 同步bug修复，复制提交
git cherry-pick <id（4c805e2 fix bug 101）>
```

#### Feature分支

新功能，新分支，开发后同一合并到主分支

----

## 忽略文件

在工作区创建`.gitignore`文件，将要忽略的文件名称填入进去。

1. 忽略文件原则：

- 忽略操作系统中自动生成的文件，eg.缩略图
- 忽略编译生成的中间文件、可执行文件，eg.Java的`.Class`
- 忽略敏感信息的配置文件，eg.口令配置

2. `.gitignore`文件上传失败

- git add -f App.class

3. 检查ignore文件规则

- git check-ignore -v App.class





## Merge与Rebase的区别

- **历史重写**：`rebase` 会重写提交历史，而 `merge` 不会。
- **复杂性**：`merge` 在处理冲突时通常更简单，因为它不需要重写历史。而 `rebase` 需要更多的注意，因为它会改变提交历史。
- **结果**：`merge` 通常会产生一个合并提交，而 `rebase` 产生的是一个线性历史。
- **协作影响**：在共享分支上使用 `rebase` 可能会导致问题，因为它会改变分支的历史，这可能会给其他协作者带来困扰。而 `merge` 不会改变已经存在的提交。



# Https

