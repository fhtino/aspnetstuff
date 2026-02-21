<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SoapLikeServer._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: Arial;">
    <form id="form1" runat="server">
        <div>
            <center>
                <h2>Hello wolrd</h2>
                <%=DateTime.UtcNow.ToString("O")%>
            </center>
        </div>
    </form>
</body>
</html>
