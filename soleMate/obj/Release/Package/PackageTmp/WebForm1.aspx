<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="soleMate.WebForm1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <link href="https://fonts.googleapis.com/css?family=Noto+Sans+TC" rel="stylesheet">
    <title>Amputee Coalition: Sole Mate</title>
    <meta charset="utf-8" />
    <style>
        * {
            font-family: 'Noto Sans TC', sans-serif;
        }

        header {
            background-color: #555555;
            color: aliceblue;
            height: 300px;
            padding-top: 5px;
            width: auto;
            border-radius: 20px;
        }

        #wrapper {
            margin: 0 auto;
            width: 90%;
            text-align: center;
        }

        #left {
            width: 33.33%;
            float: left;
            background-color: #eeefea;
            height: 550px;
            font-size: medium;
            border-radius: 20px;
        }

        #middle {
            width: 33.33%;
            float: left;
            background-color: #eeefea;
            height: 550px;
            font-size: medium;
            border-radius: 20px;
        }

        #right {
            width: 33.33%;
            float: right;
            background-color: #fffaf0;
            height: 550px;
            font-size: medium;
            border-radius: 20px;
        }

        .footer {
            padding: 2rem;
            height: auto;
            background-color: #555555;
            font-size: medium;
            text-align: center;
            border-radius: 20px;
        }

        .BannerContainer img {
            width: 100%;
            height: 200px;
        }

        .fields {
            height: 39px;
        }

        labels.CssClass {
            text-align: left;
            height: 39px;
        }

        .marginFix {
            text-align: center;
        }

        .cssfix2 {
            margin-left: 0px;
        }

        .err {
            font-size: 8px
        }

        .footerTxt {
            color: aliceblue
        }

        .errorMessages {

            color:red;
        }
        
    </style>

    <%--favicon for the page--%>
    <link rel="shortcut icon" type="image/x-icon" href="~/favicon.png" />

</head>
<body>
    <div id="wrapper" class="container">
        <header>
            <div class="BannerContainer">                
                <img id="Banner2" src="https://i.imgur.com/qn3uGBF.jpg" />
            </div>
            <h1>Shoe Exchange</h1>
        </header>
        <form runat="server">
            <main class="col-lg">
                <div id="left" class="container">

                    <h4 class="headings">Please Select Your Country and State</h4>
                    <br />
                    <asp:Label runat="server" class="labels">Country: </asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="CountryDD" OnSelectedIndexChanged="CountryDD_SelectedIndexChanged" AutoPostBack="True">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label runat="server" class="labels">State: </asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="stateDD" AutoPostBack="True">
                        <asp:ListItem>--Select Your State--</asp:ListItem>
                    </asp:DropDownList>                    
             
                </div>
                <div id="middle" class="marginFix">

                    <h4 class="headings">Please Enter Your Information</h4>
                    <br />
                    <asp:Label runat="server" class="labels">First Name: </asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="firstName" Width="250px"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator runat="server" ID="reqName" ControlToValidate="firstName" CssClass="errorMessages" ErrorMessage="Please Enter Your First Name!" />
                    <br />
                    <asp:Label runat="server" class="labels">Last Name: </asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="lastName" Width="250px"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator runat="server" ID="reqlastName" ControlToValidate="lastName" CssClass="errorMessages" ErrorMessage="Please Enter Your Last Name!" />
                    <br />
                    <asp:Label runat="server" class="labels">Email: </asp:Label>
                    <br />
                    <asp:TextBox runat="server" ID="email" Width="250px"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator runat="server" ID="reEmail" ControlToValidate="email" CssClass="errorMessages" ErrorMessage="Please Enter Your Email!" />
                    <br />
                    <asp:Label runat="server" class="labels">Shoe Size: </asp:Label>
                    <br />
                    <asp:DropDownList runat="server" ID="sizeID" CssClass="cssfix2" Width="150px"></asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label runat="server" class="labels">Leg: </asp:Label>
                    <br />
                    <asp:CheckBoxList runat="server" ID="legCheck" align="center">      
                        <asp:ListItem>Left</asp:ListItem>
                        <asp:ListItem>Right</asp:ListItem>
                    </asp:CheckBoxList>
                    <br />
                    <br />
                    <asp:Button ID="submitButton" Text="Submit" runat="server" OnClick="Click_Submit" />

                </div>
                <div id="right" class="col-lg">

                    <h4 class="headings">Results</h4>
                    <p>&nbsp;</p>
                    <p><i>You can find the name and the email of your match below after you submit the information</i> </p>
                    <asp:Label class="errorMessages" runat="server" ID="errorText"></asp:Label>
                    <asp:Label class="errorMessages" runat="server" ID="showMsg"></asp:Label>
                    <asp:Panel runat="server" ID="aspPanel" ScrollBars="Both" Height="300" Width="100%">
                        <div id="divinPanel" runat="server">                             
                             
                        </div>
                    </asp:Panel>

                </div>

            </main>

            <div class="footer">
                <footer>
                    <p>&nbsp;</p>
                    <h4 class="footerTxt">CIS 4160: Amuptee Coalition Project</h4>
                    <h4 class="footerTxt">Copyright © Lewis, Kevin, Chamal </h4>
                </footer>
            </div>

        </form>
    </div>
</body>
</html>
