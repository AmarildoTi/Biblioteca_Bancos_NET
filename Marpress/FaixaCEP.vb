Namespace FAC

    Public Class FaixaCEP

        Private _estado As String
        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property

        Private _cepInicial As Integer()
        Public ReadOnly Property CepInicial() As Integer()
            Get
                Return _cepInicial
            End Get
        End Property

        Private _cepFinal As Integer()
        Public Property CEPFinal() As Integer()
            Get
                Return _cepFinal
            End Get
            Set(ByVal value As Integer())
                _cepFinal = value
            End Set
        End Property

        Public Sub New(ByVal estado As String, ByVal cepInicial As Integer(), ByVal cepFinal As Integer())
            _estado = estado
            _cepInicial = cepInicial
            _cepFinal = cepFinal
        End Sub

    End Class

End Namespace