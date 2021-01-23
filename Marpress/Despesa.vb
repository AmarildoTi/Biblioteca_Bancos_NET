Namespace Padrao.Fatura

    Public Class Despesa

        Private _data As String
        Public Property Data() As String
            Get
                Return _data
            End Get
            Set(ByVal value As String)
                _data = value
            End Set
        End Property

        Private _loja As String
        Public Property Loja() As String
            Get
                Return _loja
            End Get
            Set(ByVal value As String)
                _loja = value
            End Set
        End Property

        Private _portador As String
        Public Property Portador() As String
            Get
                Return _portador
            End Get
            Set(ByVal value As String)
                _portador = value
            End Set
        End Property

        Private _numerooperacao As String
        Public Property NumeroOperacao() As String
            Get
                Return _numerooperacao
            End Get
            Set(ByVal value As String)
                _numerooperacao = value
            End Set
        End Property

        Private _caixa As String
        Public Property Caixa() As String
            Get
                Return _caixa
            End Get
            Set(ByVal value As String)
                _caixa = value
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

        Private _descricao As String
        Public Property Descricao() As String
            Get
                Return _descricao
            End Get
            Set(ByVal value As String)
                _descricao = value
            End Set
        End Property

        Private _credito As String
        Public Property Credito() As String
            Get
                Return _credito
            End Get
            Set(ByVal value As String)
                _credito = value
            End Set
        End Property

        Private _debito As String
        Public Property Debito() As String
            Get
                Return _debito
            End Get
            Set(ByVal value As String)
                _debito = value
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

        Private _sinal As String
        Public Property Sinal() As String
            Get
                Return _sinal
            End Get
            Set(ByVal value As String)
                _sinal = value
            End Set
        End Property

        Private _parcela As String
        Public Property Parcela() As String
            Get
                Return _parcela
            End Get
            Set(ByVal value As String)
                _parcela = value
            End Set
        End Property

        Private _categoria As String
        Public Property Categoria() As String
            Get
                Return _categoria
            End Get
            Set(ByVal value As String)
                _categoria = value
            End Set
        End Property

        Private _personalizado As New List(Of Campo)
        Public Property Personalizado() As List(Of Campo)
            Get
                Return _personalizado
            End Get
            Set(ByVal value As List(Of Campo))
                _personalizado = value
            End Set
        End Property

    End Class

End Namespace