Namespace Padrao

    Public Class Parcela

        Private _numero As String
        Public Property Numero() As String
            Get
                Return _numero
            End Get
            Set(ByVal value As String)
                _numero = value
            End Set
        End Property

        Private _plano As String
        Public Property Plano() As String
            Get
                Return _plano
            End Get
            Set(ByVal value As String)
                _plano = value
            End Set
        End Property

        Private _linhadigitavel As String
        Public Property LinhaDigitavel() As String
            Get
                Return _linhadigitavel
            End Get
            Set(ByVal value As String)
                _linhadigitavel = value
            End Set
        End Property

        Private _vencimento As String
        Public Property Vencimento() As String
            Get
                Return _vencimento
            End Get
            Set(ByVal value As String)
                _vencimento = value
            End Set
        End Property

        Private _numerodocumento As String
        Public Property NumeroDocumento() As String
            Get
                Return _numerodocumento
            End Get
            Set(ByVal value As String)
                _numerodocumento = value
            End Set
        End Property

        Private _nossonumero As String
        Public Property NossoNumero() As String
            Get
                Return _nossonumero
            End Get
            Set(ByVal value As String)
                _nossonumero = value
            End Set
        End Property

        Private _quantidade As String
        Public Property Quantidade() As String
            Get
                Return _quantidade
            End Get
            Set(ByVal value As String)
                _quantidade = value
            End Set
        End Property

        Private _valor As String
        Public Property Valor() As String
            Get
                Return _valor
            End Get
            Set(ByVal value As String)
                _valor = value
            End Set
        End Property

        Private _valordocumento As String
        Public Property ValorDocumento() As String
            Get
                Return _valordocumento
            End Get
            Set(ByVal value As String)
                _valordocumento = value
            End Set
        End Property

        Private _descontoabatimento As String
        Public Property DescontoAbatimento() As String
            Get
                Return _descontoabatimento
            End Get
            Set(ByVal value As String)
                _descontoabatimento = value
            End Set
        End Property

        Private _outrasdeducoes As String
        Public Property OutrasDeducoes() As String
            Get
                Return _outrasdeducoes
            End Get
            Set(ByVal value As String)
                _outrasdeducoes = value
            End Set
        End Property

        Private _moramultajuros As String
        Public Property MoraMultaJuros() As String
            Get
                Return _moramultajuros
            End Get
            Set(ByVal value As String)
                _moramultajuros = value
            End Set
        End Property

        Private _outrosacrescimos As String
        Public Property OutrosAcrescimos() As String
            Get
                Return _outrosacrescimos
            End Get
            Set(ByVal value As String)
                _outrosacrescimos = value
            End Set
        End Property

        Private _valorcobrado As String
        Public Property ValorCobrado() As String
            Get
                Return _valorcobrado
            End Get
            Set(ByVal value As String)
                _valorcobrado = value
            End Set
        End Property

        Private _codigodebarra As String
        Public Property CodigoDeBarra() As String
            Get
                Return _codigodebarra
            End Get
            Set(ByVal value As String)
                _codigodebarra = value
            End Set
        End Property

        Private _instrucoes As New List(Of String)
        Public Property Instrucoes() As List(Of String)
            Get
                Return _instrucoes
            End Get
            Set(ByVal value As List(Of String))
                _instrucoes = value
            End Set
        End Property

    End Class

End Namespace