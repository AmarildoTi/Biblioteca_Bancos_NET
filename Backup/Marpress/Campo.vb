Namespace Padrao

    Public Class Campo

        Private _nome As String
        Public Property Nome() As String
            Get
                Return _nome
            End Get
            Set(ByVal value As String)
                _nome = value
            End Set
        End Property

        Private _conteudo As String
        Public Property Conteudo() As String
            Get
                Return _conteudo
            End Get
            Set(ByVal value As String)
                _conteudo = value
            End Set
        End Property

        Public Sub New(ByVal nome, ByVal conteudo)
            Me.Nome = nome
            Me.Conteudo = conteudo
        End Sub

    End Class
End Namespace
