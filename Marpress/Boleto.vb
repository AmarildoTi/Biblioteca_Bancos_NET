Namespace Padrao

    Public Class Boleto

        Private _banco As String
        Public Property Banco() As String
            Get
                Return _banco
            End Get
            Set(ByVal value As String)
                _banco = value
            End Set
        End Property

        Private _localdepagamento As String
        Public Property LocalDePagamento() As String
            Get
                Return _localdepagamento
            End Get
            Set(ByVal value As String)
                _localdepagamento = value
            End Set
        End Property

        Private _beneficiario As New Pessoa
        Public Property Beneficiario() As Pessoa
            Get
                Return _beneficiario
            End Get
            Set(ByVal value As Pessoa)
                _beneficiario = value
            End Set
        End Property

        Private _agenciacodigobeneficiario As String
        Public Property AgenciaCodigoBeneficiario() As String
            Get
                Return _agenciacodigobeneficiario
            End Get
            Set(ByVal value As String)
                _agenciacodigobeneficiario = value
            End Set
        End Property

        Private _datadocumento As String
        Public Property DataDocumento() As String
            Get
                Return _datadocumento
            End Get
            Set(ByVal value As String)
                _datadocumento = value
            End Set
        End Property

        Private _especiedocumento As String
        Public Property EspecieDocumento() As String
            Get
                Return _especiedocumento
            End Get
            Set(ByVal value As String)
                _especiedocumento = value
            End Set
        End Property

        Private _aceite As String
        Public Property Aceite() As String
            Get
                Return _aceite
            End Get
            Set(ByVal value As String)
                _aceite = value
            End Set
        End Property

        Private _dataprocessamento As String
        Public Property DataProcessamento() As String
            Get
                Return _dataprocessamento
            End Get
            Set(ByVal value As String)
                _dataprocessamento = value
            End Set
        End Property

        Private _usodobanco As String
        Public Property UsoDoBanco() As String
            Get
                Return _usodobanco
            End Get
            Set(ByVal value As String)
                _usodobanco = value
            End Set
        End Property

        Private _carteira As String
        Public Property Carteira() As String
            Get
                Return _carteira
            End Get
            Set(ByVal value As String)
                _carteira = value
            End Set
        End Property

        Private _especie As String
        Public Property Especie() As String
            Get
                Return _especie
            End Get
            Set(ByVal value As String)
                _especie = value
            End Set
        End Property

        Private _parcelas As New List(Of Parcela)
        Public Property Parcelas() As List(Of Parcela)
            Get
                Return _parcelas
            End Get
            Set(ByVal value As List(Of Parcela))
                _parcelas = value
            End Set
        End Property

    End Class

End Namespace