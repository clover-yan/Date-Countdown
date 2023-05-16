# Date-Countdown

由 Clover Yan 基于 [WXRIW 的作品](https://github.com/WXRIW/Gaokao-Countdown) 制作的日期倒计时应用。

## 使用方法

运行程序，以显示高考倒计时。

你也可以通过参数进行自定义。所有的参数都是可选的。

### 参数 1

指定将在计数数字前显示的**描述文本**。

默认值为 `距离高考还有`。

> **请注意：如果想要不传递此参数，而传递之后的参数，请将此参数留空（设为 `""`）。**

### 参数 2 ~ 7

指定**目标年、月、日、时、分和秒**。

默认值为 __下一个 6 月 7 日的 9:00__。

> **请注意：一旦需要指定日期和 / 或时间，必须指定参数 2 至参数 7 的所有参数。**

### 参数 8

指定计数的**小数位数**。

默认值为 `3`。

### 其他参数

#### `-c`

隐藏版权信息。

#### `-t`

在任务栏中显示。

#### `-r` / `-g`

以红色 / 绿色显示计数。

`-r` 会覆盖 `-g` 的设置。

省略这两个参数意味着依天数而定（剩余计数不足 100 天时显示红色，否则显示绿色）。

#### `-l`

亮色模式（以白色显示描述文本）。

#### `-n`

显示负数前的负号。

#### `-b`

添加发光效果（跟随颜色模式）。

#### `-p`

置顶窗口。

如果前台窗口是全屏的，窗口将会有呼吸效果。

#### `-a`

将文字设为半透明。

#### `-k`

启用鼠标穿透。
