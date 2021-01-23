Namespace FichaCompensacao.Itau

    Public Class Itau
        Inherits Compensacao

        Enum TipoCarteira As Integer
            Normal = 1
            Especial = 2
        End Enum

        Private _TipoCobranca As TipoCarteira
        Private _Linhadigitavel As String
        Private _CodigoDeBarras As String
        Private _CodigoCliente As Integer

        ''' <summary>Ficha de Compensação Padrão - Itaú</summary>
        ''' <param name="fnTipoCobranca">Modalidade da Carteira</param>
        ''' <param name="fnCarteira">Número da Carteira</param>
        ''' <param name="fnCodigoCliente">Código do Cliente para as carteiras especiais, senão informar 0</param>
        ''' <param name="fnAgencia">Número da Agência sem Dígito Verificador</param>
        ''' <param name="fnConta">Número da Conta sem Dígito Verificador</param>
        ''' <param name="fnNossoNumero">Nosso Número sem formatação</param>
        ''' <param name="fnValorDocumento">Valor do Documento</param>
        ''' <param name="fnVencimento">Data de Vencimento</param>
        Public Sub New(ByVal fnTipoCobranca As TipoCarteira, ByVal fnCarteira As Integer, ByVal fnCodigoCliente As Integer, ByVal fnAgencia As Integer, ByVal fnConta As Integer, ByVal fnNossoNumero As Long, ByVal fnValorDocumento As Double, ByVal fnVencimento As Date)
            MyBase.New(fnNossoNumero, fnValorDocumento, fnVencimento, fnAgencia, 0, fnConta, 0, 341, 7)
            Try
                If fnNossoNumero = 0 Then
                    Throw New Exception("É necessário informar o Nosso Número para fazer a cobrança")
                End If
                If fnAgencia = 0 Then
                    Throw New Exception("É necessário informar a Agência para fazer a cobrança")
                End If
                If fnCarteira = 0 Then
                    Throw New Exception("É necessário informar a Carteira para fazer a cobrança")
                End If
                If fnAgencia.ToString.Length > 4 Then
                    Throw New Exception("A Agência só pode ter no máximo 4 caracteres")
                End If
                If TipoCob = TipoCarteira.Especial Then
                    If fnCodigoCliente = 0 Then
                        Throw New Exception("É necessário informar o Código do Cliente para fazer a cobrança")
                    End If
                    If _CodigoCliente.ToString.Length > 5 Then
                        Throw New Exception("O Código do Cliente só pode ter no máximo 5 caracteres")
                    End If
                    If fnNossoNumero.ToString.Length > 15 Then
                        Throw New Exception("O Código do Cliente só pode ter no máximo 15 caracteres")
                    End If
                Else
                    If fnConta = 0 Then
                        Throw New Exception("É necessário informar a Conta para fazer a cobrança")
                    End If
                    If fnConta.ToString.Length > 5 Then
                        Throw New Exception("A Conta só pode ter no máximo 5 caracteres")
                    End If
                    If fnNossoNumero.ToString.Length > 8 Then
                        Throw New Exception("O Código do Cliente só pode ter no máximo 8 caracteres")
                    End If
                End If
                _CodigoCliente = fnCodigoCliente
                _TipoCobranca = fnTipoCobranca
                Me.Carteira = fnCarteira
                Me.LocalPagamento = "Até o vencimento pagável preferencialmente no Itaú, após pagável em qualquer banco"
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
            Dim dv As Integer
            If TipoCob = TipoCarteira.Normal Then
                dv = Funcoes.Mod10(String.Format("{0:d4}", Agencia) & String.Format("{0:d5}", Conta))
                AgenciaCodigoCedente = String.Format("{0:d4}", Agencia) & " / " & String.Format("{0:d5}", Conta) & "-" & dv
            Else
                dv = Funcoes.Mod10(String.Format("{0:d4}", Agencia) & String.Format("{0:d5}", _CodigoCliente))
                AgenciaCodigoCedente = String.Format("{0:d4}", Agencia) & " / " & String.Format("{0:d5}", _CodigoCliente) & "-" & dv
            End If
        End Sub

        ''' <summary>Formatação do Campo "Nosso Número" na ficha de compensação</summary>
        Protected Overrides Sub formataCampoNossoNumero()
            If TipoCob = TipoCarteira.Normal Then
                Dim dignosso As String
                Dim nosso As String
                nosso = String.Format("{0:d4}", Agencia) & String.Format("{0:d5}", Conta) & String.Format("{0:d3}", CInt(Carteira)) & String.Format("{0:d8}", NumeroIdentificação)
                dignosso = Funcoes.Mod10(nosso)
                NossoNumero = String.Format("{0:d3}", CInt(Carteira)) & "/" & String.Format("{0:d8}", NumeroIdentificação) & "-" & dignosso
            Else
                Dim dignosso As String
                Dim digseu As String
                Dim nosso As String
                Dim seu As String
                nosso = String.Format("{0:d15}", NumeroIdentificação).Substring(0, 8)
                dignosso = Funcoes.Mod10(String.Format("{0:d4}", Agencia) & String.Format("{0:d5}", _CodigoCliente) & String.Format("{0:d3}", CInt(Carteira)) & nosso)
                NossoNumero = String.Format("{0:d3}", CInt(Carteira)) & "/" & nosso & "-" & dignosso
                seu = String.Format("{0:d15}", NumeroIdentificação).Substring(8, 7)
                digseu = Funcoes.Mod10(seu)
                NumeroDocumento = seu & "-" & digseu
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

            If TipoCob = TipoCarteira.Normal Then
                campolivre = String.Format("{0:d3}", CInt(Carteira)) & String.Format("{0:d8}", NumeroIdentificação) & Right(NossoNumero, 1) & String.Format("{0:d4}", Agencia) & String.Format("{0:d5}", Conta) & Right(AgenciaCodigoCedente, 1) & "000"
            Else
                Dim dac As Integer
                dac = Funcoes.Mod10(String.Format("{0:d3}", CInt(Carteira)) & NossoNumero.Substring(4, 8) & NumeroDocumento.Substring(0, 7) & String.Format("{0:d5}", _CodigoCliente))
                campolivre = String.Format("{0:d3}", CInt(Carteira)) & NossoNumero.Substring(4, 8) & NumeroDocumento.Substring(0, 7) & String.Format("{0:d5}", _CodigoCliente) & dac & "0"
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

        Public ReadOnly Property TipoCob() As TipoCarteira
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
