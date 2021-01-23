Namespace FichaCompensacao.Unibanco

    Public NotInheritable Class Unibanco
        Inherits Compensacao

        Enum TipoCobranca As Integer
            ComRegistro = 1
            SemRegistro = 2
        End Enum

        Private _TipoCobranca As TipoCobranca
        Private _Linhadigitavel As String
        Private _CodigoDeBarras As String
        Private _CodigoCliente As Integer

        ''' <summary>Ficha de Compensação Padrão - Unibanco</summary>
        ''' <param name="fnTipoCobranca">Tipo de Cobrança (Com Registro ou Sem Registro)</param>
        ''' <param name="fnCodigoCliente">Código do Cliente</param>
        ''' <param name="fnAgencia">Número da Agência sem Dígito Verificador</param>
        ''' <param name="fnConta">Número da Conta sem Dígito Verificador</param>
        ''' <param name="fnDVConta">Dígito Verificador da Conta</param>
        ''' <param name="fnNossoNumero">Nosso Número sem formatação</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        Public Sub New(ByVal fnTipoCobranca As TipoCobranca, ByVal fnCodigoCliente As Integer, ByVal fnAgencia As Integer, ByVal fnConta As Integer, ByVal fnDVConta As String, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, fnAgencia, 0, fnConta, fnDVConta, 409, 0)
            Try
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnCodigoCliente = 0 Then
                    Throw New Exception("É necessário informar o Código do Cliente para fazer a cobrança")
                End If
                _CodigoCliente = fnCodigoCliente
                _TipoCobranca = fnTipoCobranca
                If TipoCob = TipoCobranca.SemRegistro Then
                    Me.Carteira = "CNR"
                    Me.UsoBanco = "CVT: 7744-5"
                    If fnCodigoCliente.ToString.Length > 7 Then
                        Throw New Exception("O Código do Cliente só pode ter no máximo 7 caracteres")
                    End If
                    If fnNossoNumero.ToString.Length > 14 Then
                        Throw New Exception("O Nosso Número só pode ter no máximo 14 caracteres")
                    End If
                ElseIf TipoCob = TipoCobranca.ComRegistro Then
                    Me.Carteira = "CCR"
                    Me.UsoBanco = "CVT: 5539-5"
                    If fnCodigoCliente.ToString.Length > 5 Then
                        Throw New Exception("O Código do Cliente só pode ter no máximo 5 caracteres")
                    End If
                    If fnNossoNumero.ToString.Length > 10 Then
                        Throw New Exception("O Nosso Número só pode ter no máximo 10 caracteres")
                    End If
                End If
                Me.LocalPagamento = "PAGÁVEL EM QUALQUER BANCO ATÉ O VENCIMENTO"
                Me.Aceite = "N"
                Me.EspecieDocumento = "DM"
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
            AgenciaCodigoCedente = Agencia & " / " & String.Format("{0:d6}", Conta).Insert(3, ".") & "-" & DVConta
        End Sub

        ''' <summary>Formatação do Campo "Nosso Número" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoNossoNumero()
            If TipoCob = TipoCobranca.ComRegistro Then
                Dim dignosso As Integer
                Dim superdv As Integer
                dignosso = Funcoes.Mod11(String.Format("{0:d10}", NumeroIdentificação), 2, 9)
                If dignosso > 9 Then
                    dignosso = 0
                End If
                superdv = Funcoes.Mod11("1" & String.Format("{0:d10}", NumeroIdentificação) & dignosso, 2, 9)
                If superdv > 9 Then
                    superdv = 0
                End If
                NossoNumero = "1/" & String.Format("{0:d10}", NumeroIdentificação) & "-" & dignosso & "/" & superdv
            Else
                Dim dignosso As String
                dignosso = Funcoes.Mod11(String.Format("{0:d14}", NumeroIdentificação), 2, 9)
                NossoNumero = String.Format("{0:d14}", NumeroIdentificação) & "-" & IIf(dignosso > 9, 0, dignosso)
            End If
        End Sub

        Protected Overrides Sub montaCodigoDeBarras()
            Dim codbar As String
            Dim fator As Long
            Dim mvalor As String
            Dim digbar As Integer
            Dim campolivre As String

            fator = String.Format("{0:d4}", CInt(DateDiff(DateInterval.Day, CDate("07/10/1997"), DataVencimento)))
            mvalor = String.Format("{0:D10}", CType(String.Format("{0:n2}", ValorDocumento).ToString.Replace(",", "").Replace(".", ""), Integer))

            If TipoCob = TipoCobranca.SemRegistro Then
                campolivre = "5" & String.Format("{0:d7}", _CodigoCliente) & "00" & NossoNumero.Replace("-", "")
            Else
                campolivre = "04" & Format(DataVencimento, "yyMMdd") & String.Format("{0:d5}", _CodigoCliente) & NossoNumero.Replace("-", "").Substring(2)
            End If

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

        Public ReadOnly Property TipoCob() As TipoCobranca
            Get
                Return _TipoCobranca
            End Get
        End Property

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

