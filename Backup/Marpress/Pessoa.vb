Namespace Padrao
    Public Class Pessoa

        Private _apelido As String
        Public Property Apelido() As String
            Get
                Return _apelido
            End Get
            Set(ByVal value As String)
                _apelido = value
            End Set
        End Property

        Private _nome As String
        Public Property Nome() As String
            Get
                Return _nome
            End Get
            Set(ByVal value As String)
                _nome = value
            End Set
        End Property

        Private _documento As String
        Public Property Documento() As String
            Get
                Return _documento
            End Get
            Set(ByVal value As String)
                _documento = value
            End Set
        End Property

        Private _endereco As New Endereco
        Public Property Endereco() As Endereco
            Get
                Return _endereco
            End Get
            Set(ByVal value As Endereco)
                _endereco = value
            End Set
        End Property

    End Class

End Namespace
