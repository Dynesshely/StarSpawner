
``` shell
                                                |         o   |         |              
,---.,---.,---.,---.,---.,---.,---.,---.,---.   |--- ,---..   |---..   .|--- ,---.,---.
|    |   ||   ||    |   ||   ||    |   ||   |---|    |    |---|   ||   ||    |---'|    
`---'`---'`   '`---'`---'`   '`---'`---'`   '   `---'`    `   `---'`---'`---'`---'`    
                                                                                       
```

<p align="center">
  <a hrdf="./LICENSE"><img src="https://img.shields.io/github/license/Dynesshely/conconcon-tri-buter?style=for-the-badge"></img></a>
  <a hrdf=""><img src="https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white"></img></a>
  <a hrdf=""><img src="https://img.shields.io/badge/Linux-FCC624?style=for-the-badge&logo=linux&logoColor=black"></img></a>
  <a hrdf=""><img src="https://img.shields.io/badge/mac%20os-000000?style=for-the-badge&logo=macos&logoColor=F0F0F0"></img></a>
</p>

<hr>

# 中文
## conconcon-tri-buter
想要装13吗? 来这就对了!

## 发布
本地编译执行此命令, 基于 .NET 5
<br>
请确保安装了有效的 sdk
``` PS
dotnet publish -r win-x86 -c release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## 使用
### 简介
菜单目前有四项
1. Simply contribute every selected day
2. Simply contribute with random lively commit message
3. Lively contribute with lively commit message
4. Lively contribute with lively commit message with density
选择一项并回车
功能分别对应:
1. 从即日起前 n 天内每天提交 r 次 (r 可选固定不限次或是随机, 随机最多 40 次 (需要更多的请自行更改源码并编译) ), 并且提交消息为随机字符串
2. 功能类似 `选项 1`, 并且提交消息为拟真消息, 格式如下:
  1. type(scope): subject
  2. type   : 类型 (feat, fix, docs, style, refactor, test, chore)
  3. scope  : 变动范围, 可选. 多为文件名或目录
  4. subject: 简要概述变动内容及作用
  5. 此模板参照 [阮一峰老师の博客](http://www.ruanyifeng.com/blog/2016/01/commit_message_change_log.html) 生成
3. 功能类似 `选项 1`, 允许自定结束日期, 而非从即日计算, 并且提交消息为拟真消息
4. 具有密度功能的拟真提交(🎷吹爆!): 允许自定开始日期与结束日期, 需要设置密度, 允许设定每日最多提交数, 并且可选是固定最多提交数还是随机浮动提交数

## 它是如何工作的
已知 git commit 命令允许使用 --date 参数指定提交日期
而 GitHub 计算 Contribute 的方式即通过 commit 的日期计算
我们仅需创建一个文件, 提交
再删除这个文件, 如此往复并指定日期即可以假乱真
最后推送, GitHub 的 Contribution 图就可以装13啦!

# English
## conconcon-tri-buter
want to zhuang 13? come here.

## publish
``` PS
dotnet publish -r win-x86 -c release -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

## use
1. go to release and download the latesed
2. copy the release single file directly to your `.git` root directory
3. run it
4. input the day num count from today to before
5. input contributions num per day you want (input 'r' for random contributions)
6. press enter then wait it finished
7. delete this single app
8. handly commit once and push it !
9. go to your github homepage and see what happened
10. enjoy it

## how it worked
by directly create a random file and commit this change again and again
and append `--date` to set date
then finally push all

## besides
any results that you use this program caused didn't attach to me, okay?
