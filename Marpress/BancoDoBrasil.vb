Namespace FichaCompensacao.BancoDoBrasil

    Public NotInheritable Class BancoDoBrasil
        Inherits Compensacao

        Enum TipoCobranca As Integer
            SemRegistro = 1
            Registrada = 2
        End Enum

        Private _Convenio As Integer
        Private _TipoCobranca As TipoCobranca
        Private _Linhadigitavel As String
        Private _CodigoDeBarras As String

        ''' <summary>Ficha de Compensação Padrão - Banco do Brasil</summary>
        ''' <param name="fnTipoCobranca">Tipo de Cobrança - Carteira Sem Registro ou Carteira Registrada</param>
        ''' <param name="fnConvenio">Número do Convênio</param>
        ''' <param name="fnAgencia">Número da Agência sem Dígito Verificador</param>
        ''' <param name="fnDVAgencia">Dígito Verificador da Agência</param>
        ''' <param name="fnConta">Número da Conta sem Dígito Verificador</param>
        ''' <param name="fnDVConta">Dígito Verificador da Conta</param>
        ''' <param name="fnNossoNumero">Nosso Número</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        ''' <param name="fnCarteira">Número da Carteira</param>
        ''' <param name="fnVariacaoCarteira">Variação da Carteira</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal fnTipoCobranca As TipoCobranca, ByVal fnConvenio As Integer, ByVal fnAgencia As Integer, ByVal fnDVAgencia As String, ByVal fnConta As Integer, ByVal fnDVConta As String, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date, ByVal fnCarteira As Integer, ByVal fnVariacaoCarteira As Integer)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, fnAgencia, fnDVAgencia, fnConta, fnDVConta, 1, 9)
            Try
                If fnConvenio = 0 Then
                    Throw New Exception("É necessário o informar número do Convênio para fazer a cobrança")
                End If
                If fnAgencia = 0 Then
                    Throw New Exception("É necessário o informar número da Agência para fazer a cobrança")
                End If
                If fnConta = 0 Then
                    Throw New Exception("É necessário o informar número da Conta para fazer a cobrança")
                End If
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnTipoCobranca = BancoDoBrasil.TipoCobranca.Registrada Then
                    If fnConvenio.ToString.Length > 4 Then
                        Throw New Exception("Para uma cobrança registrada o convênio deve ter no máximo 4 dígitos")
                    End If
                    If fnNossoNumero.ToString.Length > 7 Then
                        Throw New Exception("Para uma cobrança registrada o Nosso Número deve ter no máximo 7 dígitos")
                    End If
                End If
                If fnTipoCobranca = BancoDoBrasil.TipoCobranca.SemRegistro Then
                    If fnConvenio.ToString.Length > 7 Then
                        Throw New Exception("Para uma cobrança sem registro o convênio deve ter no máximo 7 dígitos")
                    End If
                    If fnConvenio.ToString.Length > 6 Then
                        If fnNossoNumero.ToString.Length > 10 Then
                            Throw New Exception("Para uma cobrança sem registro com convênio de 7 dígitos o Nosso Número deve ter no máximo 10 dígitos")
                        End If
                    Else
                        If fnNossoNumero.ToString.Length > 17 Then
                            Throw New Exception("Para uma cobrança sem registro com convênio de 6 dígitos o Nosso Número deve ter no máximo 17 dígitos")
                        End If
                    End If
                End If
                _Convenio = fnConvenio
                _TipoCobranca = fnTipoCobranca
                Me.Carteira = String.Format("{0:d2}", fnCarteira) & "-" & String.Format("{0:d3}", fnVariacaoCarteira)
                Me.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO"
                Me.Aceite = "N"
                Me.EspecieDocumento = "COB"
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
            AgenciaCodigoCedente = String.Format("{0:d4}", Agencia) & "-" & DVAgencia & " / " & String.Format("{0:d8}", Conta) & "-" & DVConta
        End Sub

        ''' <summary>Formatação do Campo "Nosso Número" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoNossoNumero()
            If TipoCob = TipoCobranca.Registrada Then
                Dim dignosso As String
                Dim nosso As String
                nosso = String.Format("{0:d4}", Convenio) & String.Format("{0:d7}", CType(NumeroIdentificação, Long))
                dignosso = IIf(11 - Funcoes.Mod11(nosso, 9, 2) = 10, "X", 11 - Funcoes.Mod11(nosso, 9, 2))
                NossoNumero = nosso & "-" & dignosso
            Else
                If Convenio.ToString.Length > 6 Then
                    NossoNumero = String.Format("{0:d7}", Convenio) & String.Format("{0:d10}", NumeroIdentificação)
                Else
                    NossoNumero = String.Format("{0:d17}", NumeroIdentificação)
                End If
            End If
        End Sub

        Protected Overrides Sub montaCodigoDeBarras()
            Dim codbar As String
            Dim fator As Long
            Dim mvalor As String
            Dim conv As String
            Dim digbar As Integer
            Dim campolivre As String
            Dim cart As Integer

            fator = String.Format("{0:d4}", CInt(DateDiff(DateInterval.Day, CDate("07/10/1997"), DataVencimento)))
            mvalor = String.Format("{0:D10}", CType(String.Format("{0:n2}", ValorDocumento).ToString.Replace(",", "").Replace(".", ""), Integer))
            If _TipoCobranca = 1 Then
                conv = String.Format("{0:d6}", Convenio)
                If conv.Length > 6 Then
                    conv = 0
                    cart = Carteira.Substring(0, 2)
                Else
                    cart = 21
                End If
                campolivre = conv & NossoNumero & cart
            Else
                campolivre = NossoNumero.Substring(0, 11) & String.Format("{0:d4}", Agencia) & String.Format("{0:d8}", Conta) & Carteira.Substring(0, 2)
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

        Public ReadOnly Property Convenio() As Integer
            Get
                Return _Convenio
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
