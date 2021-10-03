<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="uploadPage.aspx.cs" Inherits="Web1.uploadPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="UploadFile.js" type="text/javascript"></script>
</head>
<body style="font-family: Arial;">
    <form id="form1" runat="server">
        <div>

            <div style="background-color: lightsteelblue; width: 500px; padding: 5px;">
                Select the file:
                <input type="file" id="FileInput" /><br />
                <div id="divProgress" style="visibility: collapse;">
                    <div id="divProgressBar" style="width: 400px; border: 1px solid black;">
                        <div id="divProgressBarValue" style="background-color: green; width: 0px; height: 15px;"></div>
                    </div>
                    <div id="dirProgressText" style="background-color: lightyellow; font-size: small;"></div>
                </div>
                <asp:HiddenField ID="UploadGuid" ClientIDMode="Static" runat="server" />
            </div>

            <script type="text/javascript">
                function PostFileUploadOK() {
                    document.forms[0].submit();
                }
                function PostFileUploadERR() {
                    alert("postfileupload funciton ERROR");
                }
            </script>

            <input id="buttonSubmit" type="button" value="Submit" onclick="SendFile('/uploadHandler.ashx', PostFileUploadOK, PostFileUploadERR); return false;" /><br />
            <br />
        </div>
    </form>
</body>
</html>
