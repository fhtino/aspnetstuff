<%@ Page Title="" Language="C#" MasterPageFile="~/main.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="OwinAuthSimple.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Authentication providers</h2>
    <br />
    <br />
    <asp:ListView ID="ListView1" runat="server" ItemType="System.String">
        <ItemTemplate>
            <button type="submit" class="btn btn-default" name="provider" value="<%#: Item %>" title="<%#: Item %>">
                <%#: Item %><br />
            </button>
            &nbsp;&nbsp;               
        </ItemTemplate>
    </asp:ListView>
    <br />
    <br />
</asp:Content>
