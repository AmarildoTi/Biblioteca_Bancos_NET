
Public MustInherit Class Compensacao
    Inherits Cobranca

    Private _Aceite As String = 0
    Private _Agencia As Integer = 0
    Private _Banco As Integer = 0
    Private _Carteira As String = ""
    Private _Conta As Integer = 0
    Private _DataDocumento As Date
    Private _DataProcessamento As Date
    Private _Desconto As Double = 0
    Private _DVAgencia As String = ""
    Private _DVConta As String = ""
    Private _DVBanco As Integer = 0
    Private _EspecieDocumento As String = ""
    Private _Mora_Multa_Juros As Double = 0
    Private _NumeroDocumento As String = ""
    Private _OutrasDeducoes_Abatimento As Double = 0
    Private _OutrosAcrescimos As Double = 0
    Private _UsoBanco As String = ""
    Private _NossoNumero As String = ""
    Private _AgenciaCodigoCedente As String = ""

    Protected MustOverride Sub formataCampoNossoNumero()
    Protected MustOverride Sub formataCampoAgenciaCodigoCedente()

    Public Sub New(ByVal nn As Long, ByVal vl As Double, ByVal dtv As Date, ByVal ag As Integer, ByVal dvag As String, ByVal cc As Integer, ByVal dvcc As String, ByVal nb As Integer, ByVal dvnb As Integer)
        MyBase.New(nn, vl, dtv)
        _Agencia = ag
        _DVAgencia = dvag
        _Conta = cc
        _DVConta = dvcc
        _Banco = nb
        _DVBanco = dvnb
    End Sub

    Public Property EspecieDocumento() As String
        Get
            Return _EspecieDocumento
        End Get
        Set(ByVal value As String)
            _EspecieDocumento = value
        End Set
    End Property

    ''' <summary>
    ''' Número da Agência sem o Dígito Verificador
    ''' </summary>
    Public ReadOnly Property Agencia() As Integer
        Get
            Return _Agencia
        End Get
    End Property

    ''' <summary>
    ''' Número da Conta sem o Dígito Verificador
    ''' </summary>
    Public ReadOnly Property Conta() As Integer
        Get
            Return _Conta
        End Get
    End Property

    Public Property NumeroDocumento() As String
        Get
            Return _NumeroDocumento
        End Get
        Set(ByVal value As String)
            _NumeroDocumento = value
        End Set
    End Property

    Public Property Aceite() As String
        Get
            Return _Aceite
        End Get
        Set(ByVal value As String)
            _Aceite = value
        End Set
    End Property

    Public Property DataDocumento() As Date
        Get
            Return _DataDocumento
        End Get
        Set(ByVal value As Date)
            _DataDocumento = value
        End Set
    End Property

    Public Property DataProcessamento() As Date
        Get
            Return _DataProcessamento
        End Get
        Set(ByVal value As Date)
            _DataProcessamento = value
        End Set
    End Property

    Public Property UsoBanco() As String
        Get
            Return _UsoBanco
        End Get
        Set(ByVal value As String)
            _UsoBanco = value
        End Set
    End Property

    Public Property Carteira() As String
        Get
            Return _Carteira
        End Get
        Set(ByVal value As String)
            _Carteira = value
        End Set
    End Property

    Public Property Desconto() As Double
        Get
            Return _Desconto
        End Get
        Set(ByVal value As Double)
            _Desconto = value
        End Set
    End Property

    Public Property OutrasDeducoes_Abatimento() As Double
        Get
            Return _OutrasDeducoes_Abatimento
        End Get
        Set(ByVal value As Double)
            _OutrasDeducoes_Abatimento = value
        End Set
    End Property

    Public Property Mora_Multa_Juros() As Double
        Get
            Return _Mora_Multa_Juros
        End Get
        Set(ByVal value As Double)
            _Mora_Multa_Juros = value
        End Set
    End Property

    Public Property OutrosAcrescimos() As Double
        Get
            Return _OutrosAcrescimos
        End Get
        Set(ByVal value As Double)
            _OutrosAcrescimos = value
        End Set
    End Property

    ''' <summary>
    ''' Número do Banco sem o Dígito Verificador
    ''' </summary>
    Public ReadOnly Property Banco() As Integer
        Get
            Return _Banco
        End Get
    End Property

    ''' <summary>
    ''' Dígito Verificador da Conta
    ''' </summary>
    Public ReadOnly Property DVConta() As String
        Get
            Return _DVConta
        End Get
    End Property

    ''' <summary>
    ''' Dígito Verificador da Agência
    ''' </summary>
    Public ReadOnly Property DVAgencia() As String
        Get
            Return _DVAgencia
        End Get
    End Property

    ''' <summary>
    ''' Dígito Verificador do Banco
    ''' </summary>
    Public ReadOnly Property DVBanco() As Integer
        Get
            Return _DVBanco
        End Get
    End Property

    Public Property NossoNumero() As String
        Get
            Return _NossoNumero
        End Get
        Set(ByVal value As String)
            _NossoNumero = value
        End Set
    End Property

    Public Property AgenciaCodigoCedente() As String
        Get
            Return _AgenciaCodigoCedente
        End Get
        Set(ByVal value As String)
            _AgenciaCodigoCedente = value
        End Set
    End Property

End Class
