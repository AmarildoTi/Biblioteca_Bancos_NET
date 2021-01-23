Imports Marpress.Interfaces

Namespace Padrao.Fatura
    Public Class Fatura
        Implements ICIF

        Private _cif As New CIF
        Public Property CIF() As CIF Implements ICIF.CIF
            Get
                Return _cif
            End Get
            Set(ByVal value As CIF)
                _cif = value
            End Set
        End Property

        Private _destinario As New Pessoa
        Public Property Destinatario() As Pessoa Implements ICIF.Destinatario
            Get
                Return _destinario
            End Get
            Set(ByVal value As Pessoa)
                _destinario = value
            End Set
        End Property

        Private _remetente As New Pessoa
        Public Property Remetente() As Pessoa Implements ICIF.Remetente
            Get
                Return _remetente
            End Get
            Set(ByVal value As Pessoa)
                _remetente = value
            End Set
        End Property

        Private _codigocliente As String
        Public Property CodigoCliente() As String
            Get
                Return _codigocliente
            End Get
            Set(ByVal value As String)
                _codigocliente = value
            End Set
        End Property

        Private _idfatura As String
        Public Property IDFatura() As String
            Get
                Return _idfatura
            End Get
            Set(ByVal value As String)
                _idfatura = value
            End Set
        End Property

        Private _cartao As String
        Public Property Cartao() As String
            Get
                Return _cartao
            End Get
            Set(ByVal value As String)
                _cartao = value
            End Set
        End Property

        Private _vencimento As String
        Public Property Vencimento() As String
            Get
                Return _vencimento
            End Get
            Set(ByVal value As String)
                _vencimento = value
            End Set
        End Property

        Private _totalfatura As String
        Public Property TotalFatura() As String
            Get
                Return _totalfatura
            End Get
            Set(ByVal value As String)
                _totalfatura = value
            End Set
        End Property

        Private _totalfaturaanterior As String
        Public Property TotalFaturaAnterior() As String
            Get
                Return _totalfaturaanterior
            End Get
            Set(ByVal value As String)
                _totalfaturaanterior = value
            End Set
        End Property

        Private _totaldebito As String
        Public Property TotalDebito() As String
            Get
                Return _totaldebito
            End Get
            Set(ByVal value As String)
                _totaldebito = value
            End Set
        End Property

        Private _pagamentofaturaanterior As String
        Public Property PagamentoFaturaAnterior() As String
            Get
                Return _pagamentofaturaanterior
            End Get
            Set(ByVal value As String)
                _pagamentofaturaanterior = value
            End Set
        End Property

        Private _pagamentominimo As String
        Public Property PagamentoMinimo() As String
            Get
                Return _pagamentominimo
            End Get
            Set(ByVal value As String)
                _pagamentominimo = value
            End Set
        End Property

        Private _limitecredito As String
        Public Property LimiteCredito() As String
            Get
                Return _limitecredito
            End Get
            Set(ByVal value As String)
                _limitecredito = value
            End Set
        End Property

        Private _proximoslancamentos As String
        Public Property ProximosLancamentos() As String
            Get
                Return _proximoslancamentos
            End Get
            Set(ByVal value As String)
                _proximoslancamentos = value
            End Set
        End Property

        Private _creditodisponivel As String
        Public Property CreditoDisponivel() As String
            Get
                Return _creditodisponivel
            End Get
            Set(ByVal value As String)
                _creditodisponivel = value
            End Set
        End Property

        Private _encargosMes As String
        Public Property EncargosMes() As String
            Get
                Return _encargosMes
            End Get
            Set(ByVal value As String)
                _encargosMes = value
            End Set
        End Property

        Private _encargosproximomes As String
        Public Property EncargosProximoMes() As String
            Get
                Return _encargosproximomes
            End Get
            Set(ByVal value As String)
                _encargosproximomes = value
            End Set
        End Property

        Private _encargosanual As String
        Public Property EncargosAnual() As String
            Get
                Return _encargosanual
            End Get
            Set(ByVal value As String)
                _encargosanual = value
            End Set
        End Property

        Private _multaporatraso As String
        Public Property MultaPorAtraso() As String
            Get
                Return _multaporatraso
            End Get
            Set(ByVal value As String)
                _multaporatraso = value
            End Set
        End Property

        Private _valorcambio As String
        Public Property ValorCambio() As String
            Get
                Return _valorcambio
            End Get
            Set(ByVal value As String)
                _valorcambio = value
            End Set
        End Property

        Private _datacambio As String
        Public Property DataCambio() As String
            Get
                Return _datacambio
            End Get
            Set(ByVal value As String)
                _datacambio = value
            End Set
        End Property

        Private _jurosmora As String
        Public Property JurosMora() As String
            Get
                Return _jurosmora
            End Get
            Set(ByVal value As String)
                _jurosmora = value
            End Set
        End Property

        Private _iofmensal As String
        Public Property IOF_Mensal() As String
            Get
                Return _iofmensal
            End Get
            Set(ByVal value As String)
                _iofmensal = value
            End Set
        End Property

        Private _cetanual As String
        Public Property CET_Anual() As String
            Get
                Return _cetanual
            End Get
            Set(ByVal value As String)
                _cetanual = value
            End Set
        End Property

        Private _cetanualproximomes As String
        Public Property CET_AnualProximoMes() As String
            Get
                Return _cetanualproximomes
            End Get
            Set(ByVal value As String)
                _cetanualproximomes = value
            End Set
        End Property

        Private _cetparcelamento As String
        Public Property CET_Parcelamento() As String
            Get
                Return _cetparcelamento
            End Get
            Set(ByVal value As String)
                _cetparcelamento = value
            End Set
        End Property

        Private _cetsaque As String
        Public Property CET_Saque() As String
            Get
                Return _cetsaque
            End Get
            Set(ByVal value As String)
                _cetsaque = value
            End Set
        End Property

        Private _cetparcelas As String
        Public Property CET_Parcelas() As String
            Get
                Return _cetparcelas
            End Get
            Set(ByVal value As String)
                _cetparcelas = value
            End Set
        End Property

        Private _datacorte As String
        Public Property DataCorte() As String
            Get
                Return _datacorte
            End Get
            Set(ByVal value As String)
                _datacorte = value
            End Set
        End Property

        Private _limitesaque As String
        Public Property LimiteSaque() As String
            Get
                Return _limitesaque
            End Get
            Set(ByVal value As String)
                _limitesaque = value
            End Set
        End Property

        Private _disponivelsaque As String
        Public Property DisponivelSaque() As String
            Get
                Return _disponivelsaque
            End Get
            Set(ByVal value As String)
                _disponivelsaque = value
            End Set
        End Property

        Private _cartoes As New List(Of Cartao)
        Public Property Cartoes() As List(Of Cartao)
            Get
                Return _cartoes
            End Get
            Set(ByVal value As List(Of Cartao))
                _cartoes = value
            End Set
        End Property

        Private _mensagem As New List(Of String)
        Public Property Mensagem() As List(Of String)
            Get
                Return _mensagem
            End Get
            Set(ByVal value As List(Of String))
                _mensagem = value
            End Set
        End Property

        Private _boleto As New Boleto
        Public Property Boleto() As Boleto
            Get
                Return _boleto
            End Get
            Set(ByVal value As Boleto)
                _boleto = value
            End Set
        End Property

    End Class
End Namespace