
Public MustInherit Class Cobranca

    Private _Cedente As String = ""
    Private _DataVencimento As Date
    Private _EspecieMoeda As String = ""
    Private _Quantidade As Double = 0
    Private _Valor As Double = 0
    Private _ValorDocumento As Double = 0
    Private _LocalPagamento As String = ""
    Private _NumeroIdentificacao As Long = 0
    Private _instrucao As List(Of String)

    Public MustOverride ReadOnly Property CodigoDeBarras() As String
    Public MustOverride ReadOnly Property LinhaDigitavel() As String

    Protected MustOverride Sub montaLinhaDigitavel()
    Protected MustOverride Sub montaCodigoDeBarras()

    Public Sub New(ByVal nosso As Long, ByVal valor As Double, ByVal venc As Date)
        _NumeroIdentificacao = nosso
        _ValorDocumento = valor
        _DataVencimento = venc
    End Sub

    Public Property Cedente() As String
        Get
            Return _Cedente
        End Get
        Set(ByVal value As String)
            _Cedente = value
        End Set
    End Property

    Public ReadOnly Property DataVencimento() As Date
        Get
            Return _DataVencimento
        End Get
    End Property

    Public Property EspecieMoeda() As String
        Get
            Return _EspecieMoeda
        End Get
        Set(ByVal value As String)
            _EspecieMoeda = value
        End Set
    End Property

    Public Property Quantidade() As Double
        Get
            Return _Quantidade
        End Get
        Set(ByVal value As Double)
            _Quantidade = value
        End Set
    End Property

    Public Property Valor() As Double
        Get
            Return _Valor
        End Get
        Set(ByVal value As Double)
            _Valor = value
        End Set
    End Property

    Public ReadOnly Property ValorDocumento() As Double
        Get
            Return _ValorDocumento
        End Get
    End Property

    Public Property LocalPagamento() As String
        Get
            Return _LocalPagamento
        End Get
        Set(ByVal value As String)
            _LocalPagamento = value
        End Set
    End Property

    Public Property Instrucao() As List(Of String)
        Get
            Return _instrucao
        End Get
        Set(ByVal value As List(Of String))
            _instrucao = value
        End Set
    End Property

    ''' <summary>Número de identificação, Número para baixa de pagamento (Nosso Número sem formatação)</summary>
    Public ReadOnly Property NumeroIdentificação() As Long
        Get
            Return _NumeroIdentificacao
        End Get
    End Property
End Class
