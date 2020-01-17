# p2p聊天程序
本项目用c#的socket编程完成。
## 可执行程序
可执行程序带有UI用户界面，在Windows系统下打开“可执行程序\client_tcp.exe”来使用。
除了客户端程序外还有模拟服务器server和udp客户端client_udp的可执行程序
UI界面的使用方法在报告中“界面设计”部分详述。
## code
项目代码，包括三个文件夹：
- Net_client：客户端程序的编写
- Net_server：模拟服务器程序的编写
- Net_server_udp：客户端程序，文字聊天使用UDP协议
### Net_client
- login: 登录界面
- FriendList: 好友列表界面
- chatWindow: 聊天窗口界面
- groupChatWindow：群聊窗口界面
- Net_class：网络通信所用的类，包括socket编程中一些复用较多的函数。