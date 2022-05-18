# Html rendering of a GitHub MD file
This sample shows how to retrieve a MD file from GitHub, 
render it locally to HTML using Markdig library and 
show it as a web page. Images are retrieved on-the-fly from GitHub, 
through the web-application that works as proxy. 
&lt;img&gt; src is updateed using HtmlAgilityPack.    
The project is an Asp.Net (.NET Framework) web application. 
The logic can be easily adapted to Asp.Net Core (...todo).

```
[browser]  ---->  [web-app] ----> [github]
                      |
                      |
                   [cache]
```

 - simple.aspx : gets the content from GitHub and renders it to html. MD and images are retrieved from GitHub on every request.
 - full.aspx : full solution with local caching of html and images and use of PrismJS for syntax highlighting of code blocks.

Links:
 - Markdig  https://github.com/lunet-io/markdig
 - Prism  https://prismjs.com/
 - HtmlAgilityPack https://html-agility-pack.net/




<br/>
<br/>

Output example [sample1.md](mdsample/sample1.md):

![Output1](imgs/output1.png)

