Namespace Padrao.Fatura
    Public Class Parcelamento

        Private _data As String
        Public Property Data() As String
            Get
                Return _data
            End Get
            Set(ByVal value As String)
                _data = value
            End Set
        End Property

        Private _parcelas As String
        Public Property Parcelas() As String
            Get
                Return _parcelas
            End Get
            Set(ByVal value As String)
                _parcelas = value
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
