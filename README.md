# ClientUpdate
Common c/s Software Update program

Required configuration file format
```xml
<?xml version="1.0" encoding="UTF-8"?>
<ClientUpdate>
  <!--版本号-->
  <Version>1.0</Version>

  <!--更新内容指向的url-->
  <ContentUrl>http://www.baidu.com/</ContentUrl>

  <!--更新内容说明-->
  <Content>更新内容如下：</Content>

  <!--更新文件地址-->
  <FileUrl>
	http://localhost:8010/ClientUpdate/ClientUpdate.zip
  </FileUrl>

  <!--更新完之后需要启动的程序 不带.exe（相对于更新程序）-->
  <Start>ClientUpdate.Test</Start>

  <!--更新完之后需要删除的文件(多个文件使用英文分号做分隔，相对于更新程序)-->
  <Delete/>

  <!--更新辅助脚本位置(相对于更新程序 不写则不执行)-->
  <ScriptUrl/>
  
  <!--更新辅助脚本解密公钥-->
  <ScriptKey/>
</ClientUpdate>
```
