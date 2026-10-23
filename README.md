# RA2 Map Spawner — 使用与代码说明

> 原项目简介:This project enables users to customize settings to generate maps tailored for AI-vs-AI spectator battles (colloquially known as "digital cockfighting") within the Mental Omega 3.3 mod for Command & Conquer: Yuri's Revenge.

本文档以 **GitHub Releases 提供的两个发布版**(免安装版 / 框架依赖版)作为主要使用入口,随后说明仓库结构与代码实现,帮助新参与者快速接手。

## 目录

- [1. 这是什么](#1-这是什么)
- [2. 快速开始](#2-快速开始)
- [3. 仓库文件清单](#3-仓库文件清单)
- [4. 代码功能详解](#4-代码功能详解)
- [5. 构建与发布](#5-构建与发布)
- [6. 常见问题(FAQ)](#6-常见问题faq)
- [7. 已知限制](#7-已知限制)
- [8. 修改指引](#8-修改指引)
- [9. 旧版 C++ 控制台程序](#9-旧版-c-控制台程序)
- [10. 许可](#10-许可)

---

## 1. 这是什么

一个给《命令与征服:尤里的复仇》(Mental Omega 3.3 等 mod)做 **AI 对战观战地图** 的小工具。

它的工作方式非常固定,可以概括为:

```
FORMS\FORM1.map / FORM2.map / FORM3.map(模板三选一,原样保留)
        +
[Units] / [Infantry] 两个生成区块(参数化批量生成)
        ↓
<自定义位置>\<自定义文件名>.map(默认 MAPS\test.map,每次保存覆盖)
```

- **模板** `FORMS\FORM1~3.map` 是一张“空地图”的全部 INI 段,程序**只读不改**;用“保存设置”里的 **预设1/2/3** 选择用哪一份;
- 用户在界面上选择类型、填写参数、点“追加生成”,程序算出每个单位/步兵的坐标并累积;
- 点“保存地图”时,把 **所选模板全文 + 生成区块** 拼成一个新的地图文件,写到“保存设置”里指定的**位置**与**文件名**;
- 把生成的地图放进游戏/map 编辑器即可看到刷出来的部队。

> 程序只负责“往地图末尾追加 `[Units]` / `[Infantry]` 段”,不修改模板中的其他内容。

---

## 2. 快速开始

### 2.0 两个下载版本,任选其一

两个版本的**程序功能完全相同**,区别只在运行环境要求:

| 版本 | Release 压缩包 | 体积 | 需要 .NET? | 选择建议 |
|---|---|---|---|---|
| **免安装版**(self-contained) | `MapSpawner_selfcontained_win-x64.zip` | 约 43 MB | **不需要** | 首选。拷到任意 64 位 Windows 即可运行 |
| **框架依赖版**(FDD) | `MapSpawner_framework-dependent_win-x64.zip` | 约 0.7 MB | 需 **.NET 9 Desktop Runtime (x64)** | 本机已装 .NET、想要小体积时选它 |

两者都在仓库的 [Releases 页面](https://github.com/G-Cabbager/Map_Spawner/releases) 下载,**解压后双击 `MapSpawnerGUI.exe` 即可使用**,无需安装。

> 解压后请**整个文件夹一起使用**:`FORMS`、`MAPS` 必须与 `MapSpawnerGUI.exe` 同级(见 2.2)。
> 想自己从源码构建,见第 5 节。

### 2.1 为什么用 `PublishGUI-SC`

`PublishGUI-SC` 是 **self-contained(自包含)+ 单文件** 发布:

| 特性 | 说明 |
|---|---|
| 无需安装 .NET | 运行时已打包进 exe,拷到任意 64 位 Windows 即可运行 |
| 只有一个程序文件 | `MapSpawnerGUI.exe`(约 48.6 MB),运行时库已压缩内嵌 |
| 首次启动稍慢 | 单文件需自解压到临时目录,约 1~2 秒,属正常现象 |
| 数据仍外置 | `FORMS`、`MAPS` 两个文件夹必须与 exe 同级(见下) |

### 2.2 目录结构

```
PublishGUI-SC\
├── MapSpawnerGUI.exe       ← 双击这个启动
├── FORMS\
│   ├── FORM1.map           ← 预设1 用的地图模板(只读,勿删)
│   ├── FORM2.map           ← 预设2(新建时为空文件)
│   └── FORM3.map           ← 预设3(新建时为空文件)
└── MAPS\
    └── test.map            ← 默认输出(可在“保存设置”里改位置和文件名)
```

> ⚠️ **三个硬性注意**
> 1. 必须**整个文件夹**一起拷贝/分发,**不要只拷 exe**;
> 2. `FORMS`、`MAPS` 必须与 `MapSpawnerGUI.exe` **同级**;
> 3. 若所选模板缺失,程序会弹警告并**只输出生成区块**(地图不完整)。

### 2.3 操作流程

1. 把 `PublishGUI-SC` 整个文件夹复制到任意位置(建议路径不含特殊符号);
2. 双击 `MapSpawnerGUI.exe`;
3. 顶部选择 **单位 (Unit)** 或 **步兵 (Infantry)**;
4. 填写通用参数与类型参数,设置布局参数(方向/角坐标/行数/总数);
5. 点 **追加生成** —— 可反复调整参数后多次追加,结果会累积;
6. 在右侧 **保存设置** 里选 **预设1/2/3**(对应 `FORM1/2/3.map`),填 **文件名** 与 **输出位置**;
7. 点 **保存地图**,弹窗会显示完整输出路径、所用模板与条数。

> 目标文件已存在时会被**直接覆盖**。`输出位置` 填相对路径(如 `MAPS`)按程序所在目录解析,填绝对路径(如 `D:\Games\RA2\Maps`)则原样写入,便于直接输出到游戏地图目录。

### 2.4 输出位置由什么决定

`Form1.ResolveDataRoot()` 从 **exe 所在目录** 开始逐级向上查找“包含 `FORMS\FORM1.map` 的目录”:

- 免安装版:`FORMS` 就在 exe 旁边 → **第一级命中**,数据根 = `PublishGUI-SC\`;
- 仓库内开发运行(`bin\Debug\net9.0-windows`):会逐级向上找到**仓库根**,读写仓库里的 `FORMS` / `MAPS`;
- 全都找不到:退回 exe 所在目录(此时模板缺失,仅有生成区块)。

因此输出路径 **不依赖“从哪个目录启动”**,只依赖 exe 的位置。

---

## 3. 仓库文件清单

### 3.1 仓库布局

仓库**只放源码与数据模板**。`PublishGUI-SC/`、`PublishGUI/` 这类可执行产物**不进仓库**,而是打包成 zip 由 [Releases 页面](https://github.com/G-Cabbager/Map_Spawner/releases) 分发——否则每次 `git clone` 都要多拉 50 MB 以上。

| 路径 | 作用 |
|---|---|
| `FORMS/FORM1.map` | 地图模板(约 84 KB),预设1 使用。程序读取后原样作为输出开头 |
| `FORMS/FORM2.map`、`FORM3.map` | 预设2 / 预设3 的模板,仓库内为**空文件**,放入自己的地图即生效 |
| `MAPS/test.map` | 默认输出文件(约 94 KB)。按“保存设置”默认值保存时会重写它 |
| `MapSpawnerGUI/` | **主程序源码**(.NET 9 WinForms),详见 3.2 |
| `main.cpp` / `main.exe` | 早期 C++ 控制台版本,算法参考实现,详见第 9 节 |
| `.vscode/tasks.json` | 用 MinGW `g++` 编译 `main.cpp` 的构建任务(与 GUI 无关) |
| `.gitignore` | 忽略 `bin/`、`obj/`、`PublishGUI/`、`PublishGUI-SC/` 等可再生产物 |
| `LICENSE` | 开源许可 |
| `README.md` | 本文档 |

> `PublishGUI-SC` 是**自包含免安装版**,`PublishGUI` 是**框架依赖版**,二者均由第 5 节的发布命令生成,不纳入版本控制。

### 3.2 `MapSpawnerGUI/` 源码目录

| 文件 | 作用 |
|---|---|
| `Program.cs` | 程序入口:初始化 WinForms、启动 `Form1`,并对启动异常弹窗兜底 |
| `Form1.cs` | 主窗体逻辑:收集界面参数、累积实体、刷新预览/状态、保存文件、解析数据根 |
| `Form1.Designer.cs` | 界面布局与控件定义、事件绑定、窗体图标加载(设计器生成,手改需谨慎) |
| `Generator.cs` | **核心算法**:坐标排布、实体批量生成、`[Units]`/`[Infantry]` 文本输出 |
| `Models.cs` | 数据模型:`EntityType` 枚举与 `BaseEntity` 实体字段 |
| `MapSpawnerGUI.csproj` | 工程文件:`WinExe` / `net9.0-windows` / WinForms / 图标与内嵌资源 |
| `app.ico` | 程序图标(多尺寸,由 `make_icon.ps1` 生成,约 184 KB) |
| `iconic.png` | 图标源图(1080×1080),换图标时替换它 |
| `iconic.jpg` | 源图的另一格式备份(不参与构建) |
| `make_icon.ps1` | PNG → ICO 转换脚本(输出 256/128/64/48/32/24/16 七种尺寸) |
| `bin/`、`obj/` | 构建中间产物与输出,可安全删除后重建 |
| `MapSpawnerGUI.csproj.user` | 本机 VS 配置(不参与发布) |

---

## 4. 代码功能详解

### 4.1 启动链路

```
Program.Main()
  ├─ ApplicationConfiguration.Initialize()   // WinForms 默认配置(DPI、字体等)
  ├─ Application.Run(new Form1())
  │     └─ InitializeComponent()             // 构建界面 + 事件绑定 + 加载窗体图标
  │           ├─ EntityTypeChanged           // 切换单位/步后面板
  │           ├─ BtnGenerate_Click           // 追加生成
  │           ├─ BtnSave_Click               // 保存地图
  │           └─ BtnClear_Click              // 清空
  └─ catch(Exception) → MessageBox 显示类型/消息/堆栈
```

`Program.cs` 的 try/catch 是为了在“双击无反应”时也能看到真实异常,方便排查环境问题。

### 4.2 界面参数对照

| 控件 | 名称 | 默认值 | 范围 | 含义 |
|---|---|---|---|---|
| 生成类型 | `radioUnit` / `radioInfantry` | 单位 | 二选一 | 切换“单位参数 / 步兵参数”面板 |
| 所属方 | `txtOwner` | `Europeans House` | 任意文本 | 写入每行的 Owner 字段 |
| 类型代码 | `txtType` | `MTNK` | 任意文本 | 写入 Type 字段,如 `MTNK`、`E1` |
| 朝向(单位) | `numFacing` | 32 | 0–255 | 单位 Facing |
| 经验等级(单位) | `numVeterancy` | 0 | 0–200 | 通常 0=新兵、1=老兵、2=精英 |
| 分组(单位) | `numGroup` | -1 | -1–9 | -1 表示无分组;0–9 对应游戏内 1–10 |
| 站位(步兵) | `numSubPos` | 1 | 1–3 | 步兵 SubPos |
| 经验等级(步兵) | `numVeterancyInf` | 0 | 0–200(步进 100) | 常用 0 / 100 / 200 |
| 分组(步兵) | `numGroupInf` | -1 | -1–9 | 同上 |
| 方向 | `cmbDirection` | 右向 | 左向 / 右向 | 决定排布锚点与行内延伸方向 |
| 角 X | `numCornerX` | 10 | 0–1000 | 排布基准角坐标 X |
| 角 Y | `numCornerY` | 10 | 0–1000 | 排布基准角坐标 Y |
| 行数 | `numRows` | 3 | 1–100 | 期望排成几行 |
| 总数 | `numTotal` | 6 | 1–1000 | 本次生成多少个 |
| 预设 | `cmbPreset` | 预设1 | 预设1 / 预设2 / 预设3 | 选择 `FORMS` 下用哪份模板(FORM1/2/3.map) |
| 文件名 | `txtFileName` | `test` | 任意文本 | 输出文件名;不带 `.map` 会自动补上 |
| 输出位置 | `txtOutputDir` | `MAPS` | 任意文本 | 相对路径按数据根解析;绝对路径原样使用 |
| 追加生成 | `btnGenerate` | — | — | 按当前参数生成一批并**累加**到已有结果 |
| 保存地图 | `btnSave` | — | — | 所选模板 + 全部累积区块 → 按“保存设置”写出文件 |
| 清空 | `btnClear` | — | — | 清空**全部**累积(单位与步兵一起清) |
| 输出预览 | `txtOutput` | — | 只读 | 仅显示生成区块(不含模板),等宽字体 |
| 状态栏 | `lblStatus` | — | — | 显示两类实体的条数与序号范围 |

> 单位与步兵的“经验等级/分组”是两套独立控件,切换类型时各自保留数值。

### 4.3 坐标排布算法(`Generator.GeneratePositions`)

参数:`isRightDirection`(方向)、`cornerX/cornerY`(角坐标)、`rows`(行数)、`total`(总数)。

```csharp
CeilDiv(a,b) = (a + b - 1) / b;          // 向上取整
cols        = CeilDiv(total, rows);      // 至少能放下的列数
baseCount   = total / rows;              // 每行基础数量
remainder   = total % rows;              // 余数:需要多放 1 个的行数
startFullRow= (rows - remainder) / 2;    // 让“多 1 个”的行居中分布
```

锚点换算:

| 方向 | 左上基准 X | 左上基准 Y | 每行取列区间 |
|---|---|---|---|
| 右向 | `cornerX - (cols - 1)` | `cornerY + (cols - 1)` | `[cols - numInRow, cols)` |
| 左向 | `cornerX - (rows - 1)` | `cornerY - (rows - 1)` | `[0, numInRow)` |

逐点坐标:

```csharp
rowBaseX = topLeftX + r;      // 第 r 行
rowBaseY = topLeftY + r;
x = rowBaseX + c;             // 行内 Δx = +1
y = rowBaseY - c;             // 行内 Δy = -1
```

即:**同一行内的点沿 (Δx=+1, Δy=-1) 斜向排列;换行时基准点沿 (+1,+1) 平移**。
余数行被安排在中间(`startFullRow` 起连续 `remainder` 行),使整体尽量居中。

> ⚠️ 注意左右两侧锚点使用的量不同:右向按**列数** `cols` 偏移,左向按**行数** `rows` 偏移。当 `rows` 与 `cols` 相差较大时,两种方向的视觉位置会明显不同——调整参数后请先看“输出预览”里的坐标再保存。

### 4.4 实体生成(`Generator.GenerateEntities`)

对每个坐标复制一份模板参数,并按类型补默认值:

| 字段 | 单位(Unit) | 步兵(Infantry) |
|---|---|---|
| `Layer` | `-1` | `0` |
| `Reserved1` | 沿用模板(默认 1) | 固定 `1` |
| `Facing` | 界面“朝向”(默认 32) | 固定 `64`(界面无朝向输入) |
| `SubPos` | `1` | 界面“站位”(默认 1) |

### 4.5 数据模型(`Models.cs`)

`BaseEntity` 字段与默认值:

| 字段 | 默认 | 说明 |
|---|---|---|
| `Owner` | `""` | 所属方 |
| `Type` | `""` | 类型代码 |
| `Health` | `256` | 生命值(界面不可改,固定 256) |
| `X` / `Y` | 0 | 由算法写入 |
| `Facing` | `32` | 朝向 |
| `SubPos` | `1` | 步兵站位 |
| `Status` | `Guard` | 状态 |
| `Tag` | `None` | 标签 |
| `Veterancy` | 0 | 经验等级 |
| `Group` | `-1` | 分组 |
| `Layer` | `-1` | 层级(单位 -1 / 步兵 0) |
| `Reserved1` | `1` | 占位字段 |
| `Reserved2` | `0` | 占位字段 |

### 4.6 输出格式(`Generator.WriteUnitsBlock` / `WriteInfantryBlock`)

每个区块首行是段名,之后每行 `序号=字段1,字段2,...`。**两个区块的序号都各自从 0 开始**。

`[Units]` 每行共 14 项:

| 位置 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 含义 | Owner | Type | Health | X | Y | Facing | Status | Tag | Veterancy | Group | 固定 `0` | Layer | 固定 `1` | Reserved2 |

格式示意(尖括号为变量):

```
0=<Owner>,<Type>,256,<X>,<Y>,<Facing>,Guard,None,<Veterancy>,<Group>,0,-1,1,0
```

`[Infantry]` 每行共 14 项:

| 位置 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 含义 | Owner | Type | Health | X | Y | SubPos | Status | Facing | Tag | Veterancy | Group | 固定 `0` | Reserved1(`1`) | Reserved2 |

格式示意:

```
0=<Owner>,<Type>,256,<X>,<Y>,<SubPos>,Guard,64,None,<Veterancy>,<Group>,0,1,0
```

> ⚠️ **两个区块的字段顺序不同**:`[Units]` 的 `Facing` 在第 6 位、`Tag` 之后是 `Veterancy`;`[Infantry]` 的 `SubPos` 在第 6 位、`Facing` 在第 8 位(位于 `Status` 之后)。
>
> 另:早期 `main.cpp` 生成的 `[Units]` 行在末尾几项的顺序与当前 GUI 不同(旧版第 13 项是 `Veterancy`,现 GUI 第 13 项固定为 `1`)。**以 `Generator.cs` 为准**;仓库里现存的 `MAPS\test.map` 可能是旧版产物,不要拿它当格式样板。

### 4.7 保存流程(`Form1.BtnSave_Click`)

1. 若单位与步兵都为空 → 弹提示,不写文件;
2. `ResolveDataRoot()` 定位数据根(见 2.4);
3. 按 `cmbPreset.SelectedIndex` 选模板:`0/1/2` → `FORMS\FORM1/2/3.map`;若不存在 → 弹警告,继续(仅输出生成区块);
4. 模板末尾若无换行则补一个,避免与 `[Units]` 粘连;**空模板(0 字节)不补**,以免输出开头多出空行;
5. 依次追加 `[Units]`、`[Infantry]` 区块(有条目才追加);
6. 规整输出文件名:去掉首尾空白,为空则退回 `test`,未以 `.map` 结尾则自动补上;
7. 解析输出位置:`Path.IsPathRooted` 为真则原样使用,否则拼到数据根下(默认 `MAPS`);
8. `Directory.CreateDirectory(...)` 确保目录存在(支持多级子目录);
9. `File.WriteAllText` 写入 `(输出位置)\(文件名).map`(UTF-8;换行由 `AppendLine` 产生 CRLF);
10. 弹窗显示**绝对路径**、所用模板名与两类实体条数。

### 4.8 预览、状态与清空

- `UpdatePreview()`:用 `StringBuilder` 依次拼接 `[Units]`、`[Infantry]` 文本,写入只读的 `txtOutput`。**预览不含模板内容**,只用于核对坐标与字段;
- `UpdateStatus()`:状态栏显示 `[Units] N 条 (序号 0~N-1) | [Infantry] M 条 (...)`;无数据时显示 `0 条`;
- `BtnClear_Click`:一次性清空两类累积并刷新预览/状态;无数据时直接返回(不弹窗)。

### 4.9 图标链路

```
iconic.png (1080×1080 源图)
   │  powershell -ExecutionPolicy Bypass -File make_icon.ps1 -Png iconic.png -OutIco app.ico
   ▼
app.ico (256/128/64/48/32/24/16,PNG 条目,支持透明)
   │  MapSpawnerGUI.csproj:
   │    <ApplicationIcon>app.ico</ApplicationIcon>              → exe 文件图标
   │    <EmbeddedResource Include="app.ico" LogicalName="MapSpawnerGUI.app.ico" /> → 内嵌资源
   ▼
Form1.Designer.cs 末尾:
   Assembly.GetManifestResourceStream("MapSpawnerGUI.app.ico") → Icon = new Icon(stream)
                                                              → 窗口标题栏/任务栏图标
```

`LogicalName` 必须与代码里的资源名完全一致,否则窗体图标为空(exe 图标仍然正常)。

### 4.10 异常处理

- `Program.cs`:启动阶段异常 → 弹窗显示异常类型/消息/堆栈;
- `BtnGenerate_Click` / `BtnSave_Click`:各自 try/catch,失败时弹“生成出错 / 保存失败”,不会导致程序退出。

---

## 5. 构建与发布

### 5.1 环境要求

- **开发机**:.NET SDK 9.0 及以上(`dotnet --version` 应为 9.x);
- **目标机(免安装版)**:64 位 Windows,**无需**安装任何 .NET;
- **目标机(框架依赖版)**:需安装 **.NET 9 Desktop Runtime (x64)**。

### 5.2 开发构建

在**仓库根**执行:

```powershell
dotnet build MapSpawnerGUI\MapSpawnerGUI.csproj -c Debug
```

产物:`MapSpawnerGUI\bin\Debug\net9.0-windows\MapSpawnerGUI.exe`。
直接运行该 exe 时,`ResolveDataRoot()` 会向上找到仓库根,读写仓库里的 `FORMS` / `MAPS`(见 2.4)。

### 5.3 发布免安装版(生成 Release 附件)

```powershell
dotnet publish MapSpawnerGUI\MapSpawnerGUI.csproj -c Release -r win-x64 --self-contained true `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:EnableCompressionInSingleFile=true `
  -p:DebugType=None `
  -o PublishGUI-SC
```

参数含义:

| 参数 | 作用 |
|---|---|
| `-r win-x64` | 目标平台为 64 位 Windows |
| `--self-contained true` | 把 .NET 运行时一起打包,目标机无需安装 |
| `PublishSingleFile=true` | 合并为单个 `MapSpawnerGUI.exe` |
| `IncludeNativeLibrariesForSelfExtract=true` | 原生库随单文件自解压(单文件运行所需) |
| `EnableCompressionInSingleFile=true` | 压缩内嵌运行时,体积更小、首次启动略慢 |
| `DebugType=None` | 不生成 `.pdb` 调试符号 |

产物约 **48.6 MB**,只有一个 exe。发布**不会自动带上数据目录**,需手动补齐并打包:

```powershell
Copy-Item FORMS PublishGUI-SC\ -Recurse -Force
Copy-Item MAPS  PublishGUI-SC\ -Recurse -Force
Compress-Archive -Path PublishGUI-SC -DestinationPath MapSpawner_selfcontained_win-x64.zip -Force
```

把生成的 zip 上传到 [Releases](https://github.com/G-Cabbager/Map_Spawner/releases) 即可。最终结构与第 2.2 节一致。

### 5.4 发布框架依赖版(体积小)

```powershell
dotnet publish MapSpawnerGUI\MapSpawnerGUI.csproj -c Release -o PublishGUI
Copy-Item FORMS PublishGUI\ -Recurse -Force
Copy-Item MAPS  PublishGUI\ -Recurse -Force
Compress-Archive -Path PublishGUI -DestinationPath MapSpawner_framework-dependent_win-x64.zip -Force
```

产物为多文件目录(含 `MapSpawnerGUI.dll` 等),目标机必须装有 .NET 9 Desktop Runtime,否则双击会提示缺少运行时。压缩后约 **0.7 MB**,适合“本机已装 .NET、只想快速更新”的场景。

### 5.5 发布注意事项

- **先关闭正在运行的程序**:若 `PublishGUI\MapSpawnerGUI.dll` 被占用,`dotnet publish` 会出现 `MSB3026` 复制重试甚至失败。关闭所有 `MapSpawnerGUI.exe` 进程后重试即可;
- 两个发布目录互不影响,可同时存在;
- 若资源管理器仍显示旧图标,执行 `ie4uinit.exe -show` 或重启资源管理器刷新图标缓存。

---

## 6. 常见问题(FAQ)

**Q1:保存成功,但文件在哪?**
看保存成功弹窗里的**绝对路径**——它就是最终位置。文件落在“保存设置 → 输出位置”里填的目录:`MAPS` 这类相对路径按程序所在目录解析,绝对路径则原样使用。

**Q2:怎么输出多份、互不覆盖?**
改“保存设置 → 文件名”即可,不必另存为。留空会退回 `test`;未写 `.map` 会自动补上。

**Q3:双击没反应,或提示缺少 .NET?**
请使用 `PublishGUI-SC` 免安装版。`PublishGUI` 是框架依赖版,必须安装 .NET 9 Desktop Runtime。

**Q4:首次启动明显慢?**
自包含单文件需要先把内嵌运行时解压到临时目录,1~2 秒属正常,之后启动会快。

**Q5:杀毒软件报毒/拦截?**
自包含单文件打包方式可能触发启发式误报,可加入信任;或改用 `PublishGUI`(框架依赖、多文件)版本。

**Q6:保存时提示“未找到 FORMS\FORMn.map”?**
提示里的路径就是程序实际查找的位置。确认 `FORMS` 下有所选预设对应的模板(FORM1/2/3.map)且与 exe 同级;不要单独移动 exe。缺模板时程序仍会写文件,但**不包含基础地图内容**。

> `FORM2.map`、`FORM3.map` 在仓库里是空文件,选“预设2/预设3”保存会得到**只有生成区块、没有基础地图**的结果——这是预期行为,放入自己的地图模板后才会完整。

**Q7:改了图标但界面/文件还是旧图标?**
exe 图标需重新构建才更新;资源管理器缓存可用 `ie4uinit.exe -show` 刷新。

**Q8:发布时出现 MSB3026、文件被占用?**
有正在运行的 `MapSpawnerGUI.exe` 锁住了输出目录里的 dll,关闭后重试。

**Q9:生成后进游戏看不到单位?**
先用“输出预览”核对坐标与类型代码;确认保存的是最新文件(看弹窗里的绝对路径);再核对地图编辑器/游戏读取的路径。坐标过大可能落在地图边界外。

---

## 7. 已知限制

1. **不做自动备份**:目标文件存在时直接覆盖,覆盖前不会提示;想留多份请自己在“文件名”里区分;
2. **`FORM2.map` / `FORM3.map` 默认为空**:选预设2/3 保存得到的是“只有生成区块”的文件,需自行放入地图模板;
3. **Health 固定 256**,`Status` 固定 `Guard`,`Tag` 固定 `None`,界面不可改;
4. **步兵朝向固定 64**(界面没有朝向输入);
5. **清空是全清**:单位与步兵的累积会一起被清掉,没有分类型清空或撤销;
6. **左右方向锚点算法不对称**(右向按列数偏移、左向按行数偏移),`rows` 与 `cols` 相差大时位置差异明显;
7. **不做坐标合法性校验**:角坐标 + 行数/总数过大时可能生成到地图边界外;
8. **仅支持 64 位 Windows**(发布目标为 `win-x64`);
9. **单文件 exe 首次启动较慢**(自解压),且可能被杀软启发式误报;
10. **旧 C++ 版与 GUI 版 `[Units]` 字段顺序不同**,不要混用两版的格式假设(见 4.6)。

---

## 8. 修改指引

| 想改什么 | 改哪里 | 注意事项 |
|---|---|---|
| 换基础地图模板 | 覆盖 `FORMS\FORM1.map`(或 FORM2/FORM3.map) | 发布目录里的模板也要一起更新;模板末尾建议保留换行 |
| 增加预设数量 | `Form1.PresetForms` 数组 → `Form1.Designer.cs` 的 `cmbPreset.Items` | **两处顺序必须一一对应**;再放一份 `FORMn.map` 到 `FORMS` |
| 换程序图标 | 替换 `MapSpawnerGUI\iconic.png`,运行 `make_icon.ps1` 重新生成 `app.ico` | 之后必须重新构建/发布,exe 图标才会更新 |
| 改排布规则 | `Generator.GeneratePositions` | 改完用“输出预览”核对坐标;同步更新本文 4.3 |
| 改输出字段/顺序 | `Generator.WriteUnitsBlock` / `WriteInfantryBlock` | **务必同步更新本文 4.6**,并确认游戏实际解析格式 |
| 增加界面参数 | `Form1.Designer.cs` 加控件 → `Form1.BtnGenerate_Click` 读取 → `BaseEntity` 加字段 | 设计器文件手改时注意控件初始化顺序 |
| 改用“浏览”按钮选目录 | `grpSave` 里加按钮 → `FolderBrowserDialog` 写回 `txtOutputDir` | 目前输出位置是纯手填文本框 |
| 支持分类型清空 | `Form1.BtnClear_Click` | 需新增按钮或按当前选中类型清理 |
| 换 .NET 版本 | `MapSpawnerGUI.csproj` 的 `TargetFramework` | 目标机运行时要求随之变化;免安装版不受影响 |

> 改完代码后:先用 5.2 重新构建验证,再按 5.3 / 5.4 重新发布两个版本,把新 zip 上传到 Releases。`FORMS` / `MAPS` 有改动时记得一并同步进发布目录。

---

## 9. 旧版 C++ 控制台程序

仓库根目录的 `main.cpp` / `main.exe` 是 GUI 出现之前的**命令行版本**,保留作为算法参考:

- 编译:MinGW `g++ -std=c++11`(见 `.vscode/tasks.json`),产物 `main.exe`(约 2 MB);
- 交互:先输入“要生成的组数”,再逐组输入类型、所属方、类型代码、经验/分组、方向、角坐标、行数、总数;
- 读写位置:相对**当前工作目录**的 `FORMS/FORM1.map` 与 `MAPS/test.map`(与 GUI 的“exe 目录解析”不同,必须在仓库根运行);
- 算法:与 GUI 的 `Generator.cs` 相同(同一套 `ceilDiv` / 行数均分 / 斜向排布);
- 输出差异:`[Units]` 末尾字段顺序与当前 GUI 不同(见 4.6 的提示),`[Infantry]` 基本一致。

> 日常使用请直接用 GUI 免安装版;C++ 版仅用于对照算法或应急。

---

## 10. 许可

本项目使用仓库根目录 `LICENSE` 中的许可条款,使用/分发前请先阅读。

---

### 文档维护提示

- 修改 `Generator.cs` 的字段顺序或界面参数时,**请同步更新本文第 4.2、4.5、4.6 节**;
- 修改发布参数(如目标框架、RID、单文件选项)时,请同步更新第 5 节;
- 本文示例中的数值(文件大小、默认值)以当前版本为准,版本升级后可能变化。

