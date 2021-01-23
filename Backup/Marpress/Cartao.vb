Namespace Padrao.Fatura
    Public Class Cartao

        Private _numero As String
        Public Property Numero() As String
            Get
                Return _numero
            End Get
            Set(ByVal value As String)
                _numero = value
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

        Private _tipo As String
        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property

        Private _descricao As String
        Public Property Descricao() As String
            Get
                Return _descricao
            End Get
            Set(ByVal value As String)
                _descricao = value
            End Set
        End Property

        Private _despesa As New List(Of Despesa)
        Public Property Despesa() As List(Of Despesa)
            Get
                Return _despesa
            End Get
            Set(ByVal value As List(Of Despesa))
                _despesa = value
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

    End Class

End Namespace