Public Class Divida

    Private _contrato As String
    Public Property Contrato() As String
        Get
            Return _contrato
        End Get
        Set(ByVal value As String)
            _contrato = value
        End Set
    End Property

    Private _data As String
    Public Property Data() As String
        Get
            Return _data
        End Get
        Set(ByVal value As String)
            _data = value
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

    Public Sub New(ByVal contrato As String, ByVal data As String, ByVal valor As String)
        Me.Contrato = contrato
        Me.Data = data
        Me.Valor = valor
    End Sub

End Class
