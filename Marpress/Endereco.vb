Namespace Padrao

    Public Class Endereco

        Private _logradouro As String
        Public Property Logradouro() As String
            Get
                Return _logradouro
            End Get
            Set(ByVal value As String)
                _logradouro = value
            End Set
        End Property

        Private _numero As String
        Public Property Numero() As String
            Get
                Return _numero
            End Get
            Set(ByVal value As String)
                _numero = value
            End Set
        End Property

        Private _complemento As String
        Public Property Complemento() As String
            Get
                Return _complemento
            End Get
            Set(ByVal value As String)
                _complemento = value
            End Set
        End Property

        Private _bairro As String
        Public Property Bairro() As String
            Get
                Return _bairro
            End Get
            Set(ByVal value As String)
                _bairro = value
            End Set
        End Property

        Private _cidade As String
        Public Property Cidade() As String
            Get
                Return _cidade
            End Get
            Set(ByVal value As String)
                _cidade = value
            End Set
        End Property

        Private _estado As String
        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Private _cep As String
        Public Property CEP() As String
            Get
                Return _cep
            End Get
            Set(ByVal value As String)
                _cep = value
            End Set
        End Property
    End Class

End Namespace
