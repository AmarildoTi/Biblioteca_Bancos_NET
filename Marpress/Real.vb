Namespace FichaCompensacao.Real

    Public NotInheritable Class Real
        Inherits Compensacao

        Enum NumeroBanco As Integer
            Real_275 = 2755
            Real_356 = 3565
        End Enum

        Private _Linhadigitavel As String
        Private _CodigoDeBarras As String


        ''' <summary>Ficha de Compensação Padrão - Banco Real ABN AMRO</summary>
        ''' <param name="fnAgencia">Número da Agência sem Dígito Verificador</param>
        ''' <param name="fnConta">Número da Conta sem Dígito Verificador</param>
        ''' <param name="fnNossoNumero">Nosso Número sem formatação</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        Public Sub New(ByVal fnNumeroBanco As NumeroBanco, ByVal fnAgencia As Integer, ByVal fnConta As Integer, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, fnAgencia, 0, fnConta, 0, String.Format("{0:d4}", CInt(fnNumeroBanco)).Substring(0, 3), String.Format("{0:d4}", CInt(fnNumeroBanco)).Substring(3, 1))
            Try
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnAgencia = 0 Then
                    Throw New Exception("É necessário informar a Agência para fazer a cobrança")
                End If
                If fnConta = 0 Then
                    Throw New Exception("É necessário informar a Conta para fazer a cobrança")
                End If
                If fnAgencia.ToString.Length > 4 Then
                    Throw New Exception("A Agência só pode ter no máximo 4 caracteres")
                End If
                If fnNossoNumero.ToString.Length > 13 Then
                    Throw New Exception("A Modalidade só pode ter no máximo 13 caracteres")
                End If
                If fnConta.ToString.Length > 7 Then
                    Throw New Exception("A Conta só pode ter no máximo 7 caracteres")
                End If
                Me.Carteira = "20"
                Me.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO"
                Me.Aceite = "N"
                Me.EspecieDocumento = "RC"
                formataCampoAgenciaCodigoCedente()
                formataCampoNossoNumero()
                montaCodigoDeBarras()
                montaLinhaDigitavel()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub


        ''' <summary>Formatação do Campo "Agência/Código do Cedente" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoAgenciaCodigoCedente()
            Dim dv As Integer
            dv = Funcoes.Mod10(NumeroIdentificação.ToString & String.Format("{0:d4}", Agencia) & String.Format("{0:d7}", Conta))
            AgenciaCodigoCedente = Agencia & " / " & Conta & "-" & dv
        End Sub

        ''' <summary>Formatação do Campo "Nosso Número" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoNossoNumero()
            NossoNumero = String.Format("{0:d13}", NumeroIdentificação)
        End Sub

        Protected Overrides Sub montaCodigoDeBarras()
            Dim codbar As String
            Dim fator As Long
            Dim mvalor As String
            Dim digbar As Integer
            Dim campolivre As String

            fator = String.Format("{0:d4}", CInt(DateDiff(DateInterval.Day, CDate("07/10/1997"), DataVencimento)))
            mvalor = String.Format("{0:D10}", CType(String.Format("{0:n2}", ValorDocumento).ToString.Replace(",", "").Replace(".", ""), Integer))
            campolivre = String.Format("{0:d4}", Agencia) & String.Format("{0:d7}", Conta) & Right(AgenciaCodigoCedente, 1) & String.Format("{0:d13}", NumeroIdentificação)
            codbar = String.Format("{0:d3}", Banco) & "9" & fator & mvalor & campolivre
            digbar = IIf(Funcoes.Mod11(codbar, 2, 9) = 0 Or Funcoes.Mod11(codbar, 2, 9) > 9, 1, Funcoes.Mod11(codbar, 2, 9))
            codbar = codbar.Substring(0, 4) & digbar.ToString & codbar.Substring(4)
            _CodigoDeBarras = codbar
        End Sub

        Protected Overrides Sub montaLinhaDigitavel()
            Dim lindig As String
            Dim digtav1 As String
            Dim digtav2 As String
            Dim digtav3 As String
            Dim digtov1 As String
            Dim digtov2 As String
            Dim digtov3 As String


            lindig = CodigoDeBarras.Substring(0, 4) & CodigoDeBarras.Substring(19)
            digtav1 = lindig.Substring(0, 9)
            digtov1 = Funcoes.Mod10(digtav1)
            digtav2 = lindig.Substring(9, 10)
            digtov2 = Funcoes.Mod10(digtav2)
            digtav3 = lindig.Substring(19, 10)
            digtov3 = Funcoes.Mod10(digtav3)

            _Linhadigitavel = digtav1.Insert(5, ".") & digtov1 & "  " & _
                              digtav2.Insert(5, ".") & digtov2 & "  " & _
                              digtav3.Insert(5, ".") & digtov3 & "  " & _
                              CodigoDeBarras.Substring(4, 15).Insert(1, "  ")
        End Sub

        Public Overrides ReadOnly Property CodigoDeBarras() As String
            Get
                Return _CodigoDeBarras
            End Get
        End Property

        Public Overrides ReadOnly Property LinhaDigitavel() As String
            Get
                Return _Linhadigitavel
            End Get
        End Property
    End Class

End Namespace
