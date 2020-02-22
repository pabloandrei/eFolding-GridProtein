<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SimulationAddEdit.aspx.cs" Inherits="Pages_SimulationAdd"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="container">
        <div>
            <h2>Simulations
            </h2>
            <fieldset>
                <legend>
                    <asp:Label ID="lblLegend" runat="server" Text="Legend"></asp:Label>
                </legend>
            </fieldset>
            <!--panel-->

            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="alert alert-danger" role="alert">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Errors occurred:"></asp:ValidationSummary>
                    </div>

                    <asp:Button ID="bttFinish" runat="server" class="btn btn-success pull-right"
                        Text="Finish" OnClick="bttProcess_Click" />
                </div>
            </div>

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator1" class="input-group-addon" runat="server" ErrorMessage="Field 'Label of identification' is requeried!" ControlToValidate="txtLabel" ForeColor="Red" />


            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator2" class="input-group-addon" runat="server" ErrorMessage="Field 'Initial seed' is requeried!" ControlToValidate="txtIsem" ForeColor="Red" />
            <%--<asp:RangeValidator ID="RangeValidator6" Display="None" runat="server" ControlToValidate="txtIsem" ErrorMessage="Field 'Initial seed' requeried value between 0 and 99999999" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>--%>


            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator1" class="input-group-addon" runat="server" ControlToValidate="txtIsem" ErrorMessage="Field 'Isem' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator3" class="input-group-addon" runat="server" ErrorMessage="Field 'MCSteps' is requeried!" ControlToValidate="txtMaxInterations" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator2" class="input-group-addon" runat="server" ControlToValidate="txtMaxInterations" ErrorMessage="Field 'Number of interations' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator4" class="input-group-addon" runat="server" ErrorMessage="Field 'Temperature' is requeried!" ControlToValidate="txtTemperature" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator3" class="input-group-addon" runat="server" ControlToValidate="txtTemperature" ErrorMessage="Field 'Temperature' only number!" ForeColor="Red" ValidationExpression="([0-9,\.])+(;([0-9,\.])+)*"></asp:RegularExpressionValidator>

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator5" class="input-group-addon" runat="server" ErrorMessage="Field 'Change size' is requeried!" ControlToValidate="txtTotalSitio" ForeColor="Red" />


            
            <%--<asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator6" class="input-group-addon" runat="server" ErrorMessage="Field 'Value of beta' is requeried!" ControlToValidate="txtBeta" ForeColor="Red" />
        <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator4" class="input-group-addon" runat="server" ControlToValidate="txtBeta" ErrorMessage="Field 'Value of beta'  only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>--%>

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator7" class="input-group-addon" runat="server" ErrorMessage="Field 'Note' is requeried!" ControlToValidate="txtNote" ForeColor="Red" />

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator8" class="input-group-addon" runat="server" ErrorMessage="Field 'Number of MCSteps discarted' is requeried!" ControlToValidate="txtValueDiscard" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator5" class="input-group-addon" runat="server" ControlToValidate="txtValueDiscard" ErrorMessage="Field 'Number of MCSteps discarted' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <%--            <asp:RangeValidator ID="RangeValidator1" Display="None" runat="server" ControlToValidate="txtValueDiscard" ErrorMessage="Field 'Number of MCSteps discarted' requeried value between 0 and 1000" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>--%>


            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator9" class="input-group-addon" runat="server" ErrorMessage="Field 'Value of div of result' is requeried!" ControlToValidate="txtValueDivResult" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator6" class="input-group-addon" runat="server" ControlToValidate="txtValueDivResult" ErrorMessage="Field 'Value of div of result' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <asp:RangeValidator ID="RangeValidator2" Display="None" runat="server" ControlToValidate="txtValueDivResult" ErrorMessage="Field 'Value of div of result' requeried value between 0 and 1000" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>


            <%--<asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator10" class="input-group-addon" runat="server" ErrorMessage="Field 'Value of Delta' is requeried!" ControlToValidate="txtValueOfDelta" ForeColor="Red" />--%>
            <%--<asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator7" class="input-group-addon" runat="server" ControlToValidate="txtValueOfDelta" ErrorMessage="Field 'Value of Delta' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>--%>

            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator11" class="input-group-addon" runat="server" ErrorMessage="Field 'Max motion peer isem' requeried!" ControlToValidate="txtMaxMotionPeerIsem" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator8" class="input-group-addon" runat="server" ControlToValidate="txtMaxMotionPeerIsem" ErrorMessage="Field 'Max motion peer isem' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <asp:RangeValidator ID="RangeValidator3" Display="None" runat="server" ControlToValidate="txtMaxMotionPeerIsem" ErrorMessage="Field 'Max motion peer isem' requeried value between 0 and 1000" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>


            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator12" class="input-group-addon" runat="server" ErrorMessage="Field 'Rec path every' requeried!" ControlToValidate="txtRecPathEvery" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator9" class="input-group-addon" runat="server" ControlToValidate="txtRecPathEvery" ErrorMessage="Field 'Rec path every' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <asp:RangeValidator ID="RangeValidator4" Display="None" runat="server" ControlToValidate="txtRecPathEvery" ErrorMessage="Field 'Rec path every' requeried value between 0 and 1000" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>



            <asp:RequiredFieldValidator Display="None" ID="RequiredFieldValidator13" class="input-group-addon" runat="server" ErrorMessage="Field 'Split file every' requeried!" ControlToValidate="txtSplitFileEvery" ForeColor="Red" />
            <asp:RegularExpressionValidator Display="None" ID="RegularExpressionValidator10" class="input-group-addon" runat="server" ControlToValidate="txtSplitFileEvery" ErrorMessage="Field 'Split file every' only number!" ForeColor="Red" ValidationExpression="^\d+"></asp:RegularExpressionValidator>
            <asp:RangeValidator ID="RangeValidator5" Display="None" runat="server" ControlToValidate="txtSplitFileEvery" ErrorMessage="Field 'Split file every' requeried value between 0 and 1000" MaximumValue="1000" MinimumValue="1" ForeColor="Red"></asp:RangeValidator>


            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-8">

                            <div class="panel panel-default">
                                <div class="panel-heading">Informations:</div>
                                <div class="panel-body">


                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon1">Label of identification:</span>
                                        <asp:TextBox ID="txtLabel" runat="server" class="form-control" placeholder="Label" MaxLength="255" ToolTip="max 255 characters" ControlToValidate="txtLabel" AutoPostBack="False"></asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Label of identification is...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon17">Select the Model:</span>
                                        <asp:DropDownList ID="ddlModel" runat="server" class="form-control"
                                            OnSelectedIndexChanged="ddlModel_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Enabled="true" Selected="False" Text="Randomic" Value="0" />
                                            <asp:ListItem Enabled="true" Selected="False" Text="Negative" Value="1" />
                                            <asp:ListItem Enabled="true" Selected="False" Text="Positive" Value="2" />
                                            <asp:ListItem Enabled="true" Selected="True" Text="HP Model" Value="3" />
                                        </asp:DropDownList>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Model is...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon18">Model to HP:</span>
                                        <asp:TextBox ID="txtModelHP" runat="server" class="form-control" placeholder="Model to HP:" MaxLength="500" ToolTip="max 500 characters" ControlToValidate="txtLabel" AutoPostBack="True"></asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Label of identification is...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon5">Change size:</span>
                                        <asp:TextBox ID="txtTotalSitio" runat="server" class="form-control" placeholder="Change size" MaxLength="10"
                                            Text="27" AutoPostBack="True"></asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Change size is...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon3">MCSteps:</span>
                                        <asp:TextBox ID="txtMaxInterations" runat="server" class="form-control" placeholder="MCSteps" MaxLength="10"
                                            OnTextChanged="txtMaxInterations_TextChanged" AutoPostBack="True" Text="10000"></asp:TextBox>
                                        <asp:Label class="input-group-addon" ID="txtTime" runat="server" Text="0"></asp:Label>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon2">Initial seed:</span>
                                        <asp:TextBox ID="txtIsem" runat="server" class="form-control" placeholder="Initial seed" MaxLength="8"></asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Initial seed is...">?</a>
                                    </div>
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon4">Temperature:</span>
                                        <asp:TextBox ID="txtTemperature" runat="server" class="form-control" placeholder="Temperature" MaxLength="500"
                                            Text="" AutoPostBack="False"></asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Temperature is...">?</a>
                                    </div>
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon7">Note:</span>
                                        <asp:TextBox ID="txtNote" runat="server" class="form-control" placeholder="Note"
                                            MaxLength="200" Rows="2"
                                            TextMode="MultiLine" Width="444px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">Configuration of e-mail:</div>
                                <div class="panel-body">

                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="chkEmail" runat="server" Checked="false" class="input-group" />
                                        </span>
                                        <span class="form-control" id="basic-addon8">Received result?</span>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Received result is...">?</a>
                                    </div>
                                </div>
                            </div>

                            <div class="panel panel-default">
                                <div class="panel-heading">Configuration create result:</div>
                                <div class="panel-body">
                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon9">Number of MCSteps discarted:</span>
                                        <asp:TextBox ID="txtValueDiscard" runat="server" class="form-control" placeholder="Number of MCSteps discarted" MaxLength="10">20</asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Number of MCSteps discarte is...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon10">Value of div of result:</span>
                                        <asp:TextBox ID="txtValueDivResult" runat="server" class="form-control" placeholder="Value of div of result" MaxLength="10">100</asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Value of div of result...">?</a>
                                    </div>
                                </div>
                            </div>


                            <div class="panel panel-default">
                                <div class="panel-heading">Configuration process:</div>
                                <div class="panel-body">

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon12">Max motion peer isem (loop):</span>
                                        <asp:TextBox ID="txtMaxMotionPeerIsem" runat="server" class="form-control" placeholder="Max motion peer isem (loop)" MaxLength="10">10</asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Max motion peer isem (loop)...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon13">Rec path every:</span>
                                        <asp:TextBox ID="txtRecPathEvery" runat="server" class="form-control" placeholder="Rec path every" MaxLength="10">100</asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Rec path every...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon" id="basic-addon14">Split file every:</span>
                                        <asp:TextBox ID="txtSplitFileEvery" runat="server" class="form-control" placeholder="Split file every" MaxLength="10">1000</asp:TextBox>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Split file every...">?</a>
                                    </div>
                                </div>
                            </div>


                            <div class="panel panel-default">
                                <div class="panel-heading">Configuration of Output:</div>
                                <div class="panel-body">

                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="chkEvolution" runat="server" Checked="true" class="input-group" />
                                        </span>
                                        <span class="form-control" id="basic-addon15">Evolution of energy and Spin Ray</span>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Evolution of energy and Spin Ray...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="chkDistribution" runat="server" Checked="true" class="input-group" />
                                        </span>
                                        <span class="form-control" id="basic-addon16">Distribution of energy and Spin Ray</span>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Distribution of energy and Spin Ray...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="chkConfiguration" runat="server" Checked="true" class="input-group" />
                                        </span>
                                        <span class="form-control" id="basic-addon17">Configuration of simulation</span>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Configuration of simulation...">?</a>
                                    </div>

                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <asp:CheckBox ID="chkDebug" runat="server" Checked="false" class="input-group" />
                                        </span>
                                        <span class="form-control" id="basic-addon18">Debug of simulation</span>
                                        <a href="#" class="input-group-addon" data-toggle="tooltip" title="Debug of simulation...">?</a>
                                    </div>

                                </div>
                            </div>

                        </div>
                        <div class="col-md-4">

                            <div class="panel panel-default">
                                <div class="panel-heading">Hints:</div>
                                <div class="panel-body">
                                    <center><b>Aditcional informations:</b></center>
                                    <%-- <ul>
                                        <li><b>Label of identification</b> - Rotulo de identificação da simulação.<br>
                                        </li>
                                        <li><b>Initial seed</b> - Número inicial da semente. Deve ser um número inteiro impar.<br>
                                        </li>
                                        <li><b>Number of interations</b> - Número de interações para geração Monte Carlo Step.<br>
                                        </li>
                                        <li><b>Temperature</b> - Temperatura utilizada na simulação. (1=27 Celsius)<br>
                                        </li>
                                        <li><b>Total of sitio</b> - Número de sitios da cadeia.<br>
                                        </li>
                                        <li><b>Type</b> - Define o tipo da cadeia a ser utilizada na simulação. O tipo REAL não ocorre sobreposição, entretanto existe vizinhos de contatos topologico de primeiros vizinhos.<br>
                                        </li>
                                        <li><b>Note</b> - anotações sobre a simulação.<br>
                                        </li>
                                    </ul>
                                    <br>
                                    <center><b>Configuration of e-mail</b></center>
                                    <ul>
                                        <li><b>Received result?</b> - Deseja receber e-mail de notificação da simulação?<br>
                                        </li>
                                    </ul>
                                    <br>

                                    <center><b>Configuration create result:</b></center>
                                    <ul>
                                        <li><b>Value of discard</b> - durante o processo de consolidação da simulação, o valor de discarte é utilizado para saltar (pular) os primeiros resultados da simulação. Quando valor igual a ZERO, não ocorre nenhum descarte; 10 por exemplo seria o descarte dos 10 primeiros movimentos aceitos na simulalão.<br>
                                        </li>
                                        <li><b>Value of div of result</b> - durante o processo de extratificação dos resultados, é necessário gerar o valor de DELTA, onde DELTA é número de passos Monte Carlo Step dividivo pelo valor do "Value of div of result". Exemplo: delta = MCSteps / valueDivResult.<br>
                                            Este valor é utilizado para sumarizar os valores contidos na sheet "Trajectory" do arquivo de resultados gerado no formato Excel. Delta portanto é indicador de do periodo de acumulado dos valor do raio de giração.<br>
                                        </li>
                                    </ul>
                                    <br>
                                    <center><b>Configuration process</b></center>
                                    <ul>
                                        <li><b>Rec path every</b> - Este item de configuração indica qual o modo de escrita dos resultados em simulação aceitos durante o tempo de Monte Carlo. Valor 0 e 1, indicam que a cada movimento aceito, o resultado será salvo em seu repositório de informações. Valor a cima de um (1), são considerado saltos entre as condfigurações aceitas. Exemplo: 10, irá salvar informação a cada 10 movimentos aceitos.<br>
                                        </li>
                                        <li><b>Split file every</b> - Esta opção defini o tamanho de registro por arquivo. O split de arquivo é necessário em cenários os quais os passo de Monte Carlo são muito longos. Em algumas máquinas poderá ocorrer problemas de I/O. Indicado é utilizar o valor 1000.<br>
                                        </li>
                                    </ul>--%>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-body">
                <asp:Button ID="Button2" runat="server" class="btn btn-success pull-right"
                    Text="Finish" OnClick="bttProcess_Click" />
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">Log:</div>
            <div class="panel-body">

                <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-bordered table-condensed"
                    EmptyDataText="No data found!"
                    AllowSorting="True"
                    ShowFooter="True">
                    <Columns>
                        <asp:BoundField DataField="date" HeaderText="Date" />
                        <asp:BoundField DataField="StatusDescription" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>


    </div>

</asp:Content>

