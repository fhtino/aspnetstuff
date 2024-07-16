<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="AsyncWeb._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family: 'Courier New'">
            <%=DateTime.UtcNow.ToString("O")%>
            <br />
            <br />
            <br />
            <strong>ThreadPool:</strong><br />
            <asp:Label ID="LblMsg" runat="server" Text="Label"></asp:Label>
        </div>
    </form>
</body>
</html>
