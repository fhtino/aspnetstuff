<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="full.aspx.cs" Inherits="RenderGitHubMD.full" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" />
    <link href="prism.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <script src="prism.js"></script>
        <div id="ContentDiv" runat="server" enableviewstate="false" clientidmode="Static">
        </div>
        <div style="padding: 10px; margin-top: 100px; margin-bottom: 50px; background-color: #ebefff;">
            Original document: 
            <asp:HyperLink ID="LinkToSource" Target="_blank" runat="server">link...</asp:HyperLink>
        </div>
    </form>
</body>
</html>
