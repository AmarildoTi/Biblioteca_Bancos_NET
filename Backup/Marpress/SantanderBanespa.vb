Namespace FichaCompensacao.SantanderBanespa

    Public NotInheritable Class SantanderBanespa
        Inherits Compensacao

        Enum TipoCobranca As Integer
            CobrancaSimplesRapidaComRegistro = 101
            CobrancaSimplesSemRegistro = 102
            CobrancaPenhorComRegistro = 201
            CobrancaAntigaDoBanespa = 33
        End Enum

        Enum NumeroBanco As Integer
            Santander = 3530
            Banespa = 337
            Meridional = 86
        End Enum

        Private _CodigoCedente As String
        Private _TipoCobranca As TipoCobranca
        Private _NumeroBanco As NumeroBanco
        Private _IOF As Integer

        Private _Linhadigitavel As String
        Private _CodigoDeBarras As String

        ''' <summary>Ficha de Compensação Padrão - Santander Banespa</summary>
        ''' <param name="fnNumeroBanco">Número do Banco - Santander = 353, Banespa = 033, Meridional = 008</param>
        ''' <param name="fnTipoCobranca">Modalidade da Carteira</param>
        ''' <param name="fnCodigoCedente">Número do Código do Cedente (PSK)</param>
        ''' <param name="fnIOF">Seguradoras Informar IOF, Demais Informar 0</param>
        ''' <param name="fnAgencia">Número da Agência sem Dígito Verificador</param>
        ''' <param name="fnNossoNumero">Nosso Número sem formatação</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        Public Sub New(ByVal fnNumeroBanco As NumeroBanco, ByVal fnTipoCobranca As TipoCobranca, ByVal fnCodigoCedente As String, ByVal fnIOF As Integer, ByVal fnAgencia As Integer, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, fnAgencia, 0, 0, 0, String.Format("{0:d4}", CInt(fnNumeroBanco)).Substring(0, 3), String.Format("{0:d4}", CInt(fnNumeroBanco)).Substring(3, 1))
            Try
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnCodigoCedente = 0 Then
                    Throw New Exception("É necessário informar o Código do Cedente para fazer a cobrança")
                End If
                If fnAgencia = 0 Then
                    Throw New Exception("É necessário informar a Agência para fazer a cobrança")
                End If
                If fnCodigoCedente.Length > 7 Then
                    Throw New Exception("O Código do Cedente só pode ter no máximo 7 caracteres")
                End If
                If fnNossoNumero.ToString.Length > 12 Then
                    Throw New Exception("O Nosso Número só pode ter no máximo 12 caracteres")
                End If
                _IOF = fnIOF
                _CodigoCedente = fnCodigoCedente
                _TipoCobranca = fnTipoCobranca
                If TipoCob = TipoCobranca.CobrancaSimplesSemRegistro Then
                    Me.Carteira = "CSR"
                Else
                    Me.Carteira = "ECR"
                End If
                Me.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO"
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

        ''' <summary>Ficha de Compensação Padrão - Antigo Banespa</summary>
        ''' <param name="fnCodigoCedente">Código do Cedente com 11 Caracteres</param>
        ''' <param name="fnNossoNumero">Nosso Número sem formatação</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        Public Sub New(ByVal fnCodigoCedente As String, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, 0, 0, 0, 0, 33, 7)
            Try
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnCodigoCedente = 0 Then
                    Throw New Exception("É necessário informar o Código do Cedente para fazer a cobrança")
                End If
                If fnCodigoCedente.Length > 11 Then
                    Throw New Exception("O Código do Cedente só pode ter no máximo 11 caracteres")
                End If
                If fnNossoNumero.ToString.Length > 7 Then
                    Throw New Exception("O Nosso Número só pode ter no máximo 7 caracteres")
                End If
                _CodigoCedente = fnCodigoCedente
                _TipoCobranca = TipoCobranca.CobrancaAntigaDoBanespa
                Me.Carteira = "COB"
                Me.LocalPagamento = "PAGÁVEL EM QUALQUER AGÊNCIA BANCÁRIA ATÉ O VENCIMENTO"
                Me.Aceite = "N"
                Me.EspecieDocumento = "RC-CI"
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
            If TipoCob = TipoCobranca.CobrancaAntigaDoBanespa Then
                AgenciaCodigoCedente = CodigoCedente.Insert(10, " ").Insert(5, " ").Insert(3, " ")
            Else
                AgenciaCodigoCedente = Agencia & " / " & CodigoCedente
            End If
        End Sub

        ''' <summary>Formatação do Campo "Nosso Número" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoNossoNumero()
            If TipoCob = TipoCobranca.CobrancaAntigaDoBanespa Then
                Dim dignosso As String
                Dim nosso As String
                nosso = String.Format("{0:d7}", CType(NumeroIdentificação, Long))
                dignosso = Funcoes.NNBanespa(AgenciaCodigoCedente.Substring(0, 3) & nosso)
                NossoNumero = AgenciaCodigoCedente.Substring(0, 3) & " " & nosso & " " & dignosso
            Else
                Dim dignosso As String
                Dim nosso As String
                nosso = String.Format("{0:d12}", CType(NumeroIdentificação, Long))
                dignosso = IIf(Funcoes.Mod11(nosso, 2, 9) > 9, 0, Funcoes.Mod11(nosso, 2, 9))
                NossoNumero = nosso & dignosso
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

            If TipoCob = TipoCobranca.CobrancaAntigaDoBanespa Then
                Dim resp As Boolean = True
                Dim constante1 As String
                Dim dvBarra1, dvBarra2 As Integer

                constante1 = CodigoCedente & String.Format("{0:d7}", NumeroIdentificação) & String.Format("{0:d5}", CInt(TipoCob))
                dvBarra1 = Funcoes.Mod10(constante1)
                dvBarra2 = Funcoes.Mod11(constante1 & dvBarra1, 2, 7)
                While resp = True
                    If dvBarra2 < 10 Then
                        Exit While
                    End If
                    If dvBarra2 = 11 Then
                        dvBarra2 = 0
                        Exit While
                    ElseIf dvBarra2 = 10 Then
                        If dvBarra1 = 9 Then
                            dvBarra1 = 0
                            dvBarra2 = Funcoes.Mod11(constante1 & dvBarra1.ToString.Trim, 2, 7)
                        Else
                            dvBarra1 = dvBarra1 + 1
                            dvBarra2 = Funcoes.Mod11(constante1 & dvBarra1.ToString.Trim, 2, 7)
                        End If
                    End If
                End While
                campolivre = constante1 & dvBarra1 & dvBarra2
            Else
                campolivre = "9" & String.Format("{0:d7}", CInt(CodigoCedente)) & NossoNumero.Substring(0, 13) & IOF & CInt(TipoCob)
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

        Public ReadOnly Property CodigoCedente() As String
            Get
                Return _CodigoCedente
            End Get
        End Property

        Public ReadOnly Property IOF() As Integer
            Get
                Return _IOF
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
