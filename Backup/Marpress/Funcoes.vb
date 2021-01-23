Imports Marpress.Interfaces
Imports Marpress.FAC
Imports Marpress.FichaCompensacao
Imports System.IO
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser
Imports iTextSharp.text.BaseColor

Namespace Funcoes

    Public Module Funcoes

        Public Function CapturarNumero(ByVal endereco As String) As String
            Dim achouInicio As Boolean = False
            Dim achouFim As Boolean = False
            Dim inicio As Integer = 0
            Dim fim As Integer = 0
            Dim i As Integer = 0
            For i = 0 To endereco.Trim.Length - 1
                If IsNumeric(endereco.Substring(i, 1)) Then
                    If Not achouInicio Then
                        achouInicio = True
                        inicio = i
                    End If
                Else
                    If achouInicio Then
                        achouFim = True
                        fim = i
                    End If
                    If achouInicio And achouFim Then
                        Exit For
                    End If
                End If
            Next
            If Not achouFim Then
                fim = i
            End If
            Return endereco.Substring(inicio, fim - inicio)
        End Function

        ''' <summary>
        ''' Função para formatar uma string em formato data dd/MM/yyyy
        ''' </summary>
        ''' <param name="data">String com a data</param>
        ''' <param name="formato">formatação da data informada no parâmetro data</param>
        ''' <returns>Retorna a String em um formato data dd/MM/yyyy</returns>
        ''' <remarks></remarks>
        Public Function FormatarData(ByVal data As String, ByVal formato As String) As String
            Return FormatDateTime(Date.ParseExact(data, formato, Nothing), DateFormat.ShortDate)
        End Function

        Public Function EditaDois(ByVal valor As String) As String
            Return Double.Parse(valor.Trim.Insert(valor.Trim.Length - 2, ",")).ToString("C").Replace("R$", "")
        End Function

        Public Function EditaDois(ByVal valor As Double) As String
            Return valor.ToString("C").Replace("R$", "")
        End Function

        Public diretorioImagem As String = "Y:\xgfc\imglib\"
        Public diretorioPDF As String = "C:\Amarildo\Imagem_Pdf\"

        Public Function SomenteNumeros(ByVal cep As String) As String
            Dim numeros As String = ""
            For i As Integer = 0 To cep.Length - 1
                If IsNumeric(cep.Substring(i, 1)) Then
                    numeros += cep.Substring(i, 1)
                End If
            Next
            Return numeros
        End Function

        Public Function DefinirCategoriaCEP(ByRef cep As String, ByVal tipo As FAC.Tipo) As String
            Dim categoria As String = ""
            Select Case tipo
                Case FAC.Tipo.SIMPLES
                    Select Case cep.Substring(0, 1)
                        Case 0
                            categoria = "82015"
                        Case 1
                            categoria = "82023"
                        Case Else
                            categoria = "82031"
                    End Select
                Case FAC.Tipo.REGISTRADO
                    Select Case cep.Substring(0, 1)
                        Case 0
                            categoria = "82104"
                        Case 1
                            categoria = "82112"
                        Case Else
                            categoria = "82120"
                    End Select
                Case FAC.Tipo.REGISTRADO_COM_AR
                    Select Case cep.Substring(0, 1)
                        Case 0
                            categoria = "82139"
                        Case 1
                            categoria = "82147"
                        Case Else
                            categoria = "82155"
                    End Select
            End Select
            Return categoria
        End Function
        Public Function validaDataPostagem(ByVal data As Date) As Date
            data = VerificaFeriado(data)
            data = VerificaFimDeSemana(data)
            data = VerificaFeriado(data)
            data = VerificaFimDeSemana(data)
            Return data
        End Function

        Public Function VerificaFimDeSemana(ByVal data As Date) As Date
            If data.DayOfWeek = DayOfWeek.Saturday Then
                data = DateAdd(DateInterval.Day, 2, data)
            ElseIf data.DayOfWeek = DayOfWeek.Sunday Then
                data = DateAdd(DateInterval.Day, 1, data)
            End If
            Return data
        End Function

        Public Function VerificaFeriado(ByVal data As Date) As Date
            Dim diames As String = String.Format("{0:ddMM}", data)
            Select Case diames
                Case "0101", "25/01", "2104", "0105", "0907", "0709", "1210", "0211", "1511", "2512"
                    data = DateAdd(DateInterval.Day, 1, data)
            End Select
            If SextaFeiraSanta(data.Year) = data Then
                data = DateAdd(DateInterval.Day, 1, data)
            End If
            If CorposChristi(data.Year) = data Then
                data = DateAdd(DateInterval.Day, 1, data)
            End If
            If Carnaval(data.Year) = data Then
                data = DateAdd(DateInterval.Day, 1, data)
            End If
            Return data
        End Function

        Public Function SextaFeiraSanta(ByVal Ano As Integer) As Date
            Dim D As Date = Pascoa(Ano)
            Return DateSerial(Ano, D.Month, D.Day - 2)
        End Function

        Public Function Pascoa(ByVal Ano As Integer) As Date

            Dim A As Integer = Ano Mod 19
            Dim B As Integer = Int(Ano / 100)
            Dim C As Integer = Ano Mod 100
            Dim D As Integer = Int(B / 4)
            Dim E As Integer = B Mod 4
            Dim F As Integer = Int((B + 8) / 25)
            Dim G As Integer = Int((B - F + 1) / 3)
            Dim H As Integer = (19 * A + B - D - G + 15) Mod 30
            Dim I As Integer = Int(C / 4)
            Dim J As Integer = C Mod 4
            Dim L As Integer = (32 + 2 * E + 2 * I - H - J) Mod 7
            Dim M As Integer = Int((A + 11 + H + 22 * L) / 451)

            Dim Mes As Integer = Int((H + L - 7 * M + 114) / 31)
            Dim Dia As Integer = 1 + ((H + L - 7 * M + 114) Mod 31)

            Return DateSerial(Ano, Mes, Dia)
        End Function

        Public Function CorposChristi(ByVal Ano As Integer) As Date
            Dim D As Date = Pascoa(Ano)
            Return DateSerial(Ano, D.Month, D.Day + 60)
        End Function

        Public Function Carnaval(ByVal Ano As Integer) As Date
            Dim D As Date = Pascoa(Ano)
            Return DateSerial(Ano, D.Month, D.Day - 47)
        End Function

        Public Sub DefinirTriagemCEP(ByRef objeto As ICIF)
            Dim cep As Integer = Integer.Parse(SomenteNumeros(objeto.Destinatario.Endereco.CEP))
            If cep >= 100100 And cep < 2000000 Then
                objeto.CIF.CodigoCEP = "1"
                objeto.CIF.CodigoTriagem = "1"
            End If
            If cep >= 2001000 And cep < 3000000 Then
                objeto.CIF.CodigoCEP = "1"
                objeto.CIF.CodigoTriagem = "2"
            End If
            If cep >= 3001000 And cep < 7000000 Then
                objeto.CIF.CodigoCEP = "1"
                objeto.CIF.CodigoTriagem = "1"
            End If
            If cep >= 7001000 And cep < 9000000 Then
                objeto.CIF.CodigoCEP = "1"
                objeto.CIF.CodigoTriagem = "2"
            End If
            If cep >= 9001000 And cep < 10000000 Then
                objeto.CIF.CodigoCEP = "1"
                objeto.CIF.CodigoTriagem = "1"
            End If
            If cep >= 11000000 And cep < 14000000 Then
                objeto.CIF.CategoriaCEP = "82023"
                objeto.CIF.CodigoCEP = "2"
                objeto.CIF.CodigoTriagem = "3"
            End If
            If cep >= 14000000 And cep < 20000000 Then
                objeto.CIF.CodigoCEP = "2"
                objeto.CIF.CodigoTriagem = "4"
            End If
            If cep >= 20000000 And cep < 60000000 Then
                objeto.CIF.CodigoCEP = "3"
                objeto.CIF.CodigoTriagem = "5"
            End If
            If cep >= 60000000 And cep < 100000000 Then
                objeto.CIF.CodigoCEP = "3"
                objeto.CIF.CodigoTriagem = "6"
            End If
        End Sub

        ''' <summary>Cálculo Módulo 11 - Peso Inicial e Peso Final</summary>
        ''' <param name="codigo">Sequência numérica para cálculo</param>
        ''' <param name="pesoInicial">Peso Inicial</param>
        ''' <param name="pesoFinal">Peso Final</param>
        ''' <returns>Retorna um valor Inteiro</returns>
        Public Function Mod11(ByVal codigo As String, ByVal pesoInicial As Integer, ByVal pesoFinal As Integer) As Integer
            Dim i, soma, peso As Integer
            peso = pesoInicial
            soma = 0

            i = codigo.Length
            If pesoInicial > pesoFinal Then
                While i > 0
                    soma = soma + codigo.Substring(i - 1, 1) * pesoInicial
                    pesoInicial = pesoInicial - 1
                    If pesoInicial < pesoFinal Then pesoInicial = peso
                    i = i - 1
                End While
            End If
            If pesoInicial < pesoFinal Then
                While i > 0
                    soma = soma + codigo.Substring(i - 1, 1) * pesoInicial
                    pesoInicial = pesoInicial + 1
                    If pesoInicial > pesoFinal Then pesoInicial = peso
                    i = i - 1
                End While
            End If
            Return 11 - (soma Mod 11)
        End Function


        ''' <summary>Função para Codificar o código de barras</summary>
        ''' <param name="strNumero">Sequência numérica a ser codificada</param>
        ''' <returns>Retorna um String</returns>
        Public Function Cod_Bar(ByVal strNumero As String) As String
            Dim i, l, tamanho, X, Y, numx, numy As Integer
            Dim codbarra, compxerox, compxerox1, compxerox2, binxer As String
            Static codxerox() As String = {"00110", "10001", "01001", "11000", "00101", "10100", "01100", "00011", "10010", "01010"}

            tamanho = strNumero.Length
            If tamanho Mod 2 <> 0 Then
                MsgBox("Código Inválido, Quantidade de números precisam ser múltiplos de 2", MsgBoxStyle.Critical, "Função 2 de 5 intercalado")
                Cod_Bar = "Codigo de Barras Incorreto"
                Exit Function
            ElseIf tamanho = 0 Then
                MsgBox("Código de Barras em Branco", MsgBoxStyle.Critical, "Função 2 de 5 intercalado")
                Cod_Bar = "Codigo de Barras em Branco"
                Exit Function
            End If
            codbarra = "<"
            l = 0

            For i = 1 To tamanho / 2
                l = l + 1
                numx = strNumero.Substring(l - 1, 1)
                l = l + 1
                numy = strNumero.Substring(l - 1, 1)
                compxerox1 = codxerox(numx)
                compxerox2 = codxerox(numy)
                compxerox = compxerox1.Substring(0, 1)
                compxerox = compxerox & compxerox2.Substring(0, 1)
                compxerox = compxerox & compxerox1.Substring(1, 1)
                compxerox = compxerox & compxerox2.Substring(1, 1)
                compxerox = compxerox & compxerox1.Substring(2, 1)
                compxerox = compxerox & compxerox2.Substring(2, 1)
                compxerox = compxerox & compxerox1.Substring(3, 1)
                compxerox = compxerox & compxerox2.Substring(3, 1)
                compxerox = compxerox & compxerox1.Substring(4, 1)
                compxerox = compxerox & compxerox2.Substring(4, 1)
                Y = 1
                For X = 1 To 5
                    binxer = compxerox.Substring(Y - 1, 2)
                    Select Case binxer
                        Case "00"
                            codbarra = codbarra & "n"
                        Case "01"
                            codbarra = codbarra & "N"
                        Case "10"
                            codbarra = codbarra & "w"
                        Case "11"
                            codbarra = codbarra & "W"
                    End Select
                    Y = Y + 2
                Next
            Next
            codbarra = codbarra & ">"
            Return codbarra
        End Function

        Public Function NNBanespa(ByVal cnumero As String)
            Dim cvetor As String
            Dim nsoma, nconta As Integer
            cvetor = "7319731973"
            nsoma = 0
            For nconta = 1 To 10
                nsoma = nsoma + (cvetor.Substring(nconta - 1, 1) * cnumero.Substring(nconta - 1, 1))
            Next
            Return IIf(nsoma Mod 10 > 0, 10 - (nsoma Mod 10), 0)
        End Function

        Function NNNossaCaixa(ByVal cnumero As String)
            Dim cvetor As String
            Dim nsoma, nconta As Integer
            cvetor = "31973197319731319731973"
            nsoma = 0
            For nconta = 1 To 23
                nsoma = nsoma + (cvetor.Substring(nconta - 1, 1) * cnumero.Substring(nconta - 1, 1))
            Next
            Return IIf(nsoma Mod 10 > 0, 10 - (nsoma Mod 10), 0)
        End Function

        Function NNBankBoston(ByVal cnumero As String)
            Dim cvetor As String
            Dim nsoma, nconta As Integer
            cvetor = "98765432"
            nsoma = 0
            For nconta = 1 To 8
                nsoma = nsoma + (cvetor.Substring(nconta - 1, 1) * cnumero.Substring(nconta - 1, 1))
            Next
            nsoma = nsoma * 10
            Return IIf(nsoma Mod 11 = 10, 0, nsoma Mod 11)
        End Function

        Public Function RetiraAcentos(ByVal texto As String) As String
            Dim acentos = New String() {"ç", "Ç", "á", "é", "í", "ó", "ú", "ý", "Á", "É", "Í", "Ó", "Ú", "Ý", "à", "è", "ì", "ò", "ù", "À", "È", "Ì", "Ò", "Ù", "ã", "õ", "ñ", "ä", "ë", "ï", "ö", "ü", "ÿ", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "â", "ê", "î", "ô", "û", "Â", "Ê", "Î", "Ô", "Û", "'", "/"}
            Dim semAcento = New String() {"c", "C", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "Y", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", "a", "o", "n", "a", "e", "i", "o", "u", "y", "A", "E", "I", "O", "U", "A", "O", "N", "a", "e", "i", "o", "u", "A", "E", "I", "O", "U", " ", " "}
            For i As Integer = 0 To acentos.Length - 1
                texto = texto.Replace(acentos(i), semAcento(i))
            Next
            Return texto
        End Function

        ''' <summary>
        ''' Funcão usada para a construção do código de barras - PostNet
        ''' </summary>
        ''' <param name="cep">CEP - somente números</param>
        ''' <returns>CEP + Dígito Verificador</returns>
        ''' <remarks></remarks>
        Public Function PostNet(ByVal cep As String) As String
            Dim dvCEP As String = ""
            If cep.Length <> 8 Then
                Throw New Exception("CEP Inválido!!!!")
            End If
            Dim soma As Integer = 0
            For i As Integer = 0 To 7
                soma = soma + Integer.Parse(cep.Substring(i, 1))
            Next
            dvCEP = IIf(Right(soma.ToString.Trim, 1) = "0", 0, 10 - Right(soma.ToString.Trim, 1)).ToString
            Return "*" & cep & dvCEP & "*"
        End Function

        ''' <summary>
        ''' Funcão usada para verificar se o CEP está dentro da Faixa de CEP do Estado informado
        ''' </summary>
        ''' <param name="cep">CEP</param>
        ''' <param name="uf">Estado</param>
        ''' <returns>Verdadeiro ou Falso</returns>
        ''' <remarks></remarks>
        Public Function ValidaCEP(ByVal cep As Integer, ByVal uf As String) As Boolean
            Dim retorno = False
            Dim faixas As New List(Of FaixaCEP)
            faixas.Add(New FaixaCEP("AC", New Integer() {69900000}, New Integer() {69999999}))
            faixas.Add(New FaixaCEP("AL", New Integer() {57000000}, New Integer() {57999999}))
            faixas.Add(New FaixaCEP("AM", New Integer() {69000000, 69400000}, New Integer() {69299999, 69899999}))
            faixas.Add(New FaixaCEP("AP", New Integer() {68900000}, New Integer() {68999999}))
            faixas.Add(New FaixaCEP("BA", New Integer() {40000000}, New Integer() {48999999}))
            faixas.Add(New FaixaCEP("CE", New Integer() {60000000}, New Integer() {63999999}))
            faixas.Add(New FaixaCEP("DF", New Integer() {70000000, 73000000}, New Integer() {72799999, 73699999}))
            faixas.Add(New FaixaCEP("ES", New Integer() {29000000}, New Integer() {29999999}))
            faixas.Add(New FaixaCEP("GO", New Integer() {72800000, 73700000}, New Integer() {72999999, 76799999}))
            faixas.Add(New FaixaCEP("MA", New Integer() {65000000}, New Integer() {65999999}))
            faixas.Add(New FaixaCEP("MG", New Integer() {30000000}, New Integer() {39999999}))
            faixas.Add(New FaixaCEP("MS", New Integer() {79000000}, New Integer() {79999999}))
            faixas.Add(New FaixaCEP("MT", New Integer() {78000000}, New Integer() {78899999}))
            faixas.Add(New FaixaCEP("PA", New Integer() {66000000}, New Integer() {68899999}))
            faixas.Add(New FaixaCEP("PB", New Integer() {58000000}, New Integer() {58999999}))
            faixas.Add(New FaixaCEP("PE", New Integer() {50000000}, New Integer() {56999999}))
            faixas.Add(New FaixaCEP("PI", New Integer() {64000000}, New Integer() {64999999}))
            faixas.Add(New FaixaCEP("PR", New Integer() {80000000}, New Integer() {87999999}))
            faixas.Add(New FaixaCEP("RJ", New Integer() {20000000}, New Integer() {28999999}))
            faixas.Add(New FaixaCEP("RN", New Integer() {59000000}, New Integer() {59999999}))
            faixas.Add(New FaixaCEP("RO", New Integer() {76801000}, New Integer() {76999000}))
            faixas.Add(New FaixaCEP("RR", New Integer() {69300000}, New Integer() {69399999}))
            faixas.Add(New FaixaCEP("RS", New Integer() {90000000}, New Integer() {99999999}))
            faixas.Add(New FaixaCEP("SC", New Integer() {88000000}, New Integer() {89999999}))
            faixas.Add(New FaixaCEP("SE", New Integer() {49000000}, New Integer() {49999999}))
            faixas.Add(New FaixaCEP("SP", New Integer() {1000000}, New Integer() {19999999}))
            faixas.Add(New FaixaCEP("TO", New Integer() {77000000}, New Integer() {77999999}))

            For Each faixa As FaixaCEP In faixas
                For i As Integer = 0 To faixa.CepInicial.Length - 1
                    If uf = faixa.Estado And cep >= faixa.CepInicial(i) And cep <= faixa.CEPFinal(i) Then
                        retorno = True
                    End If
                Next
            Next
            Return retorno
        End Function

        ''' <summary>
        ''' Funcão usada para padronizar os registros dentro do FAC SIMPLES
        ''' </summary>
        ''' <param name="cliente">Cliente que tenha um contrato FAC</param>
        ''' <param name="arquivo">Objeto com os registros</param>
        ''' <param name="producao">Processamento será produção?</param>
        ''' <param name="dataFAC">Data de Postagem FAC</param>
        ''' <param name="label">Label que será exibida os detalhes do processo</param>
        ''' <remarks></remarks>
        Public Sub processaFAC(ByVal cliente As FAC.Contrato, ByVal processamento As String, ByRef arquivo As IArquivo, ByVal producao As Boolean, ByVal dataFAC As Date, ByRef label As Label)
            Try
                dataFAC = validaDataPostagem(dataFAC)
                Dim arqFAC As StreamReader
                Dim arqDBF As String = ""
                Dim pastaFAC As String = ""
                Dim caminho As String = "C:\Amarildo\Fac\"
                Select Case cliente
                    Case FAC.Contrato.ACSP
                        arqDBF = "FACACSP.DBF"
                        pastaFAC = "ACSP"
                    Case FAC.Contrato.BVS
                        arqDBF = "FACBVS.DBF"
                        pastaFAC = "BOAVISTA"
                    Case FAC.Contrato.DMCARD
                        arqDBF = "FACDMFAT.DBF"
                        pastaFAC = "DMCARD"
                    Case FAC.Contrato.HOEPERS
                        arqDBF = "FACHOPER.DBF"
                        pastaFAC = "HOEPERS"
                    Case FAC.Contrato.LOPES
                        arqDBF = "FACLOPES.DBF"
                        pastaFAC = "LOPES"
                    Case FAC.Contrato.OMNI
                        arqDBF = "FAC_OMNI.DBF"
                        pastaFAC = "OMNI"
                    Case FAC.Contrato.OSCAR
                        arqDBF = "FACOSCAR.DBF"
                        pastaFAC = "OSCAR"
                    Case FAC.Contrato.JOCALCADOS
                        arqDBF = "FACOSCAR.DBF"
                        pastaFAC = "OSCAR"
                End Select
                arqFAC = New StreamReader(caminho & arqDBF)

                Dim codigoDR As Integer = 99
                Dim codigoAdm As Integer = 99999999
                Dim numeroCartao As Long = 999999999999
                Dim numeroLote As Integer = 99999
                Dim codigoUnidade As Integer = 99999999
                Dim cepUnidade As Integer = 99999999
                Dim numeroContrato As Long = 9999999999
                Dim nomeMidia As String = ""
                Dim servicoAdicional As String = "0000000000"
                Dim cnae As Long = 999999999

                If producao Then
                    Dim conn As New OleDbConnection
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & caminho & ";Extended Properties=dBASE IV;"
                    conn.Open()
                    Dim cmd As OleDbCommand = conn.CreateCommand
                    cmd.CommandText = "SELECT * FROM " & caminho & arqDBF
                    Dim dt As New DataTable
                    arqFAC.Close()
                    dt.Load(cmd.ExecuteReader)
                    For Each row As DataRow In dt.Rows
                        codigoDR = row("CODIGO_DR")
                        codigoAdm = row("COD_ADMIN")
                        numeroCartao = row("NUM_CARTAO")
                        numeroLote = row("NUM_LOTE")
                        codigoUnidade = row("COD_POSTAG")
                        cepUnidade = row("CEP_ORIGEM")
                        numeroContrato = row("N_CONTRATO")
                        cnae = row("CNAE")
                        If cliente = FAC.Contrato.DMCARD_CARTOES Then
                            numeroCartao = 65918649
                        End If
                        If cliente = FAC.Contrato.JOCALCADOS Then
                            numeroCartao = 69889287
                        End If
                    Next
                    arqFAC = New StreamReader(caminho & arqDBF)
                    numeroLote += 1
                    cmd.CommandText = "UPDATE " & caminho & arqDBF & " SET NUM_LOTE = " & numeroLote.ToString
                    arqFAC.Close()
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                    conn.Close()
                    conn.Dispose()
                    GC.Collect()
                    nomeMidia = codigoAdm.ToString.Trim.PadLeft(8, "0") & "_" & _
                                numeroLote.ToString.Trim.PadLeft(5, "0") & "_" & _
                                "UNICA" & "_" & _
                                codigoDR.ToString.Trim.PadLeft(2, "0") & ".txt"
                End If
                With arquivo
                    For i As Integer = 0 To .Linhas.Count - 1
                        If .Linhas(i).CIF.TipoRegistro <> "CepErrado" And .Linhas(i).CIF.TipoRegistro = processamento Then
                            Dim cep As Integer = Integer.Parse(SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"))
                            .Linhas(i).CIF.CategoriaCEP = DefinirCategoriaCEP(SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"), Tipo.SIMPLES)
                            DefinirTriagemCEP(.Linhas(i))
                            If cep = 0 Or cep = 11111111 Or cep = 22222222 Or cep = 33333333 Or cep = 44444444 Or _
                               cep = 55555555 Or cep = 66666666 Or cep = 77777777 Or cep = 88888888 Or cep = 99999999 Then
                                .Linhas(i).CIF.CategoriaCEP = Nothing
                                .Linhas(i).CIF.CodigoCEP = Nothing
                                .Linhas(i).CIF.CodigoTriagem = Nothing
                                .Linhas(i).CIF.TipoRegistro = "CepErrado"
                            End If
                            If .Linhas(i).CIF.CodigoTriagem = Nothing Then
                                .Linhas(i).CIF.TipoRegistro = "CepErrado"
                            End If
                        End If
                        If Not ValidaCEP(SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"), .Linhas(i).Destinatario.Endereco.Estado) Then
                            .Linhas(i).CIF.TipoRegistro = "CepErrado"
                        End If
                        .Linhas(i).CIF.CodigoAdministrativo = codigoAdm
                        .Linhas(i).CIF.CodigoPostagem = codigoUnidade
                        .Linhas(i).CIF.CNAE = cnae.ToString.PadLeft(9, "0")
                        .Linhas(i).CIF.ServicoAdicional = servicoAdicional
                        .Linhas(i).CIF.IDV = "01"
                    Next
                    Dim selecao As IEnumerable(Of ICIF) = arquivo.Linhas.OrderBy(Function(arq) arq.CIF.CodigoTriagem & arq.Destinatario.Endereco.CEP)
                    arquivo.Linhas = selecao.ToList
                    Dim peso As Double = 12.5
                    Dim pesoTotal As Double = 0.0
                    Dim idObjeto As Integer = 0
                    Dim texto As String = label.Text
                    For i As Integer = 0 To .Linhas.Count - 1
                        If .Linhas(i).CIF.TipoRegistro <> "CepErrado" And .Linhas(i).CIF.TipoRegistro = processamento Then
                            idObjeto += 1
                            pesoTotal = pesoTotal + peso
                            .Linhas(i).CIF.CodigoCIF = codigoDR.ToString.Trim.PadLeft(2, "0") & _
                                                   codigoAdm.ToString.Trim.PadLeft(8, "0") & _
                                                   numeroLote.ToString.Trim.PadLeft(5, "0") & _
                                                   idObjeto.ToString.Trim.PadLeft(11, "0") & _
                                                   .Linhas(i).CIF.CodigoCEP & "0" & String.Format("{0:ddMMyy}", dataFAC)
                            If producao Then
                                If idObjeto = 1 Then
                                    MidiaCIF.header("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, codigoDR, codigoAdm, numeroCartao, numeroLote, codigoUnidade, cepUnidade, numeroContrato)
                                End If
                                MidiaCIF.detalhe("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, idObjeto, peso, SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"), .Linhas(i).CIF.CategoriaCEP)
                            End If
                            label.Text = texto & vbCrLf & "FAC Simples " & IIf(.Linhas(i).CIF.TipoRegistro.Trim <> "", "(" & .Linhas(i).CIF.TipoRegistro.Trim & ")", "") & "... " & idObjeto
                        End If
                    Next
                    If producao Then
                        If idObjeto > 0 Then
                            MidiaCIF.trailer("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, idObjeto, pesoTotal)
                        End If
                        arqFAC.Close()
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Funcão usada para padronizar os registros dentro do FAC REGISTRADO
        ''' </summary>
        ''' <param name="cliente">Cliente que tenha um contrato FAC</param>
        ''' <param name="arquivo">Objeto com os registros</param>
        ''' <param name="producao">Processamento será produção?</param>
        ''' <param name="dataFAC">Data de Postagem FAC</param>
        ''' <param name="label">Label que será exibida os detalhes do processo</param>
        ''' <remarks></remarks>
        Public Sub processaFACRegistrado(ByVal tipofac As FAC.Tipo, ByVal cliente As FAC.Contrato, ByVal processamento As String, ByRef arquivo As IArquivo, ByVal producao As Boolean, ByVal dataFAC As Date, ByRef label As Label)
            Try
                dataFAC = validaDataPostagem(dataFAC)
                Dim arqFAC As StreamReader
                Dim arqDBF As String = ""
                Dim pastaFAC As String = ""
                Dim caminho As String = "C:\Amarildo\Fac\"
                Select Case cliente
                    Case FAC.Contrato.ACSP
                        arqDBF = "FACACSP.DBF"
                        pastaFAC = "ACSP"
                    Case FAC.Contrato.BVS
                        arqDBF = "FACBVS.DBF"
                        pastaFAC = "BOAVISTA"
                    Case FAC.Contrato.DMCARD
                        arqDBF = "FACDMFAT.DBF"
                        pastaFAC = "DMCARD"
                    Case FAC.Contrato.LOPES
                        arqDBF = "FACLOPES.DBF"
                        pastaFAC = "LOPES"
                    Case FAC.Contrato.OMNI
                        arqDBF = "FAC_OMNI.DBF"
                        pastaFAC = "OMNI"
                End Select
                arqFAC = New StreamReader(caminho & arqDBF)

                Dim codigoDR As Integer = 99
                Dim codigoAdm As Integer = 99999999
                Dim numeroCartao As Long = 999999999999
                Dim numeroLote As Integer = 99999
                Dim codigoUnidade As Integer = 99999999
                Dim cepUnidade As Integer = 99999999
                Dim numeroContrato As Long = 9999999999
                Dim sigla As String = "99"
                Dim cnae As Long = 123456789
                Dim servicoAdicional = "0000000000"

                Dim nomeMidia As String = ""

                Dim conn As New OleDbConnection
                Dim cmd As OleDbCommand = conn.CreateCommand
                Dim dt As DataTable

                If producao Then
                    conn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & caminho & ";Extended Properties=dBASE IV;"
                    conn.Open()
                    cmd.CommandText = "SELECT * FROM " & caminho & arqDBF
                    dt = New DataTable
                    arqFAC.Close()
                    dt.Load(cmd.ExecuteReader)
                    For Each row As DataRow In dt.Rows
                        codigoDR = row("CODIGO_DR")
                        codigoAdm = row("COD_ADMIN")
                        numeroCartao = row("NUM_CARTAO")
                        numeroLote = row("NUM_LOTE")
                        codigoUnidade = row("COD_POSTAG")
                        cepUnidade = row("CEP_ORIGEM")
                        numeroContrato = row("N_CONTRATO")
                        cnae = row("CNAE")
                    Next
                    arqFAC = New StreamReader(caminho & arqDBF)
                    numeroLote += 1
                    cmd.CommandText = "UPDATE " & caminho & arqDBF & " SET NUM_LOTE = " & numeroLote.ToString
                    arqFAC.Close()
                    cmd.ExecuteNonQuery()
                    arqFAC = New StreamReader(caminho & arqDBF)
                    nomeMidia = codigoAdm.ToString.Trim.PadLeft(8, "0") & "_" & _
                                numeroLote.ToString.Trim.PadLeft(5, "0") & "_" & _
                                "UNICA" & "_" & _
                                codigoDR.ToString.Trim.PadLeft(2, "0") & ".txt"
                End If
                Select Case cliente
                    Case FAC.Contrato.ACSP
                        arqDBF = "FACRACSP.DBF"
                        pastaFAC = "ACSP\REGISTRADO"
                    Case FAC.Contrato.BVS
                        arqDBF = "FACRBVS.DBF"
                        pastaFAC = "BOAVISTA\REGISTRADO"
                    Case FAC.Contrato.DMCARD
                        arqDBF = "FACRDM.DBF"
                        pastaFAC = "DMCARD\REGISTRADO"
                    Case FAC.Contrato.LOPES
                        arqDBF = "FACARLOP.DBF"
                        pastaFAC = "LOPES\REGISTRADO"
                    Case FAC.Contrato.OMNI
                        arqDBF = "FACROMNI.DBF"
                        pastaFAC = "OMNI\REGISTRADO"
                End Select
                With arquivo
                    For i As Integer = 0 To .Linhas.Count - 1
                        If .Linhas(i).CIF.TipoRegistro <> "CepErrado" And .Linhas(i).CIF.TipoRegistro = processamento Then
                            Dim cep As Integer = Integer.Parse(SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"))
                            .Linhas(i).CIF.CategoriaCEP = DefinirCategoriaCEP(SomenteNumeros(.Linhas(i).Destinatario.Endereco.CEP).PadLeft(8, "0"), tipofac)
                            DefinirTriagemCEP(.Linhas(i))
                            If cep = 0 Or cep = 11111111 Or cep = 22222222 Or cep = 33333333 Or cep = 44444444 Or _
                               cep = 55555555 Or cep = 66666666 Or cep = 77777777 Or cep = 88888888 Or cep = 99999999 Then
                                .Linhas(i).CIF.CategoriaCEP = Nothing
                                .Linhas(i).CIF.CodigoCEP = Nothing
                                .Linhas(i).CIF.CodigoTriagem = Nothing
                                .Linhas(i).CIF.TipoRegistro = "CepErrado"
                            End If
                            If .Linhas(i).CIF.CodigoTriagem = Nothing Then
                                .Linhas(i).CIF.TipoRegistro = "CepErrado"
                            End If
                        End If
                        If Not ValidaCEP(.Linhas(i).Destinatario.Endereco.CEP, .Linhas(i).Destinatario.Endereco.Estado) Then
                            .Linhas(i).CIF.TipoRegistro = "CepErrado"
                        End If
                        .Linhas(i).CIF.CodigoAdministrativo = codigoAdm
                        .Linhas(i).CIF.CodigoPostagem = codigoUnidade
                        .Linhas(i).CIF.CNAE = cnae.ToString.PadLeft(9, "0")
                        .Linhas(i).CIF.ServicoAdicional = servicoAdicional
                        .Linhas(i).CIF.IDV = "02"
                    Next
                    Dim selecao As IEnumerable(Of ICIF) = arquivo.Linhas.OrderBy(Function(arq) arq.CIF.CodigoTriagem & arq.Destinatario.Endereco.CEP)
                    arquivo.Linhas = selecao.ToList
                    Dim peso As Double = 12.5
                    Dim pesoTotal As Double = 0.0
                    Dim idObjeto As Integer = 0
                    Dim cont As Integer = 0
                    Dim texto As String = label.Text
                    For i As Integer = 0 To .Linhas.Count - 1
                        If .Linhas(i).CIF.TipoRegistro <> "CepErrado" And .Linhas(i).CIF.TipoRegistro = processamento Then
                            cont += 1
                            If producao Then
                                cmd.CommandText = "SELECT TOP 1 * FROM " & caminho & arqDBF & " WHERE DATAPOSTAG is null"
                                dt = New DataTable
                                dt.Load(cmd.ExecuteReader)
                                For Each row As DataRow In dt.Rows
                                    idObjeto = row("NUM_OBJETO")
                                    sigla = row("SIGLA")
                                Next
                                cmd.CommandText = "UPDATE " & caminho & arqDBF & " SET DATAPOSTAG = '" & String.Format("{0:dd/MM/yyyy}", dataFAC) & "', NUM_LOTE = " & numeroLote & ", CEP = '" & .Linhas(i).Destinatario.Endereco.CEP & "' WHERE NUM_OBJETO = " & idObjeto
                                cmd.ExecuteNonQuery()
                            Else
                                idObjeto = cont
                                sigla = "XX"
                            End If
                            pesoTotal = pesoTotal + peso
                            .Linhas(i).CIF.CodigoCIF = (sigla & String.Format("{0:D8}", idObjeto) & CalculaDVAR(String.Format("{0:D8}", idObjeto)) & "BR").PadRight(34, " ") & numeroLote.ToString.PadLeft(5, "0")
                            If producao Then
                                If cont = 1 Then
                                    MidiaCIF.header("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, codigoDR, codigoAdm, numeroCartao, numeroLote, codigoUnidade, cepUnidade, numeroContrato)
                                End If
                                MidiaCIF.detalhe("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, .Linhas(i).CIF.CodigoCIF.Substring(0, 11), peso, .Linhas(i).Destinatario.Endereco.CEP.Trim.PadLeft(8, "0"), .Linhas(i).CIF.CategoriaCEP)
                            End If
                            If cont > 0 Then
                                If cont > 0 Then
                                    label.Text = texto & vbCrLf & "FAC Registrado " & IIf(.Linhas(i).CIF.TipoRegistro.Trim <> "", "(" & .Linhas(i).CIF.TipoRegistro.Trim & ")", "") & "... " & cont
                                End If
                            End If
                        End If
                    Next
                    If producao Then
                        If idObjeto > 0 Then
                            MidiaCIF.trailer("C:\Amarildo\Fac\" & String.Format("{0:ddMMyy}" & "\", dataFAC) & "\" & pastaFAC & "\", nomeMidia, cont, pesoTotal)
                        End If
                        arqFAC.Close()
                    End If
                    cmd.Dispose()
                    conn.Close()
                    conn.Dispose()
                    GC.Collect()
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' Cálculo usado para obter o dígito verificador do número do objeto - AR
        ''' </summary>
        ''' <param name="ar">Número Objeto</param>
        ''' <returns>Retorna um valor inteiro</returns>
        ''' <remarks></remarks>
        Public Function CalculaDVAR(ByVal ar As String) As Integer
            Try
                If ar.Length <> 8 Then Throw New Exception("Número do AR deve conter 8 dígitos")
                Dim digitos As String() = {8, 6, 4, 2, 3, 5, 9, 7}
                Dim soma As Integer = 0
                Dim dv As Integer = 0
                For x As Integer = 0 To 7
                    soma += ar.Substring(x, 1) * digitos(x)
                Next
                dv = soma Mod 11
                If dv = 0 Then
                    dv = 5
                ElseIf dv = 1 Then
                    dv = 0
                Else
                    dv = 11 - dv
                End If
                Return dv
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function


        Public Function MontarLinhaDigitavel(ByVal codigodebarras As String, ByVal ficha As TipoCobranca) As String
            Try
                If codigodebarras.Length <> 44 Then
                    Throw New Exception("Tamanho do código diferente de 44 posições")
                End If
                Dim lindig As String
                Dim digtav1 As String
                Dim digtav2 As String
                Dim digtav3 As String
                Dim digtov1 As String
                Dim digtov2 As String
                Dim digtov3 As String

                If ficha = TipoCobranca.COMPENSACAO Then
                    lindig = codigodebarras.Substring(0, 4) & codigodebarras.Substring(19)
                    digtav1 = lindig.Substring(0, 9)
                    digtov1 = Funcoes.Mod10(digtav1)
                    digtav2 = lindig.Substring(9, 10)
                    digtov2 = Funcoes.Mod10(digtav2)
                    digtav3 = lindig.Substring(19, 10)
                    digtov3 = Funcoes.Mod10(digtav3)

                    Return digtav1.Insert(5, ".") & digtov1 & "  " & _
                                      digtav2.Insert(5, ".") & digtov2 & "  " & _
                                      digtav3.Insert(5, ".") & digtov3 & "  " & _
                                      codigodebarras.Substring(4, 15).Insert(1, "  ")
                Else
                    lindig = codigodebarras.Substring(0, 4) & codigodebarras.Substring(19)
                    digtav1 = lindig.Substring(0, 9)
                    digtov1 = Funcoes.Mod10(digtav1)
                    digtav2 = lindig.Substring(9, 10)
                    digtov2 = Funcoes.Mod10(digtav2)
                    digtav3 = lindig.Substring(19, 10)
                    digtov3 = Funcoes.Mod10(digtav3)

                    Return digtav1.Insert(5, ".") & digtov1 & "  " & _
                                      digtav2.Insert(5, ".") & digtov2 & "  " & _
                                      digtav3.Insert(5, ".") & digtov3 & "  " & _
                                      codigodebarras.Substring(4, 15).Insert(1, "  ")
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function


        Public Function MontarCodigoDeBarras(ByVal linhadigitavel As String, ByVal ficha As TipoCobranca) As String
            Try
                linhadigitavel = linhadigitavel.Replace(" ", "").Replace(" ", "").Replace(" ", "").Replace(".", "").Replace("-", "").Trim
                If linhadigitavel.Length <> 47 And TipoCobranca.COMPENSACAO Then
                    Throw New Exception("Tamanho da linha digitável diferente de 47 posições")
                End If
                If linhadigitavel.Length <> 48 And TipoCobranca.ARRECADACAO Then
                    Throw New Exception("Tamanho da linha digitável diferente de 48 posições")
                End If
                If ficha = TipoCobranca.COMPENSACAO Then
                    Return linhadigitavel.Substring(0, 4) & linhadigitavel.Substring(32, 15) & _
                           linhadigitavel.Substring(4, 5) & linhadigitavel.Substring(10, 10) & _
                           linhadigitavel.Substring(21, 10)
                Else
                    Return linhadigitavel.Substring(0, 11) & linhadigitavel.Substring(12, 11) & _
                           linhadigitavel.Substring(24, 11) & linhadigitavel.Substring(36, 11)
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function


        ''' <summary>Cálculo Módulo 10</summary>
        ''' <param name="codigo">Sequência numérica para cálculo</param>
        ''' <returns>Retorna um valor Inteiro</returns>
        Public Function Mod10(ByVal codigo As String) As Integer
            Dim i, soma, somad, peso As Integer
            peso = 2
            soma = 0
            somad = 0
            i = codigo.Length
            While i > 0
                somad = codigo.Substring(i - 1, 1) * peso
                i = i - 1
                If somad > 9 Then
                    somad = CType(somad.ToString.Trim.Substring(0, 1), Integer) + _
                            CType(somad.ToString.Trim.Substring(1, 1), Integer)
                End If
                soma = soma + somad
                peso = peso + 1
                If peso > 2 Then peso = 1
            End While
            i = soma.ToString.Trim.Length
            If soma.ToString.Trim.Substring(i - 1, 1) <> 0 Then
                Return 10 - soma.ToString.Trim.Substring(i - 1, 1)
            Else
                Return 0
            End If
        End Function

        Public Function DataMatrix2D(ByVal objeto As ICIF) As String
            Dim conteudo As String = ""
            With objeto
                conteudo += SomenteNumeros(.Destinatario.Endereco.CEP).PadLeft(8, "0")
                conteudo += .Destinatario.Endereco.Numero.PadLeft(5, "0")
                conteudo += SomenteNumeros(.Remetente.Endereco.CEP).PadLeft(8, "0")
                conteudo += .Remetente.Endereco.Numero.PadLeft(5, "0")
                conteudo += Right(PostNet(SomenteNumeros(.Destinatario.Endereco.CEP).PadLeft(8, "0")).Replace("*", ""), 1)
                conteudo += .CIF.IDV
                conteudo += .CIF.CodigoCIF
                conteudo += .CIF.ServicoAdicional
                conteudo += .CIF.CategoriaCEP
                conteudo += "000000000000000"
                conteudo += .CIF.CNAE
                conteudo += "|"
            End With
            Return conteudo
        End Function

        Public Function DataPorExtenso(ByVal data As Date) As String
            Return FormatDateTime(data, DateFormat.LongDate).Replace(Format(data, "dddd"), "")
        End Function

        Public Function NumeroToExtenso(ByVal number As Decimal) As String
            Dim cent As Integer
            Try
                ' se for =0 retorna 0 reais
                If number = 0 Then
                    Return "Zero Reais"
                End If
                ' Verifica a parte decimal, ou seja, os centavos
                cent = Decimal.Round((number - Int(number)) * 100, MidpointRounding.ToEven)
                ' Verifica apenas a parte inteira
                number = Int(number)
                ' Caso existam centavos
                If cent > 0 Then
                    ' Caso seja 1 não coloca "Reais" mas sim "Real"
                    If number = 1 Then
                        Return "Um Real e " + getDecimal(cent) + " Centavos"
                        ' Caso o valor seja inferior a 1 Real
                    ElseIf number = 0 Then
                        Return getDecimal(cent) + " Centavos"
                    Else
                        Return getInteger(number) + " Reais e " + getDecimal(cent) + " Centavos"
                    End If
                Else
                    ' Caso seja 1 não coloca "Reais" mas sim "Real"
                    If number = 1 Then
                        Return "Um Real"
                    Else
                        Return getInteger(number) + " Reais"
                    End If
                End If
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Função auxiliar - Parte decimal a converter
        ''' </summary>
        ''' <param name="number">Parte decimal a converter</param>
        Public Function getDecimal(ByVal number As Byte) As String
            Try
                Select Case number
                    Case 0
                        Return ""
                    Case 1 To 19
                        Dim strArray() As String = _
                           {"Um", "Dois", "Três", "Quatro", "Cinco", "Seis", _
                            "Sete", "Oito", "Nove", "Dez", "Onze", _
                            "Doze", "Treze", "Quatorze", "Quinze", _
                            "Dezesseis", "Dezessete", "Dezoito", "Dezenove"}
                        Return strArray(number - 1) + " "
                    Case 20 To 99
                        Dim strArray() As String = _
                            {"Vinte", "Trinta", "Quarenta", "Cinquenta", _
                            "Sessenta", "Setenta", "Oitenta", "Noventa"}
                        If (number Mod 10) = 0 Then
                            Return strArray(number \ 10 - 2) + " "
                        Else
                            Return strArray(number \ 10 - 2) + " e " + getDecimal(number Mod 10) + " "
                        End If
                    Case Else
                        Return ""
                End Select
            Catch ex As Exception
                Return ""
            End Try
        End Function

        ''' <summary>
        ''' Função auxiliar - Parte inteira a converter
        ''' </summary>
        ''' <param name="number">Parte inteira a converter</param>
        Public Function getInteger(ByVal number As Decimal) As String
            Try
                number = Int(number)
                Select Case number
                    Case Is < 0
                        Return "-" & getInteger(-number)
                    Case 0
                        Return ""
                    Case 1 To 19
                        Dim strArray() As String = _
                            {"Um", "Dois", "Três", "Quatro", "Cinco", "Seis", _
                            "Sete", "Oito", "Nove", "Dez", "Onze", "Doze", _
                            "Treze", "Quatorze", "Quinze", "Dezesseis", _
                            "Dezessete", "Dezoito", "Dezenove"}
                        Return strArray(number - 1) + " "
                    Case 20 To 99
                        Dim strArray() As String = _
                            {"Vinte", "Trinta", "Quarenta", "Cinquenta", _
                            "Sessenta", "Setenta", "Oitenta", "Noventa"}
                        If (number Mod 10) = 0 Then
                            Return strArray(number \ 10 - 2)
                        Else
                            Return strArray(number \ 10 - 2) + " e " + getInteger(number Mod 10)
                        End If
                    Case 100
                        Return "Cem"
                    Case 101 To 999
                        Dim strArray() As String = _
                               {"Cento", "Duzentos", "Trezentos", "Quatrocentos", "Quinhentos", _
                               "Seiscentos", "Setecentos", "Oitocentos", "Novecentos"}
                        If (number Mod 100) = 0 Then
                            Return strArray(number \ 100 - 1) + " "
                        Else
                            Return strArray(number \ 100 - 1) + " e " + getInteger(number Mod 100)
                        End If
                    Case 1000 To 1999
                        Select Case (number Mod 1000)
                            Case 0
                                Return "Mil"
                            Case Is <= 100
                                Return "Mil e " + getInteger(number Mod 1000)
                            Case Else
                                Return "Mil, " + getInteger(number Mod 1000)
                        End Select
                    Case 2000 To 999999
                        Select Case (number Mod 1000)
                            Case 0
                                Return getInteger(number \ 1000) & "Mil"
                            Case Is <= 100
                                Return getInteger(number \ 1000) & "Mil e " & getInteger(number Mod 1000)
                            Case Else
                                Return getInteger(number \ 1000) & "Mil, " & getInteger(number Mod 1000)
                        End Select
                    Case 1000000 To 1999999
                        Select Case (number Mod 1000000)
                            Case 0
                                Return "Um Milhão"
                            Case Is <= 100
                                Return getInteger(number \ 1000000) + "Milhão e " & getInteger(number Mod 1000000)
                            Case Else
                                Return getInteger(number \ 1000000) + "Milhão, " & getInteger(number Mod 1000000)
                        End Select
                    Case 2000000 To 999999999
                        Select Case (number Mod 1000000)
                            Case 0
                                Return getInteger(number \ 1000000) + " Milhões"
                            Case Is <= 100
                                Return getInteger(number \ 1000000) + "Milhões e " & getInteger(number Mod 1000000)
                            Case Else
                                Return getInteger(number \ 1000000) + "Milhões, " & getInteger(number Mod 1000000)
                        End Select
                    Case 1000000000 To 1999999999
                        Select Case (number Mod 1000000000)
                            Case 0
                                Return "Um Bilhão"
                            Case Is <= 100
                                Return getInteger(number \ 1000000000) + "Bilhão e " + getInteger(number Mod 1000000000)
                            Case Else
                                Return getInteger(number \ 1000000000) + "Bilhão, " + getInteger(number Mod 1000000000)
                        End Select
                    Case Else
                        Select Case (number Mod 1000000000)
                            Case 0
                                Return getInteger(number \ 1000000000) + " Bilhões"
                            Case Is <= 100
                                Return getInteger(number \ 1000000000) + "Bilhões e " + getInteger(number Mod 1000000000)
                            Case Else
                                Return getInteger(number \ 1000000000) + "Bilhões, " + getInteger(number Mod 1000000000)
                        End Select
                End Select
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Sub escreverFrase(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal xx As Double, ByVal yy As Double, ByVal espacamento As Double, ByVal alinhamento As Integer, ByVal tabulacao As Single, ByVal rotacao As Double, ByVal texto As String, ByVal cor As BaseColor)
            Dim frase As New Phrase
            Dim pedaco As New Chunk

            Dim linha As String() = texto.Split("#")
            For i As Integer = 0 To linha.Count - 1
                Dim pedacos As String() = linha(i).Split("|")
                Dim fonte As New Font
                fonte = FontFactory.GetFont(pedacos(0), BaseFont.CP1252, BaseFont.EMBEDDED, Convert.ToSingle(pedacos(1)), Convert.ToInt32(pedacos(2)), cor)
                pedaco = New Chunk(pedacos(3), fonte)
                frase.Add(pedaco)
            Next
            If rotacao = 0 Then
                Dim ct As ColumnText = New ColumnText(cb)
                ct.SetIndent(tabulacao, True)
                ct.SetSimpleColumn(frase, Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y + espacamento), Utilities.MillimetersToPoints(xx), Utilities.MillimetersToPoints(yy), Utilities.MillimetersToPoints(espacamento), alinhamento)
                ct.Go()
            Else
                ColumnText.ShowTextAligned(cb, alinhamento, frase, Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y), rotacao)
            End If

        End Sub

        Public Sub criarBox(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal largura As Double, ByVal altura As Double, ByVal corBorda As BaseColor, ByVal corFundo As BaseColor)
            cb.Rectangle(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y - altura), Utilities.MillimetersToPoints(largura), Utilities.MillimetersToPoints(altura))
            cb.SetColorFill(corFundo)
            cb.SetColorStroke(corBorda)
            cb.FillStroke()
            cb.SetColorFill(BaseColor.BLACK)
            cb.SetColorStroke(BaseColor.BLACK)
            cb.FillStroke()
        End Sub

        Public Sub criarBoxArredondado(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal largura As Double, ByVal altura As Double, ByVal corBorda As BaseColor, ByVal corFundo As BaseColor, ByVal arredondado As Double)
            cb.RoundRectangle(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y - altura), Utilities.MillimetersToPoints(largura), Utilities.MillimetersToPoints(altura), Single.Parse(arredondado))
            cb.SetColorFill(corFundo)
            cb.SetColorStroke(corBorda)
            cb.FillStroke()
            cb.SetColorFill(BaseColor.BLACK)
            cb.SetColorStroke(BaseColor.BLACK)
            cb.FillStroke()
        End Sub

        Public Sub criarLinha(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal largura As Double, ByVal cor As BaseColor)
            cb.MoveTo(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y))
            cb.LineTo(Utilities.MillimetersToPoints(x) + Utilities.MillimetersToPoints(largura), Utilities.MillimetersToPoints(297 - y))
            cb.SetColorStroke(cor)
            cb.Stroke()
            cb.SetColorFill(BaseColor.BLACK)
            cb.Fill()
        End Sub

        Public Sub criarLinhaPontilhada(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal largura As Double, ByVal espaco As Double, ByVal cor As BaseColor)
            Dim quantidade As Integer = Math.Abs(largura / espaco / 2)
            For i As Integer = 0 To quantidade - 1
                criarLinha(cb, x, y, espaco, cor)
                x += espaco * 2
            Next
        End Sub

        Public Sub criarColuna(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal altura As Double, ByVal cor As BaseColor)
            cb.MoveTo(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y) - Utilities.MillimetersToPoints(altura))
            cb.LineTo(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y))
            cb.SetColorStroke(cor)
            cb.Stroke()
            cb.SetColorFill(BaseColor.BLACK)
            cb.Fill()
        End Sub

        Public Sub inserirImagem(ByVal cb As PdfContentByte, ByVal imagem As Image, ByVal x As Double, ByVal y As Double, ByVal alinhamento As Integer, ByVal porcentagem As Double, ByVal rotacao As Double)
            imagem.ScalePercent(porcentagem)
            imagem.RotationDegrees = rotacao
            If rotacao = 180 Then
                imagem.SetAbsolutePosition(Utilities.MillimetersToPoints(x) - imagem.Width, Utilities.MillimetersToPoints(297 - y) - imagem.Height)
            Else
                imagem.SetAbsolutePosition(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y))
            End If
            imagem.Alignment = alinhamento
            cb.AddImage(imagem)
        End Sub

        Public Sub inserirImagem(ByVal cb As PdfContentByte, ByVal imagem As String, ByVal x As Double, ByVal y As Double, ByVal alinhamento As Integer, ByVal porcentagem As Double)
            Dim img As Image = Image.GetInstance(diretorioImagem & imagem)
            img.ScalePercent(porcentagem)
            img.SetAbsolutePosition(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y))
            img.Alignment = alinhamento
            cb.AddImage(img)
        End Sub

        Public Sub inserirImagem(ByVal cb As PdfContentByte, ByVal imagem As String, ByVal x As Double, ByVal y As Double, ByVal alinhamento As Integer, ByVal largura As Double, ByVal altura As Double)
            Dim img As Image = Image.GetInstance(diretorioImagem & imagem)
            img.SetAbsolutePosition(Utilities.MillimetersToPoints(x), Utilities.MillimetersToPoints(297 - y))
            img.ScaleToFit(Utilities.MillimetersToPoints(largura), Utilities.MillimetersToPoints(altura))
            img.Alignment = alinhamento
            cb.AddImage(img)
        End Sub

        Public Sub criarFichaCompensacao(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal numerobanco As Integer)
            Dim cinzaescuro As New BaseColor(160, 160, 160)
            Dim logobanco As String = "logo_ficha_" & numerobanco.ToString.PadLeft(3, "0") & ".gif"
            inserirImagem(cb, logobanco, x + 20, y + 203.8, 0, 45, 6)

            ' Ficha Compensação
            criarBox(cb, x + 147, y + 297 - 93, 48, 9, cinzaescuro, cinzaescuro)
            criarBox(cb, x + 15, y + 297 - 93, 180, 0.2, BaseColor.BLACK, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 91, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Local de Pagamento", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 91, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Vencimento", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 84, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 82.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Beneficiário", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 82.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Agência / Código do Beneficiário", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 78, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 44, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 45, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Número do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 94, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 95, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Espécie Documento", BaseColor.BLACK)
            criarColuna(cb, x + 116, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 117, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Aceite", BaseColor.BLACK)
            criarColuna(cb, x + 126, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 127, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data Processamento", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Nosso Número", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 72, 48, 6, cinzaescuro, cinzaescuro)
            criarLinha(cb, x + 15, y + 297 - 72, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Uso do Banco", BaseColor.BLACK)
            criarColuna(cb, x + 34, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 35, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Carteira", BaseColor.BLACK)
            criarColuna(cb, x + 65, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 66, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Esp. Moeda", BaseColor.BLACK)
            criarColuna(cb, x + 79, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 80, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Qauntidade", BaseColor.BLACK)
            criarColuna(cb, x + 108, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 109, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Valor", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( = ) Valor do Documento", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 66, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 64.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Instruções: (Texto de responsabilidade exclusiva do Beneficiário", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 64.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( - ) Desconto", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 60, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 58.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( - ) Abatimento/Outras Deduções", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 54, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 52.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( + ) Mora / Multa", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 48, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 46.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( + ) Outros Acréscimos", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 42, 48, 6, cinzaescuro, cinzaescuro)
            criarLinha(cb, x + 147, y + 297 - 42, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 40.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( = ) Valor Cobrado)", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 93, 0.2, 57, BaseColor.BLACK, BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 36, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 34.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Pagador", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 20, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 20.7, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Sacador/Avalista:", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 20.7, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Código da Baixa", BaseColor.BLACK)
            escreverFrase(cb, x + 137, y + 297 - 18.1, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Autenticação Mecânica", BaseColor.BLACK)
            escreverFrase(cb, x + 163, y + 297 - 17.6, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|6|1|FICHA DE COMPENSAÇÃO", BaseColor.BLACK)
            ' Ficha Compensação
        End Sub

        Public Sub criarFichaCompensacaoFatura(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal numerobanco As Integer)
            Dim cinzaescuro As New BaseColor(160, 160, 160)
            Dim logobanco As String = "logo_ficha_" & numerobanco.ToString.PadLeft(3, "0") & ".gif"
            inserirImagem(cb, logobanco, x + 20, y + 203.8, 0, 45, 6)
            ' Ficha Compensação
            criarBox(cb, x + 147, y + 297 - 93, 48, 9, cinzaescuro, cinzaescuro)
            criarBox(cb, x + 15, y + 297 - 93, 180, 0.2, BaseColor.BLACK, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 91, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Local de Pagamento", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 91, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Vencimento", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 84, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 82.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Beneficiário", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 82.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Agência / Código do Beneficiário", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 78, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 44, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 45, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Número do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 94, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 95, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Espécie Documento", BaseColor.BLACK)
            criarColuna(cb, x + 116, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 117, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Aceite", BaseColor.BLACK)
            criarColuna(cb, x + 126, y + 297 - 78, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 127, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data Processamento", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 76.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Nosso Número", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 72, 48, 6, cinzaescuro, cinzaescuro)
            criarLinha(cb, x + 15, y + 297 - 72, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Uso do Banco", BaseColor.BLACK)
            criarColuna(cb, x + 34, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 35, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Carteira", BaseColor.BLACK)
            criarColuna(cb, x + 65, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 66, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Esp. Moeda", BaseColor.BLACK)
            criarColuna(cb, x + 79, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 80, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Qauntidade", BaseColor.BLACK)
            criarColuna(cb, x + 108, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 109, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Valor", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 70.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( = ) Total desta Fatura", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 66, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 64.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Instruções: (Texto de responsabilidade exclusiva do Beneficiário", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 64.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( - ) Pagamento Mínimo", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 60, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 58.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( - ) Desconto/Abatimento/Outras Deduções", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 54, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 52.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( + ) Mora / Multa", BaseColor.BLACK)
            criarLinha(cb, x + 147, y + 297 - 48, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 46.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( + ) Outros Acréscimos", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 42, 48, 6, cinzaescuro, cinzaescuro)
            criarLinha(cb, x + 147, y + 297 - 42, 48, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 40.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|( = ) Valor Cobrado)", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 93, 0.2, 57, BaseColor.BLACK, BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 36, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 34.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Pagador", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 20, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 20.7, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Sacador/Avalista:", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 20.7, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Código da Baixa", BaseColor.BLACK)
            escreverFrase(cb, x + 137, y + 297 - 18.1, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Autenticação Mecânica", BaseColor.BLACK)
            escreverFrase(cb, x + 163, y + 297 - 17.6, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|6|1|FICHA DE COMPENSAÇÃO", BaseColor.BLACK)
            ' Ficha Compensação
        End Sub

        Public Sub criarReciboPagador(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double)
            Dim cinzaescuro As New BaseColor(160, 160, 160)

            ' Recibo Pagador
            escreverFrase(cb, x + 160, y + 297 - 130, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|10|1|Recibo do Pagador", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 128, 48, 6, cinzaescuro, cinzaescuro)
            criarBox(cb, x + 15, y + 297 - 128, 180, 0.2, BaseColor.BLACK, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 126.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Vencimento", BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 126.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Beneficiário", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 122, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Nosso Número", BaseColor.BLACK)
            criarColuna(cb, x + 54, y + 297 - 122, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 55, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Número do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 94, y + 297 - 122, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 95, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Espécie Documento", BaseColor.BLACK)
            criarColuna(cb, x + 116, y + 297 - 122, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 117, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Aceite", BaseColor.BLACK)
            criarColuna(cb, x + 126, y + 297 - 122, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 127, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data Processamento", BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 120.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Agência / Código Beneficiário", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 116, 48, 6, cinzaescuro, cinzaescuro)
            criarLinha(cb, x + 15, y + 297 - 116, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 114.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Pagador", BaseColor.BLACK)
            criarColuna(cb, x + 34, y + 297 - 72, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 148, y + 297 - 114.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Valor do Documento", BaseColor.BLACK)
            criarBox(cb, x + 147, y + 297 - 128, 0.2, 18, BaseColor.BLACK, BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 110, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 160, y + 297 - 108, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Autenticação Mecânica", BaseColor.BLACK)
            ' Recibo Pagador
        End Sub

        Public Sub criarReciboPagador(ByVal cb As PdfContentByte, ByVal x As Double, ByVal y As Double, ByVal numerobanco As Integer)
            Dim cinzaescuro As New BaseColor(160, 160, 160)
            Dim logobanco As String = "logo_ficha_" & numerobanco.ToString.PadLeft(3, "0") & ".gif"
            inserirImagem(cb, logobanco, x + 20, y + 180, 0, 45, 6)

            ' Recibo Pagador
            criarBox(cb, x + 140, y + 297 - 111, 55, 6, cinzaescuro, cinzaescuro)
            escreverFrase(cb, x + 160, y + 297 - 119, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|10|1|Recibo do Pagador", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 117, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 115.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Beneficiário", BaseColor.BLACK)
            criarColuna(cb, x + 134, y + 297 - 117, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 135, y + 297 - 115.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Agência / Código Beneficiário", BaseColor.BLACK)
            criarColuna(cb, x + 164, y + 297 - 117, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 165, y + 297 - 115.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Nosso Número", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 111, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 16, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Data do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 35, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 36, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Espécie Moeda", BaseColor.BLACK)
            criarColuna(cb, x + 51, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 52, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Espécie Documento", BaseColor.BLACK)
            criarColuna(cb, x + 71, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 72, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Aceite", BaseColor.BLACK)
            criarColuna(cb, x + 78, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 79, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Número do Documento", BaseColor.BLACK)
            criarColuna(cb, x + 116, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 117, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Uso Banco", BaseColor.BLACK)
            criarColuna(cb, x + 134, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 135, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Cip", BaseColor.BLACK)
            criarColuna(cb, x + 140, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 141, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Vencimento", BaseColor.BLACK)
            criarColuna(cb, x + 164, y + 297 - 111, 6, BaseColor.BLACK)
            escreverFrase(cb, x + 165, y + 297 - 109.2, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Valor do Documento", BaseColor.BLACK)
            criarLinha(cb, x + 15, y + 297 - 105, 180, BaseColor.BLACK)
            escreverFrase(cb, x + 160, y + 297 - 103, x + 195, 1, 4, Element.ALIGN_LEFT, 0, 0, "Arial|5|1|Autenticação Mecânica", BaseColor.BLACK)
            ' Recibo Pagador
        End Sub

    End Module

End Namespace
